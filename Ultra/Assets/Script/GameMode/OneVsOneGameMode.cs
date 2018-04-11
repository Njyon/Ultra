using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        playerInfoManagerObj = GameObject.Find("PlayerInfoManager");
        if(playerInfoManagerObj == null)
        {
            Debug.Log("<color=red> Player Info Manager GameObject Not Found </color>");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        //Get PlayerInfoManager
        playerInfoManager = playerInfoManagerObj.GetComponent<PlayerInfoManager>();
        if(playerInfoManager == null)
        {
            Debug.Log("<color=red> Player Info Manager Script Not Found </color>");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        //PlayerOne
        Initiate(playerInfoManager.playerOne.character, true);
        //PlayerTwo
        Initiate(playerInfoManager.playerTwo.character, false);
    }

    //Spawn both Player
    void Initiate(Characters character, bool isPlayerOne)
    {
        if (keeram == null || nav == null)
        {
            Debug.Log("<color=red> Characters not Assigned to GameMode </color>");
            UnityEditor.EditorApplication.isPlaying = false;
        }

        switch (character)
        {
            case Characters.Keeram:
                if(isPlayerOne)
                {
                    PlayerOne = Instantiate(keeram, SpawnLocationP1.transform.position, SpawnLocationP1.transform.rotation);
                    PlayerOne.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerOne;
                    PlayerOne.GetComponent<MyCharacter>().Posses();
                }
                else
                {
                    PlayerTwo = Instantiate(keeram, SpawnLocationP2.transform.position, SpawnLocationP2.transform.rotation);
                    PlayerTwo.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerTwo;
                    PlayerTwo.GetComponent<MyCharacter>().Posses();
                }
                break;
            case Characters.Nav:
                if (isPlayerOne)
                {
                    PlayerOne = Instantiate(nav, SpawnLocationP1.transform.position, SpawnLocationP1.transform.rotation);
                    PlayerOne.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerOne;
                    PlayerOne.GetComponent<MyCharacter>().Posses();
                }
                else
                {
                    PlayerTwo = Instantiate(nav, SpawnLocationP2.transform.position, SpawnLocationP2.transform.rotation);
                    PlayerTwo.GetComponent<MyCharacter>().playerEnum = PlayerEnum.PlayerTwo;
                    PlayerTwo.GetComponent<MyCharacter>().Posses();
                }

                break;
            case Characters.None:

                if(isPlayerOne)
                {
                    Debug.Log("<color=red> Player One No Character Assigned </color>");
                    UnityEditor.EditorApplication.isPlaying = false;
                }
                else
                {
                    Debug.Log("<color=red> Player Two No Character Assigned </color>");
                    UnityEditor.EditorApplication.isPlaying = false;
                }
                break;
        }
    }
}
