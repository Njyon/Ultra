using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelecter
{
    public GameObject[] characters = new GameObject[2];                                 // Needs to get Setted
    public Characters characterEnum = Characters.None;

    public bool charakterSelected = false;
    public int slotIndex = 0;
    public Vector3 characterPosition;                              // Needs to get Setted


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

    public void P1UnselectSlot()
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
    public void P1SwitchSlotUp()
    {
        characters[slotIndex].SetActive(false);
        if (slotIndex + 1 == characters.Length)                     // Math needed because Array.Lenght doesnt Start at 0
        {
            slotIndex = -1;
        }
        slotIndex++;
        characters[slotIndex].SetActive(true);
    }

    /// <summary>
    /// Player Switch Character Selection DOWN
    /// </summary>
    public void P1SwitchSlotDown()
    {
        characters[slotIndex].SetActive(false);
        if (slotIndex == 0)
        {
            slotIndex = characters.Length;                          // No Math needed becaus Array.Length start at 1
        }
        slotIndex--;
        characters[slotIndex].SetActive(true);
    }

    public void SetArray()
    {
        Debug.Log("FUCK");
    }
}
