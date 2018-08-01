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

    [Header("PlayerInfoManager")]
    [SerializeField] private GameObject playerInfoManagerObj;

    //Player Slots
    MenuSelecter playerOne = new MenuSelecter();
    MenuSelecter playerTwo = new MenuSelecter();

    [Header("Colors")]
    [ColorUsageAttribute(true, true)] public Color[] colors;

    [Header("Renderer Cloth")]
    [SerializeField] Renderer renderer1;
    [SerializeField] Renderer renderer2;

    public delegate void P1_IsReady(bool isReady);
    public delegate void P2_IsReady(bool isReady);

    public P1_IsReady p1_Ready;
    public P2_IsReady p2_Ready;

    #region Subscibe & Unsubscribe from Delegates
    /// <summary>
    /// Subscribe to Delegate
    /// </summary>
    public void ApplyInput()
    {
        InputManager.p1_OnKeyPressed += P1_InputDownCheck;
        InputManager.p2_OnKeyPressed += P2_InputDownCheck;

        InputManager.p1_OnKeyPressed += P1_InputUpCheck;
        InputManager.p2_OnKeyPressed += P2_InputUpCheck;

        InputManager.P1_LeftStickRightAction += playerOne.ChangeColorUp;
        InputManager.P2_LeftStickRightAction += playerTwo.ChangeColorUp;

        InputManager.P1_LeftStickLeftAction += playerOne.ChangeColorDown;
        InputManager.P2_LeftStickLeftAction += playerTwo.ChangeColorDown;

        //Fix because The Input is an Update and gets called Every Frame
        playerOne.SwitchUpAction += P1SwitchUp;
        playerOne.SwitchDownAction += P1SwitchDown;
        playerTwo.SwitchUpAction += P2SwitchUp;
        playerTwo.SwitchDownAction += P2SwitchDown;
    }

    /// <summary>
    /// UnSubscribe from Delegate
    /// </summary>
    public void RemoveInput()
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
        if (keyCode == KeyCode.Joystick1Button0 || keyCode == KeyCode.Joystick1Button1)                                    // P1: Release A
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
        if (keyCode == KeyCode.Joystick2Button0 || keyCode == KeyCode.Joystick2Button1)                                    // P2: Release A
        {
            Check();        //Check if Game can Start
        }
    }
#endregion
    
    void Check()
    {
        if(playerOne.charakterSelected)
        {
            p1_Ready(true);
        }
        else
        {
            p1_Ready(false);
        }

        if(playerTwo.charakterSelected)
        {
            p2_Ready(true);
        }
        else
        {
            p2_Ready(false);
        }

        if (playerOne.charakterSelected && playerTwo.charakterSelected && !gameStarting)
        {
            // TODO: TIMER!

            gameStarting = true;

            PlayerInfoManager.playerOne.character = playerOne.characterEnum;
            PlayerInfoManager.playerTwo.character = playerTwo.characterEnum;

            PlayerInfoManager.playerOne.color = renderer1.material.GetColor("_EmissionColor");
            PlayerInfoManager.playerTwo.color = renderer2.material.GetColor("_EmissionColor");

            RemoveInput();
            StartGame();
            
        }
    }
    public bool NoCharSelected()
    {
        if(!playerOne.charakterSelected && !playerTwo.charakterSelected && !gameStarting)
        {
            return true;
        }
        else
        {
            return false;
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

        playerOne.characters = new GameObject[amountOfPlayableCharacters];
        playerTwo.characters = new GameObject[amountOfPlayableCharacters];

        playerOne.characterPosition = p1Keeram.transform.position;
        playerTwo.characterPosition = p2Keeram.transform.position;
        
        
        playerOne.characters[0] = p1Keeram;
        playerOne.characters[1] = p1Nav;

        playerTwo.characters[0] = p2Keeram;
        playerTwo.characters[1] = p2Nav;
        
        
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
        
        playerInfoManager = playerInfoManagerObj.GetComponent<PlayerInfoManager>();

        playerOne.characterPosition = playerOne.characters[playerOne.slotIndex].transform.position;
        playerTwo.characterPosition = playerTwo.characters[playerTwo.slotIndex].transform.position;

        playerOne.rend = renderer1;
        playerTwo.rend = renderer2;

        playerOne.colors = colors;
        playerTwo.colors = colors;

        playerOne.ApplyColor(playerOne.colors[0]);
        playerTwo.ApplyColor(playerOne.colors[0]);

        // Set both Character invisible
        for (int i = 0; i < playerOne.characters.Length; i++)
        {
            if (playerOne.characters[i] != null)
                playerOne.characters[i].SetActive(false);
            if (playerTwo.characters[i] != null)
                playerTwo.characters[i].SetActive(false);
        }
    }

    /// <summary>
    /// Enables Nav
    /// </summary>
    public void ShowNav()
    {
        p1Nav.SetActive(true);
        p2Nav.SetActive(true);
    }
    /// <summary>
    /// Hiddes Nav
    /// </summary>
    public void HiddeNav()
    {
        p1Nav.SetActive(false);
        p2Nav.SetActive(false);
    }

    #region Load Next Scene
    void StartGame()
    {
        StartCoroutine(LoadNewScene());
    }
    IEnumerator LoadNewScene()
    {
        yield return new WaitForSeconds(0.1f);
        AsyncOperation async = SceneManager.LoadSceneAsync(2);
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
        yield return new WaitForSeconds(0.2f);
        playerOne.isSwitchingUp = false;
    }
    IEnumerator P1Down()
    {
        yield return new WaitForSeconds(0.2f);
        playerOne.isSwitchingDown = false;
    }
    IEnumerator P2Up()
    {
        yield return new WaitForSeconds(0.2f);
        playerTwo.isSwitchingUp = false;
    }
    IEnumerator P2Down()
    {
        yield return new WaitForSeconds(0.2f);
        playerTwo.isSwitchingDown = false;
    }
#endregion
}
