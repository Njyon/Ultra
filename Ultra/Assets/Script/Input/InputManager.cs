using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    #region Player 1 Input
        #region A Button
            public delegate void P1_AButtonDown();
            public static P1_AButtonDown P1_AButtonDownAction;
            public delegate void P1_AButtonUp();
            public static P1_AButtonUp P1_AButtonUpAction;
        #endregion
        #region B Button
            public delegate void P1_BButtonDown();
            public static P1_BButtonDown P1_BButtonDownAction;
            public delegate void P1_BButtonUp();
            public static P1_BButtonUp P1_BButtonUpAction;
        #endregion
        #region X Button
            public delegate void P1_XButtonDown();
            public static P1_XButtonDown P1_XButtonDownAction;
            public delegate void P1_XButtonUp();
            public static P1_XButtonUp P1_XButtonUpAction;
        #endregion
        #region Y Button
            public delegate void P1_YButtonDown();
            public static P1_YButtonDown P1_YButtonDownAction;
            public delegate void P1_YButtonUp();
            public static P1_YButtonUp P1_YButtonUpAction;
        #endregion
        #region Horizontal Axis
            public delegate void P1_LeftStickRight();
            public static P1_LeftStickRight P1_LeftStickRightAction;
            public delegate void P1_LeftStickLeft();
            public static P1_LeftStickLeft P1_LeftStickLeftAction;
    #endregion
        #region Vertical Axis
            public delegate void P1_LeftStickUp();
            public static P1_LeftStickUp P1_LeftStickUpAction;
            public delegate void P1_LeftStickDown();
            public static P1_LeftStickDown P1_LeftStickDownAction;
        #endregion
        #region TriggerRight
            public delegate void P1_RightTiggerDown();
            public static P1_RightTiggerDown P1_RightTiggerDownAction;
            public delegate void P1_RightTiggerUp();
            public static P1_RightTiggerUp P1_RightTiggerUpAction;
            /// <summary>
            /// Needed bool for event System
            /// </summary>
            bool P1_TriggertStateRight = false;
        #endregion
        #region TriggerLeft
            public delegate void P1_LeftTriggerDown();
            public static P1_LeftTriggerDown P1_LeftTriggerDownAction;
            public delegate void P1_LeftTriggerUp();
            public static P1_LeftTriggerUp P1_LeftTriggerUpAction;
            /// <summary>
            /// Needed bool for event System
            /// </summary>
            bool P1_TriggerStateLeft = false;
        #endregion
        #region START / SELECT
            public delegate void P1_StartButton();
            public static P1_StartButton P1_StartButtonAction;
            public delegate void P1_SelectButton();
            public static P1_SelectButton P1_SelectButtonAction;
    #endregion
    #endregion

    #region Player 2 Input
        #region A Button
            public delegate void P2_AButtonDown();
            public static P2_AButtonDown P2_AButtonDownAction;
            public delegate void P2_AButtonUp();
            public static P2_AButtonUp P2_AButtonUpAction;
        #endregion
        #region B Button
            public delegate void P2_BButtonDown();
            public static P2_BButtonDown P2_BButtonDownAction;
            public delegate void P2_BButtonUp();
            public static P2_BButtonUp P2_BButtonUpAction;
        #endregion
        #region X Button
            public delegate void P2_XButtonDown();
            public static P1_XButtonDown P2_XButtonDownAction;
            public delegate void P2_XButtonUp();
            public static P2_XButtonUp P2_XButtonUpAction;
        #endregion
        #region Y Button
            public delegate void P2_YButtonDown();
            public static P2_YButtonDown P2_YButtonDownAction;
            public delegate void P2_YButtonUp();
            public static P2_YButtonUp P2_YButtonUpAction;
        #endregion
        #region Horizontal Axis
            public delegate void P2_LeftStickRight();
            public static P2_LeftStickRight P2_LeftStickRightAction;
            public delegate void P2_LeftStickLeft();
            public static P2_LeftStickLeft P2_LeftStickLeftAction;
    #endregion
        #region Vertical Axis
            public delegate void P2_LeftStickUp();
            public static P2_LeftStickUp P2_LeftStickUpAction;
            public delegate void P2_LeftStickDown();
            public static P2_LeftStickDown P2_LeftStickDownAction;
        #endregion
        #region TriggerRight
            public delegate void P2_RightTiggerDown();
            public static P2_RightTiggerDown P2_RightTiggerDownAction;
            public delegate void P2_RightTiggerUp();
            public static P2_RightTiggerUp P2_RightTiggerUpAction;
            /// <summary>
            /// Needed bool for event System
            /// </summary>
            bool P2_TriggertStateRight = false;
        #endregion
        #region TriggerLeft
            public delegate void P2_LeftTriggerDown();
            public static P2_LeftTriggerDown P2_LeftTriggerDownAction;
            public delegate void P2_LeftTriggerUp();
            public static P2_LeftTriggerUp P2_LeftTriggerUpAction;
            /// <summary>
            /// Needed bool for event System
            /// </summary>
            bool P2_TriggerStateLeft = false;
        #endregion
        #region START / BACK
            public delegate void P2_StartButton();
            public static P2_StartButton P2_StartButtonAction;
            public delegate void P2_SelectButton();
            public static P2_SelectButton P2_SelectButtonAction;
    #endregion
    #endregion

    // Check for Input
    void Update ()
    {
        //Player 1
        #region A Button
        if (Input.GetButtonDown("P1_AButton"))
        {
            Debug.Log("P1 The 'A' Button Down");
            if (P1_AButtonDownAction != null)
                P1_AButtonDownAction();
        }
        else if(Input.GetButtonUp("P1_AButton"))
        {
            Debug.Log("P1 The 'A' Button Up");
            if (P1_AButtonUpAction != null)
                P1_AButtonUpAction();
        }
        #endregion
        #region B Button
        if (Input.GetButtonDown("P1_BButton"))
        {
            Debug.Log("P1 The 'B' Button Down");
            if (P1_BButtonDownAction != null)
                P1_BButtonDownAction();
        }
        else if (Input.GetButtonUp("P1_BButton"))
        {
            Debug.Log("P1 The 'B' Button Up");
            if (P1_BButtonUpAction != null)
                P1_BButtonUpAction();
        }
        #endregion
        #region X Button
        if (Input.GetButtonDown("P1_XButton"))
        {
            Debug.Log("P1 The 'X' Button Down");
            if (P1_XButtonDownAction != null)
                P1_XButtonDownAction();
        }
        else if (Input.GetButtonUp("P1_XButton"))
        {
            Debug.Log("P1 The 'X' Button Up");
            if (P1_XButtonUpAction != null)
                P1_XButtonUpAction();
        }
        #endregion
        #region Y Button
        if (Input.GetButtonDown("P1_YButton"))
        {
            Debug.Log("P1 The 'Y' Button Down");
            if (P1_YButtonDownAction != null)
                P1_YButtonDownAction();
        }
        else if (Input.GetButtonUp("P1_YButton"))
        {
            Debug.Log("P1 The 'Y' Button Up");
            if (P1_YButtonUpAction != null)
                P1_YButtonUpAction();
        }
        #endregion
        #region Horizontal Axis
        if (Input.GetAxisRaw("P1_Horizontal") == 1)
        {
            Debug.Log("P1_Horizontal right");
            if (P1_LeftStickRightAction != null)
                P1_LeftStickRightAction();
        }
        else if(Input.GetAxisRaw("P1_Horizontal") == -1)
        {
            Debug.Log("P1_Horizontal left");
            if (P1_LeftStickLeftAction != null)
                P1_LeftStickLeftAction();
        }
        #endregion
        #region Vertical Axis
        if (Input.GetAxisRaw("P1_Vertical") == 1)
        {
            Debug.Log("P1_Horizontal Down");
            if (P1_LeftStickDownAction != null)
                P1_LeftStickDownAction();
        }
        else if (Input.GetAxisRaw("P1_Vertical") == -1)
        {
            Debug.Log("P1_Horizontal Up");
            if (P1_LeftStickUpAction != null)
                P1_LeftStickUpAction();
        }
        #endregion
        #region TriggerLeft
        if (Input.GetAxisRaw("P1_LEFTTrigger") == 1 && !P1_TriggerStateLeft)
        {
            P1_TriggerStateLeft = true;
            Debug.Log("P1 The 'LeftTrigger' Button Down");
            if (P1_LeftTriggerDownAction != null)
                P1_LeftTriggerDownAction();
        }
        else if(Input.GetAxisRaw("P1_LEFTTrigger") == 0 && P1_TriggerStateLeft)
        {
            P1_TriggerStateLeft = false;
            Debug.Log("P1 The 'LeftTrigger' Button Up");
            if (P1_LeftTriggerUpAction != null)
                P1_LeftTriggerUpAction();
        }
        #endregion
        #region TriggerRight
        if (Input.GetAxisRaw("P1_RIGHTTrigger") == -1 && !P1_TriggertStateRight)
        {
            P1_TriggertStateRight = true;
            Debug.Log("P1 The 'RightTrigger' Button Down");
            if (P1_RightTiggerDownAction != null)
                P1_RightTiggerDownAction();
        }
        else if (Input.GetAxisRaw("P1_RIGHTTrigger") == 0 && P1_TriggertStateRight)
        {
            P1_TriggertStateRight = false;
            Debug.Log("P1 The 'RightTrigger' Button Up");
            if (P1_RightTiggerUpAction != null)
                P1_RightTiggerUpAction();
        }
        #endregion
        #region START / BACK
        if(Input.GetButtonDown("P1_START"))
        {
            Debug.Log("P1 START Pressed");
            if (P1_StartButtonAction != null)
                P1_StartButtonAction();
        }
        if(Input.GetButtonDown("P1_BACK"))
        {
            Debug.Log("P1 SELECT Pressed");
            if (P1_SelectButtonAction != null)
                P1_SelectButtonAction();
        }
        #endregion

        //Player 2
        #region A Button
        if (Input.GetButtonDown("P2_AButton"))
        {
            Debug.Log("P2 The 'A' Button Down");
            if (P2_AButtonDownAction != null)
                P2_AButtonDownAction();
        }
        else if (Input.GetButtonUp("P2_AButton"))
        {
            Debug.Log("P2 The 'A' Button Up");
            if (P2_AButtonUpAction != null)
                P2_AButtonUpAction();
        }
        #endregion
        #region B Button
        if (Input.GetButtonDown("P2_BButton"))
        {
            Debug.Log("P2 The 'B' Button Down");
            if (P2_BButtonDownAction != null)
                P2_BButtonDownAction();
        }
        else if (Input.GetButtonUp("P2_BButton"))
        {
            Debug.Log("P2 The 'B' Button Up");
            if (P2_BButtonUpAction != null)
                P2_BButtonUpAction();
        }
        #endregion
        #region X Button
        if (Input.GetButtonDown("P2_XButton"))
        {
            Debug.Log("P2 The 'X' Button Down");
            if (P2_XButtonDownAction != null)
                P2_XButtonDownAction();
        }
        else if (Input.GetButtonUp("P2_XButton"))
        {
            Debug.Log("P2 The 'X' Button Up");
            if (P2_XButtonUpAction != null)
                P2_XButtonUpAction();
        }
        #endregion
        #region Y Button
        if (Input.GetButtonDown("P2_YButton"))
        {
            Debug.Log("P2 The 'Y' Button Down");
            if (P2_YButtonDownAction != null)
                P2_YButtonDownAction();
        }
        else if (Input.GetButtonUp("P2_YButton"))
        {
            Debug.Log("P2 The 'Y' Button Up");
            if (P2_YButtonUpAction != null)
                P2_YButtonUpAction();
        }
        #endregion
        #region Horizontal Axis
        if (Input.GetAxisRaw("P2_Horizontal") == 1)
        {
            Debug.Log("P2_Horizontal right");
            if (P2_LeftStickRightAction != null)
                P2_LeftStickRightAction();
        }
        else if (Input.GetAxisRaw("P2_Horizontal") == -1)
        {
            Debug.Log("P2_Horizontal left");
            if (P2_LeftStickLeftAction != null)
                P2_LeftStickLeftAction();
        }
        #endregion
        #region Vertical Axis
        if (Input.GetAxisRaw("P2_Vertical") == 1)
        {
            Debug.Log("P2_Horizontal Down");
            if (P2_LeftStickDownAction != null)
                P2_LeftStickDownAction();
        }
        else if (Input.GetAxisRaw("P2_Vertical") == -1)
        {
            Debug.Log("P2_Horizontal Up");
            if (P2_LeftStickUpAction != null)
                P2_LeftStickUpAction();
        }
        #endregion
        #region TriggerLeft
        if (Input.GetAxisRaw("P2_LEFTTrigger") == 1 && !P2_TriggerStateLeft)
        {
            P2_TriggerStateLeft = true;
            Debug.Log("P2 The 'LeftTrigger' Button Down");
            if (P2_LeftTriggerDownAction != null)
                P2_LeftTriggerDownAction();
        }
        else if (Input.GetAxisRaw("P2_LEFTTrigger") == 0 && P2_TriggerStateLeft)
        {
            P2_TriggerStateLeft = false;
            Debug.Log("P2 The 'LeftTrigger' Button Up");
            if (P2_LeftTriggerUpAction != null)
                P2_LeftTriggerUpAction();
        }
        #endregion
        #region TriggerRight
        if (Input.GetAxisRaw("P2_RIGHTTrigger") == -1 && !P2_TriggertStateRight)
        {
            P2_TriggertStateRight = true;
            Debug.Log("P2 The 'RightTrigger' Button Down");
            if (P2_RightTiggerDownAction != null)
                P2_RightTiggerDownAction();
        }
        else if (Input.GetAxisRaw("P2_RIGHTTrigger") == 0 && P2_TriggertStateRight)
        {
            P2_TriggertStateRight = false;
            Debug.Log("P2 The 'RightTrigger' Button Up");
            if (P2_RightTiggerUpAction != null)
                P2_RightTiggerUpAction();
        }
        #endregion
        #region START / BACK
        if (Input.GetButtonDown("P2_START"))
        {
            Debug.Log("P2 START Pressed");
            if (P2_StartButtonAction != null)
                P2_StartButtonAction();
        }
        if (Input.GetButtonDown("P2_BACK"))
        {
            Debug.Log("P2 SELECT Pressed");
            if (P2_SelectButtonAction != null)
                P2_SelectButtonAction();
        }
        #endregion
    }
}
