using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DELEGATE : MonoBehaviour
{
    public delegate void ClickAction();
    public static ClickAction OnClickAction;

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 50, 5, 100, 30), "Click"))
            if (OnClickAction != null)
                OnClickAction();
    }
}
