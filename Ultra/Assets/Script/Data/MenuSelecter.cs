using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelecter
{
    public bool charakterSelected = false;
    public int slotIndex = 1;
    public GameObject[] characters;                                 // Needs to get Setted
    public Vector3 characterPosition;                               // Needs to get Setted
    public Characters characterEnum = Characters.None;
    public bool isSwitchingUp = false;
    public bool isSwitchingDown = false;

    [ColorUsageAttribute(true, true)] public Color[] colors;
    int colorIndex = 0;
    public Renderer rend;

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

        characterEnum = Characters.Nav;
        //characterEnum = (Characters)slotIndex + 1;                  // Index Need to count 1 UP because the Characters in the enum Starting at 1

        // Do Stuff with p1Characters[p1SlotIndex] | Maybe Animaton Or Something 

        //TEST

        characters[1].transform.position = new Vector3(characters[1].transform.position.x, 1, characters[1].transform.position.z);
    }

    public void UnselectSlot()
    {

        if (charakterSelected)
        {
            charakterSelected = false;

            characters[1].transform.position = characterPosition;
        }
        else if (!charakterSelected)
        {
            // TODO: Leave Lobby  OR Some Stuff
        }
    }
    
    public void ChangeColorUp()
    {
        if (!isSwitchingUp && !charakterSelected)
        {
            isSwitchingUp = true;
            if (colorIndex + 1 == colors.Length)
            {
                colorIndex = -1;
            }
            colorIndex++;
            ApplyColor(colors[colorIndex]);
            if (SwitchUpAction != null)
                SwitchUpAction();
        }
    }

    public void ChangeColorDown()
    {
        if (!isSwitchingUp && !charakterSelected)
        {
            isSwitchingUp = true;
            if (colorIndex == 0)
            {
                colorIndex = colors.Length;
            }
            colorIndex--;
            ApplyColor(colors[colorIndex]);
            if (SwitchUpAction != null)
                SwitchUpAction();
        }
    }

    public void ApplyColor(Color color)
    {
        rend.material.SetColor("_EmissionColor", color);
    }

    /// <summary>
    /// Player Switch Character Selection UP
    /// </summary>
    public void SwitchSlotUp()
    {
        if(!isSwitchingUp && !charakterSelected)
        {
            isSwitchingUp = true;
            characters[slotIndex].SetActive(false);
            if (slotIndex + 1 == characters.Length)                     // + 1 needed because Array.Lenght doesnt Start at 0
            {
                slotIndex = -1;
            }
            slotIndex++;
            characters[slotIndex].SetActive(true);
            if(SwitchUpAction != null)
                SwitchUpAction();
        }
    }
    /// <summary>
    /// Player Switch Character Selection DOWN
    /// </summary>
    public void SwitchSlotDown()
    {
        if (!isSwitchingDown && !charakterSelected)
        {
            isSwitchingDown = true;
            characters[slotIndex].SetActive(false);
            if (slotIndex == 0)
            {
                slotIndex = characters.Length;                          // No + 1 needed becaus Array.Length starts at 1
            }
            slotIndex--;
            characters[slotIndex].SetActive(true);
            if (SwitchDownAction != null)
                SwitchDownAction();
        }
    }
}
