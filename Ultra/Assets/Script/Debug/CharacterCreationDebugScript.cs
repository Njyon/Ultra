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
        GameObject camera = GameObject.Find("Main Camera");
        if (!TESTMOV2)
        {
            debugPlayer = Instantiate(debugCharacter, spawnLocation.gameObject.transform.position, spawnLocation.gameObject.transform.rotation);
            debugPlayer.GetComponent<MyCharacter>().playerEnum = playerEnum;
            debugPlayer.GetComponent<MyCharacter>().Posses();
            try
            {
                SuperCam sCam = camera.GetComponent<SuperCam>();
                sCam.AddPlayer(debugPlayer);
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
}
