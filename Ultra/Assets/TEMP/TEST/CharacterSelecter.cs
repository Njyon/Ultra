using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelecter : MonoBehaviour
{
    short playerOneID = 1;
    short playerTwoID = 2;

    Slot slotOne;
    Slot slotTwo;
    
    /// <summary>
    /// Subscribe to Delegate
    /// </summary>
    void OnEnable()
    {
        InputManager.P1_AButtonDownAction += SelectSlot;
        InputManager.P2_AButtonDownAction += SelectSlot;
    }

    /// <summary>
    /// UnSubscribe from Delegate
    /// </summary>
    void OnDisable()
    {
        InputManager.P1_AButtonDownAction -= SelectSlot;
        InputManager.P2_AButtonDownAction -= SelectSlot;
    }

    void SelectSlot()
    {
        if (slotOne.GetSlotState() == SlotState.free)
        {
            slotOne.SetSlotState(SlotState.taken);
        }
    }
}
