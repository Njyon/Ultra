using System.Collections;
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

    //Player Slots
    MenuSelecter playerOne = new MenuSelecter();
    MenuSelecter playerTwo = new MenuSelecter();

#region Subscibe & Unsubscribe from Delegates
    /// <summary>
    /// Subscribe to Delegate
    /// </summary>
    void OnEnable()
    {
        InputManager.P1_AButtonDownAction += playerOne.SelectSlot;
        InputManager.P2_AButtonDownAction += playerTwo.SelectSlot;

        InputManager.P1_AButtonDownAction += Check;
        InputManager.P2_AButtonDownAction += Check;

        InputManager.P1_AButtonDownAction += playerOne.UnselectSlot;
        InputManager.P2_AButtonDownAction += playerTwo.UnselectSlot;

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
    void OnDisable()
    {
        InputManager.P1_AButtonDownAction -= playerOne.SelectSlot;
        InputManager.P2_AButtonDownAction -= playerTwo.SelectSlot;

        InputManager.P1_AButtonDownAction -= playerOne.UnselectSlot;
        InputManager.P2_AButtonDownAction -= playerTwo.UnselectSlot;
    }
#endregion
    
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {

        }
    }

 

    void Check()
    {
        if (playerOne.charakterSelected && playerTwo.charakterSelected)
        {
            // TODO: TIMER!

            gameStarting = true;

            playerInfoManager.playerOne.character = playerOne.characterEnum;
            playerInfoManager.playerTwo.character = playerTwo.characterEnum;

            StartGame();
        }
    }

    void Awake()
    {

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
        GameObject PlayerInfoManagerObj = GameObject.Find("PlayerInfoManager");
        playerInfoManager = PlayerInfoManagerObj.GetComponent<PlayerInfoManager>();

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
#region TEST
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
