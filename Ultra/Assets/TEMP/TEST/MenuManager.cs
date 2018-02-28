using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public bool gameStarting = false;

    private bool p1ReadyForGame = false;
    private bool p2ReadyForGame = false;

    public void Check (bool p1State, bool p2State)
    {
        p1ReadyForGame = p1State;
        p2ReadyForGame = p2State;
    }

    void StartGame()
    {

    }
}
