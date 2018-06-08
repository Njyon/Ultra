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
    
    Text timer;

    [Header("Timer")]
    [SerializeField] float time;
    
    void Start()
    {
        //Set SpawnLocation
        SpawnLocationP1 = GameObject.Find("Spawn P1");
        SpawnLocationP2 = GameObject.Find("Spawn P2");

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

        GameObject camera = GameObject.Find("CameraRig");
        CameraControll sCam = camera.GetComponent<CameraControll>();
        switch (character)
        {
            case Characters.Keeram:
                if(isPlayerOne)
                {
                    PlayerOne = Instantiate(keeram, SpawnLocationP1.transform.position, SpawnLocationP1.transform.rotation);
                    PlayerOne.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerOne;
                    PlayerOne.GetComponent<MyCharacter>().SetUI(playerOneUI);
                    PlayerOne.GetComponent<MyCharacter>().Posses();
                    sCam.playerOne = PlayerOne;
                }
                else
                {
                    PlayerTwo = Instantiate(keeram, SpawnLocationP2.transform.position, SpawnLocationP2.transform.rotation);
                    PlayerTwo.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerTwo;
                    PlayerTwo.GetComponent<MyCharacter>().SetUI(playerTwoUI);
                    PlayerTwo.GetComponent<MyCharacter>().Posses();
                    sCam.playerTwo = PlayerTwo;

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
                    PlayerOne.GetComponent<MyCharacter>().Posses();
                    sCam.playerOne = PlayerOne;

                    Renderer[] rend = PlayerOne.GetComponentsInChildren<Renderer>();
                    rend[1].material = new Material(rend[1].material);
                    rend[1].material.SetColor("_EmissionColor", Color.red);
                    rend[1].material.color = Color.red;
                }
                else
                {
                    PlayerTwo = Instantiate(nav, SpawnLocationP2.transform.position, SpawnLocationP2.transform.rotation);
                    PlayerTwo.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerTwo;
                    PlayerTwo.GetComponent<MyCharacter>().SetUI(playerTwoUI);
                    PlayerTwo.GetComponent<MyCharacter>().Posses();
                    sCam.playerTwo = PlayerTwo;

                    Renderer[] rend = PlayerTwo.GetComponentsInChildren<Renderer>();
                    rend[1].material = new Material(rend[1].material);
                    rend[1].material.SetColor("_EmissionColor", Color.cyan);
                    rend[1].material.color = Color.cyan;
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
        PlayerOne.GetComponent<MyCharacter>().DePosses();
        PlayerTwo.GetComponent<MyCharacter>().DePosses();
        SceneManager.LoadScene(0);
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
