using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelecter : MonoBehaviour
{
    private MenuManager menuManager;

    [Header("AmountOfPlayable Characters")]
    public int amountOfPlayableCharacters;

#region Player One
    [Header("Player1Slot")]
    public GameObject[] p1Characters;
    public Characters p1CharacterEnum = Characters.None;

    private bool p1charakterSelected = false;
    private int p1SlotIndex = 0;
    private Vector3 p1CharacterPosition;
   

#endregion

#region Player Two
    [Header("Player2Slot")]
    public GameObject[] p2Characters;
    public Characters p2Character;

    private bool p2charakterSelected = false;
    private int p2SlotIndex = 0;

#endregion

#region Subscibe & Unsubscribe from Delegates
    /// <summary>
    /// Subscribe to Delegate
    /// </summary>
    void OnEnable()
    {
        InputManager.P1_AButtonDownAction += P1SelectSlot;
        InputManager.P2_AButtonDownAction += P2SelectSlot;
    }

    /// <summary>
    /// UnSubscribe from Delegate
    /// </summary>
    void OnDisable()
    {
        InputManager.P1_AButtonDownAction -= P1SelectSlot;
        InputManager.P2_AButtonDownAction -= P2SelectSlot;
    }
    #endregion

    void P1SelectSlot()
    {
        if (p1charakterSelected)
            return;

        p1charakterSelected = true;

        p1CharacterEnum = (Characters)p1SlotIndex + 1;                  // Index Need to count 1 UP because the Characters in the enum Starting at 1


        // Do Stuff with p1Characters[p1SlotIndex] | Maybe Animaton Or Something 

        //TEST

        p1Characters[p1SlotIndex].transform.position = new Vector3(p1Characters[p1SlotIndex].transform.position.x, 1, p1Characters[p1SlotIndex].transform.position.z);
    }

    void P1UnselectSlot()
    {
        if (menuManager.gameStarting == true)
            return;

        if (!p1charakterSelected)
        {
            p1charakterSelected = false;

            p1Characters[p1SlotIndex].transform.position = new Vector3(p1CharacterPosition.x, p1CharacterPosition.y, p1CharacterPosition.z);
        }
        else if (p1charakterSelected)
        {
            // TODO: Leave Lobby  OR Some Stuff
        }
    }

    void P2SelectSlot()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            P1SelectSlot();
        }
    }

#region Player 1 Switch Character
    /// <summary>
    /// Player 1 Switch Character Selection UP
    /// </summary>
    void P1SwitchSlotUp()
    {
        p1Characters[p1SlotIndex].SetActive(false);
        if (p1SlotIndex + 1 == p1Characters.Length)                     // Math needed because Array.Lenght doesnt Start at 0
        {
            p1SlotIndex = -1;
        }
        p1SlotIndex++;
        p1Characters[p1SlotIndex].SetActive(true);
    }

    /// <summary>
    /// Player 1 Switch Character Selection DOWN
    /// </summary>
    void P1SwitchSlotDown()
    {
        p1Characters[p1SlotIndex].SetActive(false);
        if (p1SlotIndex == 0)
        {
            p1SlotIndex = p1Characters.Length;                          // No Math needed becaus Array.Length start at 1
        }
        p1SlotIndex--;
        p1Characters[p1SlotIndex].SetActive(true);
    }

    #endregion

#region Player 2 Switch Character
    /// <summary>
    /// Player 2 Switch Character Selection UP
    /// </summary>
    void P2SwitchSlotUp()
    {
        p2Characters[p2SlotIndex].SetActive(false);
        if (p2SlotIndex + 1 == p2Characters.Length)                     // Math needed because Array.Lenght doesnt Start at 0
        {
            p2SlotIndex = -1;
        }
        p2SlotIndex++;
        p2Characters[p2SlotIndex].SetActive(true);
    }

    /// <summary>
    /// Player 2 Switch Character Selection DOWN
    /// </summary>
    void P2SwitchSlotDown()
    {
        p1Characters[p2SlotIndex].SetActive(false);
        if (p2SlotIndex == 0)
        {
            p2SlotIndex = p2Characters.Length;                          // No Math needed becaus Array.Length start at 1
        }
        p2SlotIndex--;
        p2Characters[p2SlotIndex].SetActive(true);
    }

#endregion

    void Awake()
    {
#region Check befor Playing
        if(p1Characters.Length < 2 || p2Characters.Length < 2)
        {
            Debug.Log("<color=red>Not Enough Characters in CharacterSelecter</color> ");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        for(int i = 0; i < amountOfPlayableCharacters; i++)
        {
            if(p1Characters[i] == null || p2Characters[i] == null)
            {
                Debug.Log("<color=red>Not Enough Characters in CharacterSelecter Array</color> ");
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
        #endregion

#region Getter Stuff
        GameObject menuManagerObj = GameObject.Find("MenuManager");
        menuManager = menuManagerObj.GetComponent<MenuManager>();
       
        p1CharacterPosition = p1Characters[p1SlotIndex].transform.position;

        #endregion

#region Setter Stuff

#endregion

        p1Characters[1].SetActive(false);
        p2Characters[1].SetActive(false);
    }
}
