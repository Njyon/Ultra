using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelecterV2 : MonoBehaviour
{
    private MenuManager menuManager;

    [Header("AmountOfPlayable Characters")]
    public int amountOfPlayableCharacters;

    [Header("Keeram")]
    public GameObject p1Keeram;
    public GameObject p2Keeram;

    [Header("Nav")]
    public GameObject p1Nav;
    public GameObject p2Nav;

    //Player Slots
    MenuSelecter playerOne;
    MenuSelecter playerTwo;

    #region Subscibe & Unsubscribe from Delegates
    /// <summary>
    /// Subscribe to Delegate
    /// </summary>
    void OnEnable()
    {
        InputManager.P1_AButtonDownAction += playerOne.SelectSlot;
    }

    /// <summary>
    /// UnSubscribe from Delegate
    /// </summary>
    void OnDisable()
    {
        InputManager.P1_AButtonDownAction -= playerOne.SelectSlot;
    }
#endregion


    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {

        }
    }

    void Awake()
    {

        Debug.Log(playerOne.characterEnum);
        playerOne.SetArray();
        playerTwo.SetArray();

#region Set Character Array
        playerOne.characters[0] = p1Keeram;
        playerOne.characters[1] = p1Nav;

        playerTwo.characters[0] = p2Keeram;
        playerTwo.characters[1] = p2Nav;

#endregion

#region Check befor Playing
        if (p1Keeram == null || p2Keeram == null)
        {
            Debug.Log("<color=red>Not Enough Characters in CharacterSelecter</color> ");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        for (int i = 0; i < amountOfPlayableCharacters; i++)
        {
            if (playerOne.characters[i] == null || playerTwo.characters[i] == null)
            {
                Debug.Log("<color=red>Not Enough Characters in CharacterSelecter Array</color> ");
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
#endregion
        
#region Getter Stuff
        GameObject menuManagerObj = GameObject.Find("MenuManager");
        menuManager = menuManagerObj.GetComponent<MenuManager>();

        playerOne.characterPosition = playerOne.characters[playerOne.slotIndex].transform.position;

        #endregion

        #region Setter Stuff

        #endregion

        for (int i = 1; i < playerOne.characters.Length; i++)
        {
            if (playerOne.characters[i] != null)
                playerOne.characters[i].SetActive(false);
            if (playerTwo.characters[i] != null)
                playerTwo.characters[i].SetActive(false);
        }
    }
}
