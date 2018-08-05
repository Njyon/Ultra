using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OneVsOneGameMode : MonoBehaviour
{
    GameObject playerInfoManagerObj;
    PlayerInfoManager playerInfoManager;
    GameObject SpawnLocationP1;
    GameObject SpawnLocationP2;
    GameObject PlayerOne;
    GameObject PlayerTwo;
    MyCharacter CharacterOne;
    MyCharacter CharacterTwo;

    [Header("Keeram")]
    [SerializeField] GameObject keeram;
    [Header("Nav")]
    [SerializeField] GameObject nav;
    
    [Header("Timer")]
    [SerializeField] float time;
    public GameObject Border;
    Text timer;
    public Animator BigTimer;

    [Header("MaxScore Differece befor 2x Points")]
    [SerializeField] int maxDifference;
    [SerializeField] int comeBackTime;
    [SerializeField] int comeBackTCooldown;
    [HideInInspector] public bool comeBackActive = false;
    bool comeBackCooling = false;

    [Header("ParryParticle")]
    [SerializeField] GameObject parry;

    bool gameOn = false;

    public Animator gameDoneAnimator;
    public Animator pauseMenuAnimator;
    public EventSystem eS;
    public GameObject mainButton;

    private float ActionTimer = 0;

    //[Header("PlayerData")]
    //[SerializeField] GameObject playerDataPref;
    //GameObject playerDataObj;
    //PlayerDataManager playerDataManager;

    void Start()
    {
        InputManager.p1_OnKeyPressed += GetInput;
        InputManager.p2_OnKeyPressed += GetInput;

        AudioEvents.actionTimer += SetActionTimer;

        #region Spawns
        //Set SpawnLocation
        SpawnLocationP1 = GameObject.Find("Spawn P1");
        SpawnLocationP2 = GameObject.Find("Spawn P2");
        #endregion
        #region Player Info Manager
        //Get the GameObject with player Data
        playerInfoManagerObj = GameObject.Find("PlayerInfoManager(Clone)");
        if(playerInfoManagerObj == null)
        {
            Debug.Log("<color=red> Player Info Manager GameObject Not Found </color>");
        }

        //Get PlayerInfoManager
        playerInfoManager = playerInfoManagerObj.GetComponent<PlayerInfoManager>();
        if(playerInfoManager == null)
        {
            Debug.Log("<color=red> Player Info Manager Script Not Found </color>");
        }
        #endregion
        #region PlayerData
        //if (GameObject.Find("PlayerData (Clone)") == null)
        //{
        //    playerDataObj = Instantiate(playerDataPref, Vector3.zero, Quaternion.identity);
        //    playerData = playerDataObj.GetComponent<PlayerData>();
        //    playerData.ResetValues();
        //}
        //else
        //{
        //    playerDataObj = GameObject.Find("PlayerData (Clone)");
        //    playerData = playerDataObj.GetComponent<PlayerData>();
        //    playerData.ResetValues();
        //}
        #endregion
        #region Timer
        // find Timer and get the Text Component
        try
        {
            timer = GameObject.Find("Timer").GetComponent<Text>();
        }
        catch
        {
            Debug.Log("No Timer Found");
        }
        if(timer == null)
        {
            Debug.Log("<color=red> No Timer Found! </color>");
        }
        else
        {
            timer.text = time.ToString();
        }
        #endregion

        //PlayerOne
        Initiate(PlayerInfoManager.playerOne.character, true);
        //PlayerTwo
        Initiate(PlayerInfoManager.playerTwo.character, false);
        SetPlayerEnemys();
        
        CharacterOne.parryDelegate += Parry;
        CharacterTwo.parryDelegate += Parry;

        Invoke("GameStart", 5f);
    }
    void SetActionTimer()
    {
        ActionTimer = 15f;
    }

    void SetPlayerEnemys()
    {

        CharacterOne.enemy = CharacterTwo.gameObject;
        CharacterTwo.enemy = CharacterOne.gameObject;

        CharacterOne.enemyCharacter = CharacterTwo;
        CharacterTwo.enemyCharacter = CharacterOne;
    }

    //Spawn both Player
    void Initiate(Characters character, bool isPlayerOne)
    {
        if (keeram == null || nav == null)
        {
            Debug.Log("<color=red> Characters not Assigned to GameMode </color>");
        }
        InGameUI playerOneUI = new InGameUI();
        InGameUI playerTwoUI = new InGameUI();

        try
        {
            playerOneUI = GameObject.Find("PlayerOnePannel").GetComponent<InGameUI>();
            playerTwoUI = GameObject.Find("PlayerTwoPannel").GetComponent<InGameUI>();
        }
        catch
        {
            Debug.Log("No UI Pannels Found!");
        }

        GameObject camera = GameObject.Find("CamerHolder");
        MultiTargetCamera sCam = camera.GetComponent<MultiTargetCamera>();
        switch (character)
        {
            case Characters.Keeram:
                if(isPlayerOne)
                {
                    PlayerOne = Instantiate(keeram, SpawnLocationP1.transform.position, SpawnLocationP1.transform.rotation);
                    PlayerOne.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerOne;
                    PlayerOne.GetComponent<MyCharacter>().SetUI(playerOneUI);
                    PlayerOne.GetComponent<MyCharacter>().Posses();
                    sCam.AddTarget(PlayerOne.transform);
                }
                else
                {
                    PlayerTwo = Instantiate(keeram, SpawnLocationP2.transform.position, SpawnLocationP2.transform.rotation);
                    PlayerTwo.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerTwo;
                    PlayerTwo.GetComponent<MyCharacter>().SetUI(playerTwoUI);
                    PlayerTwo.GetComponent<MyCharacter>().Posses();
                    sCam.AddTarget(PlayerTwo.transform);

                    //Renderer[] rend = PlayerTwo.GetComponentsInChildren<Renderer>();
                    //rend[1].material = new Material(Shader.Find("Standard"));
                    //rend[1].material.color = Color.magenta;
                }
                break;
            case Characters.Nav:
                if (isPlayerOne)
                {
                    PlayerOne = Instantiate(nav, SpawnLocationP1.transform.position, SpawnLocationP1.transform.rotation);
                    CharacterOne = PlayerOne.GetComponent<MyCharacter>();
                    CharacterOne.playerEnum = PlayerEnum.PlayerOne;
                    CharacterOne.SetUI(playerOneUI);
                    CharacterOne.playerDataAction += DataCounter;
                    CharacterOne.dodgeAction += DodgeCounter;
                    CharacterOne.bounceAction += BounceCounter;
                    CharacterOne.shakeCameraAction += sCam.Shake;
                    CharacterOne.gameMode = this;
                    playerOneUI.SetHUDColor(PlayerInfoManager.playerOne.color);
                    sCam.AddTarget(PlayerOne.transform);


                    Renderer rend = PlayerOne.GetComponent<Dash>().rendererCloth;

                    rend.material.SetColor("_EmissionColor", PlayerInfoManager.playerOne.color);
                    rend.materials[1].SetColor("_EmissionColor", PlayerInfoManager.playerOne.color);
                    rend.material.color = PlayerInfoManager.playerOne.color;
                }
                else
                {
                    PlayerTwo = Instantiate(nav, SpawnLocationP2.transform.position, SpawnLocationP2.transform.rotation);
                    CharacterTwo = PlayerTwo.GetComponent<MyCharacter>();
                    CharacterTwo.playerEnum = PlayerEnum.PlayerTwo;
                    CharacterTwo.SetUI(playerTwoUI);
                    CharacterTwo.playerDataAction += DataCounter;
                    CharacterTwo.dodgeAction += DodgeCounter;
                    CharacterTwo.bounceAction += BounceCounter;
                    CharacterTwo.shakeCameraAction += sCam.Shake;
                    CharacterTwo.gameMode = this;
                    playerTwoUI.SetHUDColor(PlayerInfoManager.playerTwo.color);
                    sCam.AddTarget(PlayerTwo.transform);

                    Renderer rend = PlayerTwo.GetComponent<Dash>().rendererCloth;

                    rend.material.SetColor("_EmissionColor", PlayerInfoManager.playerTwo.color);
                    rend.materials[1].SetColor("_EmissionColor", PlayerInfoManager.playerTwo.color);
                    rend.material.color = PlayerInfoManager.playerTwo.color;
                }

                break;
            case Characters.None:

                if(isPlayerOne)
                {
                    Debug.Log("<color=red> Player One No Character Assigned </color>");
                }
                else
                {
                    Debug.Log("<color=red> Player Two No Character Assigned </color>");
                }
                break;
        }

        MyCharacter.endGameAction += EndGame;
    }

    void GetInput(KeyCode keyCode)
    {
        if(keyCode == KeyCode.Joystick1Button7 || keyCode == KeyCode.Joystick2Button7)
        {
            PauseGame();
        }
        if(!gameOn && Time.timeScale == 0f)
        {
            if(keyCode == KeyCode.Joystick1Button1 || keyCode == KeyCode.Joystick2Button1)
            {
                GameResume();
            }
        }
    }
    void PauseGame()
    {
        eS.SetSelectedGameObject(mainButton);
        gameOn = false;
        Time.timeScale = 0f;
        pauseMenuAnimator.SetBool("pauseActive", true);
    }
    void GameResumeDelayed()
    {
        eS.SetSelectedGameObject(null);
        gameOn = true;
        Time.timeScale = 1f;
    }
    public void GameResume()
    {
        GameResumeDelayed();
        //Invoke("GameResumeDelayed", 0.9f);
        pauseMenuAnimator.SetBool("pauseActive", false);
    }
    public void LeaveGame()
    {
        gameOn = false;
        Fabric.EventManager.Instance.PostEvent("MusicNoAction");
        // End Combo from both Players
        PlayerOne.GetComponent<MyCharacter>().EndCombo();
        PlayerTwo.GetComponent<MyCharacter>().EndCombo();

        // Desubscribe both player from the Score counter events
        PlayerOne.GetComponent<MyCharacter>().playerDataAction -= DataCounter;
        PlayerOne.GetComponent<MyCharacter>().dodgeAction -= DodgeCounter;
        PlayerOne.GetComponent<MyCharacter>().bounceAction -= BounceCounter;
        PlayerTwo.GetComponent<MyCharacter>().playerDataAction -= DataCounter;
        PlayerTwo.GetComponent<MyCharacter>().dodgeAction -= DodgeCounter;
        PlayerTwo.GetComponent<MyCharacter>().bounceAction -= BounceCounter;

        //Deposses both player and Desubsribe them so from events
        PlayerOne.GetComponent<MyCharacter>().DePosses();
        PlayerTwo.GetComponent<MyCharacter>().DePosses();

        InputManager.p1_OnKeyPressed -= GetInput;
        InputManager.p2_OnKeyPressed -= GetInput;
        Time.timeScale = 1f;

        StartCoroutine(LoadNewScene(1));
    }
    IEnumerator LoadNewScene(int scene)
    {
        yield return new WaitForSecondsRealtime(0.1f);
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone)
        {
            yield return null;
        }
    }


    void GameStart()
    {
        gameOn = true;
        CharacterOne.Posses();
        CharacterTwo.Posses();
    }
    // End game and transition to next scene
    void EndGame()
    {
        if (!gameOn)
            return;

        gameOn = false;
        Fabric.EventManager.Instance.PostEvent("MusicNoAction");
        // End Combo from both Players
        PlayerOne.GetComponent<MyCharacter>().EndCombo();
        PlayerTwo.GetComponent<MyCharacter>().EndCombo();

        // Desubscribe both player from the Score counter events
        PlayerOne.GetComponent<MyCharacter>().playerDataAction -= DataCounter;
        PlayerOne.GetComponent<MyCharacter>().dodgeAction -= DodgeCounter;
        PlayerOne.GetComponent<MyCharacter>().bounceAction -= BounceCounter;
        PlayerTwo.GetComponent<MyCharacter>().playerDataAction -= DataCounter;
        PlayerTwo.GetComponent<MyCharacter>().dodgeAction -= DodgeCounter;
        PlayerTwo.GetComponent<MyCharacter>().bounceAction -= BounceCounter;
        
        //Deposses both player and Desubsribe them so from events
        PlayerOne.GetComponent<MyCharacter>().DePosses();
        PlayerTwo.GetComponent<MyCharacter>().DePosses();
        
        InputManager.p1_OnKeyPressed -= GetInput;
        InputManager.p2_OnKeyPressed -= GetInput;

        fixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.deltaTime * 0.02f;
        gameDoneAnimator.SetBool("GameDone", true);
        Invoke("LoadScene", 2f);
    }
    float fixedDeltaTime;
    void LoadScene()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = fixedDeltaTime;
        // Load the Win Screen
        SceneManager.LoadScene(3);
    }
    void DataCounter(PlayerEnum pE, int combo, int multiplier, int score)
    {
        switch (pE)
        {
            case PlayerEnum.PlayerOne:
                if (combo > PlayerDataManager.playerOne.HighestCombo)
                    PlayerDataManager.playerOne.HighestCombo = combo;
                if (multiplier > PlayerDataManager.playerOne.HighestMultiplier)
                    PlayerDataManager.playerOne.HighestMultiplier = multiplier;
                PlayerDataManager.playerOne.Score = score;
                break;
            case PlayerEnum.PlayerTwo:
                if (combo > PlayerDataManager.playerTwo.HighestCombo)
                    PlayerDataManager.playerTwo.HighestCombo = combo;
                if (multiplier > PlayerDataManager.playerTwo.HighestMultiplier)
                    PlayerDataManager.playerTwo.HighestMultiplier = multiplier;
                PlayerDataManager.playerTwo.Score = score;
                break;
        }
    }
    void DodgeCounter(PlayerEnum pE)
    {
        switch(pE)
        {
            case PlayerEnum.PlayerOne:
                PlayerDataManager.playerOne.AmountOfDodges++;
                break;
            case PlayerEnum.PlayerTwo:
                PlayerDataManager.playerTwo.AmountOfDodges++;
                break;
        }
    }
    void BounceCounter(PlayerEnum pE)
    {
        switch (pE)
        {
            case PlayerEnum.PlayerOne:
                PlayerDataManager.playerOne.Bounces++;
                break;
            case PlayerEnum.PlayerTwo:
                PlayerDataManager.playerTwo.Bounces++;
                break;
        }
    }
    bool parried = false;
    void Parry()
    {
        if (!parried)
        {
            var bounds = new Bounds(CharacterOne.transform.position, Vector3.zero); 
            parried = true;
            Fabric.EventManager.Instance.PostEvent("Parry", this.gameObject);
            bounds.Encapsulate(CharacterOne.transform.position);
            bounds.Encapsulate(CharacterTwo.transform.position);

            Instantiate(parry, bounds.center, Quaternion.identity);
        }
        else
        {
            parried = false;
        }
    }
    int GetHigherScore(out bool playerOneWins)
    {
        if(CharacterOne.score > CharacterTwo.score)
        {
            playerOneWins = true;
            return CharacterOne.score;
        }
        else
        {
            playerOneWins = false;
            return CharacterTwo.score;
        }       
    }
    int GetScoreDifference(bool playerOneWins)
    {
        if (playerOneWins)
        {
            return CharacterOne.score - CharacterTwo.score;
        }
        else
        {
            return CharacterTwo.score - CharacterOne.score;
        }
    }
    bool ShouldCheckBalance()
    {
        if (CharacterOne.score == 0 || CharacterTwo.score == 0)
            return false;
        else
            return true;
    }
    public void CheckForBalance()
    {
        if (!ShouldCheckBalance())
            return;
        
        bool playerOneWins;
        int winnerScore = GetHigherScore(out playerOneWins);
        int difference = GetScoreDifference(playerOneWins);

        if(!playerOneWins)
        {
            if (difference > winnerScore * 0.4f)
                StartComeBack(CharacterOne);
        }
        else
        {
            if (difference > winnerScore * 0.4f)
                StartComeBack(CharacterTwo);
        }
    }

    void StartComeBack(MyCharacter characterToBoost)
    {
        if(!comeBackActive && !comeBackCooling)
        {
            comeBackActive = true;
            characterToBoost.inComeBackMode = true;
            characterToBoost.pD.doublePoints.Play();
            Invoke("EndComeBackMode", comeBackTime);
        }
    }

    void EndComeBackMode()
    {
        comeBackCooling = true;
        comeBackActive = false;

        //Fast Fix
        CharacterOne.CheckIfComebackModeShouldEnd();
        CharacterOne.CheckIfComebackModeShouldEnd();

        Invoke("EndComeBackCooldown", comeBackTCooldown);
    }
    void EndComeBackCooldown()
    {
        comeBackCooling = false;
    }

    bool bigTimerOn = false;
    void Update()
    {
        if (!gameOn)
            return;

        if(ActionTimer > 0f)
        {
            ActionTimer -= Time.deltaTime;
        }
        else
        {
            if(AudioEvents.firstGetHit)
            {
                AudioEvents.firstGetHit = false;
                Fabric.EventManager.Instance.PostEvent("MusicNoAction");
            }
        }

        if(timer != null)
        {
            time -= Time.deltaTime;

            string minutes = ((int)time / 60).ToString();
            string second = ((int)time % 60).ToString();

            if((int)time % 60 < 10)
            {
                timer.text = minutes + ":0" + second;
            }
            else
            {
                timer.text = minutes + ":" + second;
            }

            //Color TIme in Last Secods Red (only fast hack for the Gate)
            if((int)time / 60 == 0 && (int)time % 60 <= 30 && timer.color != Color.red)
            {
                StartCoroutine(TimerBlink());
            }
            if((int)time / 60 == 0 && (int)time % 60 <= 5f && !bigTimerOn)
            {
                bigTimerOn = true;
                timer.gameObject.SetActive(false);
                Border.SetActive(false);
                StopCoroutine(TimerBlink());
                BigTimer.SetBool("Go", true);
            }

            if (time - Time.deltaTime <= 0)
            {
                // Todo: Pause Game or End animation etc
                timer.text = "0:00";
                EndGame();
            }
        }

        //Debug
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            EndGame();
        }
    }

    IEnumerator TimerBlink()
    {
        while(time > 0)
        {
            timer.color = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time * 2, 1));
            yield return null;
        }
        yield return null;
    }
}
