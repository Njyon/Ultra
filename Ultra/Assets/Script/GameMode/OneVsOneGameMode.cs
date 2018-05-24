using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OneVsOneGameMode : MonoBehaviour
{
    GameObject playerInfoManagerObj;
    PlayerInfoManager playerInfoManager;
    GameObject SpawnLocationP1;
    GameObject SpawnLocationP2;
    GameObject PlayerOne;
    GameObject PlayerTwo;

    [Header("Keeram")]
    public GameObject keeram;
    [Header("Nav")]
    public GameObject nav;
    
    void Awake()
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

        //PlayerOne
        Initiate(Characters.Nav, true);
        //PlayerTwo
        Initiate(Characters.Nav, false);
    }

    //Spawn both Player
    void Initiate(Characters character, bool isPlayerOne)
    {
        if (keeram == null || nav == null)
        {
            Debug.Log("<color=red> Characters not Assigned to GameMode </color>");
        }
        InGameUI playerOneUI = GameObject.Find("PlayerOneUI").GetComponent<InGameUI>();
        InGameUI playerTwoUI = GameObject.Find("PlayerTwoUI").GetComponent<InGameUI>();

        GameObject camera = GameObject.Find("Main Camera");
        SuperCam sCam = camera.GetComponent<SuperCam>();
        switch (character)
        {
            case Characters.Keeram:
                if(isPlayerOne)
                {
                    PlayerOne = Instantiate(keeram, SpawnLocationP1.transform.position, SpawnLocationP1.transform.rotation);
                    PlayerOne.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerOne;
                    PlayerOne.GetComponent<MyCharacter>().SetUI(playerOneUI);
                    PlayerOne.GetComponent<MyCharacter>().Posses();
                    sCam.AddPlayer(PlayerOne);
                }
                else
                {
                    PlayerTwo = Instantiate(keeram, SpawnLocationP2.transform.position, SpawnLocationP2.transform.rotation);
                    PlayerTwo.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerTwo;
                    PlayerTwo.GetComponent<MyCharacter>().SetUI(playerTwoUI);
                    PlayerTwo.GetComponent<MyCharacter>().Posses();
                    sCam.AddPlayer(PlayerTwo);

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
                    sCam.AddPlayer(PlayerOne);

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
                    sCam.AddPlayer(PlayerTwo);

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

    void EndGame()
    {
        PlayerOne.GetComponent<MyCharacter>().DePosses();
        PlayerTwo.GetComponent<MyCharacter>().DePosses();
        SceneManager.LoadScene(0);
    }
}
