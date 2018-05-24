﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelecterV2 : MonoBehaviour
{
    private bool gameStarting = false;
    private PlayerInfoManager playerInfoManager;

    [Header("AmountOfPlayable Characters")]
    public int amountOfPlayableCharacters;

    [Header("Keeram")]
    public GameObject p1Keeram;
    public GameObject p2Keeram;

    [Header("Nav")]
    public GameObject p1Nav;
    public GameObject p2Nav;

    [Header("PlayerInfoManager")]
    [SerializeField] private GameObject playerInfoManagerObj;

    //Player Slots
    MenuSelecter playerOne = new MenuSelecter();
    MenuSelecter playerTwo = new MenuSelecter();
    
    #region Subscibe & Unsubscribe from Delegates
    /// <summary>
    /// Subscribe to Delegate
    /// </summary>
    void OnEnable()
    {
        InputManager.p1_OnKeyPressed += P1_InputDownCheck;
        InputManager.p2_OnKeyPressed += P2_InputDownCheck;

        InputManager.p1_OnKeyPressed += P1_InputUpCheck;
        InputManager.p2_OnKeyPressed += P2_InputUpCheck;

        InputManager.P1_LeftStickRightAction += playerOne.SwitchSlotUp;
        InputManager.P2_LeftStickRightAction += playerTwo.SwitchSlotUp;

        InputManager.P1_LeftStickLeftAction += playerOne.SwitchSlotDown;
        InputManager.P2_LeftStickLeftAction += playerTwo.SwitchSlotDown;

        //Fix because The Input is an Update and gets called Every Frame
        playerOne.SwitchUpAction += P1SwitchUp;
        playerOne.SwitchDownAction += P1SwitchDown;
        playerTwo.SwitchUpAction += P2SwitchUp;
        playerTwo.SwitchDownAction += P2SwitchDown;
    }

    /// <summary>
    /// UnSubscribe from Delegate
    /// </summary>
    void RemoveInput()
    {
        InputManager.p1_OnKeyPressed -= P1_InputDownCheck;
        InputManager.p2_OnKeyPressed -= P2_InputDownCheck;
        
        InputManager.p1_OnKeyReleased -= P1_InputUpCheck;
        InputManager.p2_OnKeyReleased -= P2_InputUpCheck;

        InputManager.P1_LeftStickRightAction -= playerOne.SwitchSlotUp;
        InputManager.P2_LeftStickRightAction -= playerTwo.SwitchSlotUp;

        InputManager.P1_LeftStickLeftAction -= playerOne.SwitchSlotDown;
        InputManager.P2_LeftStickLeftAction -= playerTwo.SwitchSlotDown;

        playerOne.SwitchUpAction -= P1SwitchUp;
        playerOne.SwitchDownAction -= P1SwitchDown;
        playerTwo.SwitchUpAction -= P2SwitchUp;
        playerTwo.SwitchDownAction -= P2SwitchDown;
    }
    #endregion

    #region Check Input
    void P1_InputDownCheck(KeyCode keyCode)
    {
        if(keyCode == KeyCode.Joystick1Button0)                                     // P1: Press A
        {
            playerOne.SelectSlot();
        }
        else if(keyCode == KeyCode.Joystick1Button1)                                // P1: Press B
        {
            playerOne.UnselectSlot();
        }
    }
    void P1_InputUpCheck(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick1Button0)                                    // P1: Release A
        {
            Check();        //Check if Game can Start
        }
    }

    void P2_InputDownCheck(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick2Button0)                                    // P2: Press A
        {
            playerTwo.SelectSlot();
        }
        else if (keyCode == KeyCode.Joystick2Button1)                               // P2: Press B
        {
            playerTwo.UnselectSlot();
        }
    }
    void P2_InputUpCheck(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick2Button0)                                    // P2: Release A
        {
            Check();        //Check if Game can Start
        }
    }
#endregion
    
    void Check()
    {
        if (playerOne.charakterSelected && playerTwo.charakterSelected && !gameStarting)
        {
            // TODO: TIMER!

            gameStarting = true;

            playerInfoManager.playerOne.character = playerOne.characterEnum;
            playerInfoManager.playerTwo.character = playerTwo.characterEnum;

            RemoveInput();
            StartGame();
            
        }
    }

    void Awake()
    {
        if(playerInfoManagerObj == null)
        {
            Debug.Log("Missing PlayerInfoManager Prefab " + gameObject.name);
        }
        else
        {
            if(GameObject.Find("PlayerInfoManager(Clone)") == null)
            {
                Instantiate(playerInfoManagerObj, Vector3.zero, Quaternion.identity);
            }
        }
    #region Set befor Play
        playerOne.characters = new GameObject[amountOfPlayableCharacters];
        playerTwo.characters = new GameObject[amountOfPlayableCharacters];

        playerOne.characterPosition = p1Keeram.transform.position;
        playerTwo.characterPosition = p2Keeram.transform.position;

#endregion

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
        }
        for (int i = 0; i < amountOfPlayableCharacters; i++)
        {
            if (playerOne.characters[i] == null || playerTwo.characters[i] == null)
            {
                Debug.Log("<color=red>Not Enough Characters in CharacterSelecter Array</color> ");
            }
        }
#endregion
        
    #region Getter Stuff
        playerInfoManager = playerInfoManagerObj.GetComponent<PlayerInfoManager>();

        playerOne.characterPosition = playerOne.characters[playerOne.slotIndex].transform.position;
        playerTwo.characterPosition = playerTwo.characters[playerTwo.slotIndex].transform.position;

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
    #region Load Next Scene
    void StartGame()
    {
        StartCoroutine(LoadNewScene());
    }
    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(0.1f);
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        while (!async.isDone)
        {
            yield return null;
        }
    }
    #endregion

#region Input Fix
    void P1SwitchUp()
    {
        StartCoroutine(P1Up());
    }
    void P1SwitchDown()
    {
        StartCoroutine(P1Down());
    }
    void P2SwitchUp()
    {
        StartCoroutine(P2Up());
    }
    void P2SwitchDown()
    {
        StartCoroutine(P2Down());
    }


    IEnumerator P1Up()
    {
        yield return new WaitForSeconds(1.0f);
        playerOne.isSwitchingUp = false;
    }
    IEnumerator P1Down()
    {
        yield return new WaitForSeconds(1.0f);
        playerOne.isSwitchingDown = false;
    }
    IEnumerator P2Up()
    {
        yield return new WaitForSeconds(1.0f);
        playerTwo.isSwitchingUp = false;
    }
    IEnumerator P2Down()
    {
        yield return new WaitForSeconds(1.0f);
        playerTwo.isSwitchingDown = false;
    }
#endregion
}
