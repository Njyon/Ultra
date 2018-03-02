using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelecter
{
    public bool charakterSelected = false;
    public int slotIndex = 0;
    public GameObject[] characters;                                 // Needs to get Setted
    public Vector3 characterPosition;                               // Needs to get Setted
    public Characters characterEnum = Characters.None;
    public bool isSwitchingUp = false;
    public bool isSwitchingDown = false;
    #region Delegates
    public delegate void SwitchUp();
    public SwitchUp SwitchUpAction;
    public delegate void SwitchDown();
    public SwitchDown SwitchDownAction;
    #endregion
    
    public void SelectSlot()
    {
        if (charakterSelected)
            return;

        charakterSelected = true;

        characterEnum = (Characters)slotIndex + 1;                  // Index Need to count 1 UP because the Characters in the enum Starting at 1
        
        // Do Stuff with p1Characters[p1SlotIndex] | Maybe Animaton Or Something 

        //TEST

        characters[slotIndex].transform.position = new Vector3(characters[slotIndex].transform.position.x, 1, characters[slotIndex].transform.position.z);
    }

    public void UnselectSlot()
    {

        if (!charakterSelected)
        {
            charakterSelected = false;

            characters[slotIndex].transform.position = new Vector3(characterPosition.x, characterPosition.y, characterPosition.z);
        }
        else if (charakterSelected)
        {
            // TODO: Leave Lobby  OR Some Stuff
        }
    }

    /// <summary>
    /// Player Switch Character Selection UP
    /// </summary>
    public void SwitchSlotUp()
    {
        if(!isSwitchingUp)
        {
            Debug.Log("First: " + characters[0].name + " Second: " + characters[1].name);

            isSwitchingUp = true;
            characters[slotIndex].SetActive(false);
            if (slotIndex + 1 == characters.Length)                     // Math needed because Array.Lenght doesnt Start at 0
            {
                slotIndex = -1;
            }
            slotIndex++;
            characters[slotIndex].SetActive(true);
            SwitchUpAction();
        }
    }

    /// <summary>
    /// Player Switch Character Selection DOWN
    /// </summary>
    public void SwitchSlotDown()
    {
        if (!isSwitchingDown)
        {
            isSwitchingDown = true;
            characters[slotIndex].SetActive(false);
            if (slotIndex == 0)
            {
                slotIndex = characters.Length;                          // No Math needed becaus Array.Length starts at 1
            }
            slotIndex--;
            characters[slotIndex].SetActive(true);
            SwitchDownAction();
        }
    }
}
