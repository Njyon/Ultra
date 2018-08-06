using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationDebugScript : MonoBehaviour
{
    [Header("The Character That Needs to be Debuged")]
    public GameObject debugCharacter;
    [Header("The Location where the Character gets Instatiated")]
    public GameObject spawnLocation;
    [Header("Switch The Player Enum if no Controll")]
    public PlayerEnum playerEnum;

    public bool TESTMOV2;
    GameObject debugPlayer;
   
    private void Start()
    {
        Initiate();
    }

    void Initiate()
    {
        GameObject camera = GameObject.Find("CamerHolder");
        MultiTargetCamera sCam = camera.GetComponent<MultiTargetCamera>();

        InGameUI ui = GameObject.Find("PlayerTwoPannel").GetComponent<InGameUI>();

        GameObject enemy = GameObject.Find("pref_DebugChar");
        MyCharacter enemyChar = enemy.GetComponent<MyCharacter>();
        
        if (!TESTMOV2)
        {
            debugPlayer = Instantiate(debugCharacter, spawnLocation.gameObject.transform.position, spawnLocation.gameObject.transform.rotation);
            debugPlayer.GetComponent<MyCharacter>().playerEnum = playerEnum;
            debugPlayer.GetComponent<MyCharacter>().enemy = enemy;
            debugPlayer.GetComponent<MyCharacter>().playerDataAction += DataCounter;
            debugPlayer.GetComponent<MyCharacter>().dodgeAction += DodgeCounter;
            debugPlayer.GetComponent<MyCharacter>().bounceAction += BounceCounter;
            debugPlayer.GetComponent<MyCharacter>().shakeCameraAction += sCam.Shake;
            debugPlayer.GetComponent<MyCharacter>().enemyCharacter = enemyChar;
            debugPlayer.GetComponent<MyCharacter>().SetUI(ui);
            debugPlayer.GetComponent<MyCharacter>().Posses();
            ui.GetCharacter(debugPlayer.GetComponent<MyCharacter>());

            enemyChar.enemy = debugCharacter;
            enemyChar.enemyCharacter = debugPlayer.GetComponent<MyCharacter>();

            switch(playerEnum)
            {
                case PlayerEnum.PlayerOne:
                    enemyChar.playerEnum = PlayerEnum.PlayerTwo;
                    break;
                case PlayerEnum.PlayerTwo:
                    enemyChar.playerEnum = PlayerEnum.PlayerOne;
                    break;
            }

            enemyChar.playerDataAction += DataCounter;
            enemyChar.dodgeAction += DodgeCounter;
            enemyChar.bounceAction += BounceCounter;
            enemyChar.shakeCameraAction += sCam.Shake;
            enemy.GetComponent<DebugChar>().InitDebug();

            try
            {
                sCam.AddTarget(debugPlayer.transform);
                sCam.AddTarget(enemy.transform);
            }
            catch
            {
                Debug.Log("No SuperCam Script at Camera. Camera = Static");
                throw;
            }
        }
        //else
        //{
        //    debugPlayer = Instantiate(debugCharacter, spawnLocation.gameObject.transform.position, spawnLocation.gameObject.transform.rotation);
        //    debugPlayer.GetComponent<TestMov>().playerEnum = playerEnum;
        //    debugPlayer.GetComponent<TestMov>().Posses();
        //    try
        //    {S
        //        SuperCam sCam = camera.GetComponent<SuperCam>();
        //        sCam.AddPlayer(debugPlayer);
        //    }
        //    catch
        //    {
        //        Debug.Log("No SuperCam Script at Camera. Camera = Static");
        //        throw;
        //    }
        //}
    }

    // Fake so nothing goes wrong
    void DataCounter(PlayerEnum pE, int combo, int multiplier, int score)
    {

    }

    void DodgeCounter(PlayerEnum pE)
    {
       
    }
    void BounceCounter(PlayerEnum pE)
    {
  
    }
}
