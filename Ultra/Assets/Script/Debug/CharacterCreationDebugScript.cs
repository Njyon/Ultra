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

    GameObject debugPlayer;

    private void Start()
    {
        Initiate();
    }

    void Initiate()
    {
        debugPlayer = Instantiate(debugCharacter, spawnLocation.gameObject.transform.position, spawnLocation.gameObject.transform.rotation);
        debugPlayer.GetComponent<MyCharacter>().playerEnum = playerEnum;
        debugPlayer.GetComponent<MyCharacter>().Posses();
    }
}
