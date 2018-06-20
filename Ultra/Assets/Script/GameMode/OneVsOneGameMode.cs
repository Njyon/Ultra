using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [Header("Keeram")]
    [SerializeField] GameObject keeram;
    [Header("Nav")]
    [SerializeField] GameObject nav;
    
    [Header("Timer")]
    [SerializeField] float time;
    Text timer;

    //[Header("PlayerData")]
    //[SerializeField] GameObject playerDataPref;
    //GameObject playerDataObj;
    //PlayerDataManager playerDataManager;

    void Start()
    {
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

        GameObject camera = GameObject.Find("Main Camera");
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
                    PlayerOne.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerOne;
                    PlayerOne.GetComponent<MyCharacter>().SetUI(playerOneUI);
                    PlayerOne.GetComponent<MyCharacter>().playerDataAction += DataCounter;
                    PlayerOne.GetComponent<MyCharacter>().dodgeAction += DodgeCounter;
                    PlayerOne.GetComponent<MyCharacter>().bounceAction += BounceCounter;
                    PlayerOne.GetComponent<MyCharacter>().Posses();
                    sCam.AddTarget(PlayerOne.transform);


                    Renderer rend = PlayerOne.GetComponent<Dash>().rendererCloth;

                    rend.material.SetColor("_EmissionColor", PlayerInfoManager.playerTwo.color);
                    rend.material.color = PlayerInfoManager.playerTwo.color;
                }
                else
                {
                    PlayerTwo = Instantiate(nav, SpawnLocationP2.transform.position, SpawnLocationP2.transform.rotation);
                    PlayerTwo.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerTwo;
                    PlayerTwo.GetComponent<MyCharacter>().SetUI(playerTwoUI);
                    PlayerTwo.GetComponent<MyCharacter>().playerDataAction += DataCounter;
                    PlayerTwo.GetComponent<MyCharacter>().dodgeAction += DodgeCounter;
                    PlayerTwo.GetComponent<MyCharacter>().bounceAction += BounceCounter;
                    PlayerTwo.GetComponent<MyCharacter>().Posses();
                    sCam.AddTarget(PlayerTwo.transform);

                    Renderer rend = PlayerTwo.GetComponent<Dash>().rendererCloth;

                    rend.material.SetColor("_EmissionColor", PlayerInfoManager.playerTwo.color);
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
    // End game and transition to next scene
    void EndGame()
    {
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

        // Load the Win Screen
        SceneManager.LoadScene(2);
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
                PlayerDataManager.playerOne.Score += score;
                break;
            case PlayerEnum.PlayerTwo:
                if (combo > PlayerDataManager.playerTwo.HighestCombo)
                    PlayerDataManager.playerTwo.HighestCombo = combo;
                if (multiplier > PlayerDataManager.playerTwo.HighestMultiplier)
                    PlayerDataManager.playerTwo.HighestMultiplier = multiplier;
                PlayerDataManager.playerTwo.Score += score;
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

    void Update()
    {
        if(timer != null)
        {
            time -= Time.deltaTime;

            string minutes = ((int)time / 60).ToString();
            string second = ((int)time % 60).ToString();

            timer.text = minutes + ":" + second;

            if(time - Time.deltaTime <= 0)
            {
                // Todo: Pause Game or End animation etc
                timer.text = "0:00";
                EndGame();
            }
        }
    }
}
