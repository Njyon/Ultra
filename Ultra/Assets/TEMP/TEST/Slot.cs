using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlotState
{
    free,
    taken
};
public class Slot : MonoBehaviour
{


    SlotState state = SlotState.free;

    short playerID;
    GameObject Character;

    #region State Get/Set
    public SlotState GetSlotState()
    {
        return this.state;
    }

    public void SetSlotState(SlotState newState)
    {
        this.state = newState;
    }
    #endregion
}
