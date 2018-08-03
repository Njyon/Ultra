using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    // Button Inputs
    public delegate void P1_OnKeyPressedDelegate(KeyCode keyCode);
    public delegate void P1_OnKeyReleasedDelegate(KeyCode keyCode);
    public delegate void P2_OnKeyPressedDelegate(KeyCode keyCode);
    public delegate void P2_OnKeyReleasedDelegate(KeyCode keyCode);

    public static P1_OnKeyPressedDelegate p1_OnKeyPressed;
    public static P1_OnKeyReleasedDelegate p1_OnKeyReleased;
    public static P2_OnKeyPressedDelegate p2_OnKeyPressed;
    public static P2_OnKeyReleasedDelegate p2_OnKeyReleased;
    
    #region Player 1 Input
        #region Special
            public delegate void P1_SpecalLeft();
            public static P1_SpecalLeft P1_SpecalLeftAction;
            public delegate void P1_SpecalRight();
            public static P1_SpecalRight P1_SpecalRightAction;

            public delegate void P1_SpecalTop();
            public static P1_SpecalTop P1_SpecalTopAction;
            public delegate void P1_SpecalBottom();
            public static P1_SpecalBottom P1_SpecalBottomAction;
        #endregion
        #region Ligth Attack
            public delegate void P1_XButtonDirection(Direction direction);
            public static P1_XButtonDirection P1_XButtonDirectionAction;
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
        #region LeftStickZero
            public delegate void P1_LeftStickZero();
            public static P1_LeftStickZero P1_LeftStickZeroAction;

            bool p1_LeftStickZeroed = false;
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
    #endregion

    #region Player 2 Input
        #region Special
            public delegate void P2_SpecalLeft();
            public static P2_SpecalLeft P2_SpecalLeftAction;
            public delegate void P2_SpecalRight();
            public static P2_SpecalRight P2_SpecalRightAction;

            public delegate void P2_SpecalTop();
            public static P2_SpecalTop P2_SpecalTopAction;
            public delegate void P2_SpecalBottom();
            public static P2_SpecalBottom P2_SpecalBottomAction;
    #endregion
        #region Light Attack
            public delegate void P2_XButtonDirection(Direction direction);
            public static P2_XButtonDirection P2_XButtonDirectionAction;
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
        #region LeftStickZero
            public delegate void P2_LeftStickZero();
            public static P2_LeftStickZero P2_LeftStickZeroAction;

            bool p2_LeftStickZeroed = false;
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
    #endregion

    // Check for Input
    void Update()
    {
        //Player 1
        #region A Button
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (p1_OnKeyPressed != null)
                p1_OnKeyPressed(KeyCode.Joystick1Button0);
        }
        else if(Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            if (p1_OnKeyReleased != null)
                p1_OnKeyReleased(KeyCode.Joystick1Button0);
        }
        #endregion
        #region B Button
        //if (Input.GetKeyDown(KeyCode.Joystick1Button1) && Input.GetAxisRaw("P1_Horizontal") == 1)
        //{
        //    if (P1_SpecalRightAction != null)
        //        P1_SpecalRightAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick1Button1) && Input.GetAxisRaw("P1_Horizontal") == -1)
        //{
        //    if (P1_SpecalLeftAction != null)
        //        P1_SpecalLeftAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick1Button1) && Input.GetAxisRaw("P1_Vertical") == 1)
        //{
        //    if (P1_SpecalBottomAction != null)
        //        P1_SpecalBottomAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick1Button1) && Input.GetAxisRaw("P1_Vertical") == -1)
        //{
        //    if (P1_SpecalTopAction != null)
        //        P1_SpecalTopAction();
        //}
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (p1_OnKeyPressed != null)
                p1_OnKeyPressed(KeyCode.Joystick1Button1);
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button1))
        {
            if (p1_OnKeyReleased != null)
                p1_OnKeyReleased(KeyCode.Joystick1Button1);
        }
        #endregion
        #region X Button
        if(Input.GetKeyDown(KeyCode.Joystick1Button2) && Input.GetAxisRaw("P1_Horizontal") >= 0.6f && Input.GetAxisRaw("P1_Vertical") <= -0.6f)
        {
            if (P1_XButtonDirectionAction != null)
                P1_XButtonDirectionAction(Direction.RightUpAngel);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button2) && Input.GetAxisRaw("P1_Horizontal") >= 0.6f && Input.GetAxisRaw("P1_Vertical") >= 0.6f)
        {
            if (P1_XButtonDirectionAction != null)
                P1_XButtonDirectionAction(Direction.RightDownAngel);
        }
        else if(Input.GetKeyDown(KeyCode.Joystick1Button2) && Input.GetAxisRaw("P1_Horizontal") <= -0.6f && Input.GetAxisRaw("P1_Vertical") <= -0.6f)
        {
            if (P1_XButtonDirectionAction != null)
                P1_XButtonDirectionAction(Direction.LeftUpAngel);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button2) && Input.GetAxisRaw("P1_Horizontal") <= -0.6f && Input.GetAxisRaw("P1_Vertical") >= 0.6f)
        {
            if (P1_XButtonDirectionAction != null)
                P1_XButtonDirectionAction(Direction.LeftDownAngel);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button2) && Input.GetAxisRaw("P1_Horizontal") == 1)
        {
            if (P1_XButtonDirectionAction != null)
                P1_XButtonDirectionAction(Direction.Right);
        }
        else if(Input.GetKeyDown(KeyCode.Joystick1Button2) && Input.GetAxisRaw("P1_Horizontal") == -1)
        {
            if (P1_XButtonDirectionAction != null)
                P1_XButtonDirectionAction(Direction.Left);
        }
        else if(Input.GetKeyDown(KeyCode.Joystick1Button2) && Input.GetAxisRaw("P1_Vertical") == 1)
        {
            if (P1_XButtonDirectionAction != null)
                P1_XButtonDirectionAction(Direction.Down);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button2) && Input.GetAxisRaw("P1_Vertical") == -1)
        {
            if (P1_XButtonDirectionAction != null)
                P1_XButtonDirectionAction(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            if (p1_OnKeyPressed != null)
                p1_OnKeyPressed(KeyCode.Joystick1Button2);
            if (P1_XButtonDirectionAction != null)
                P1_XButtonDirectionAction(Direction.None);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick1Button2))
        {
            if (p1_OnKeyReleased != null)
                p1_OnKeyReleased(KeyCode.Joystick1Button2);
        }
        #endregion
        #region Y Button
        //if (Input.GetKeyDown(KeyCode.Joystick1Button3) && Input.GetAxisRaw("P1_Horizontal") == 1)
        //{
        //    if (P1_SpecalRightAction != null)
        //        P1_SpecalRightAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick1Button3) && Input.GetAxisRaw("P1_Horizontal") == -1)
        //{
        //    if (P1_SpecalLeftAction != null)
        //        P1_SpecalLeftAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick1Button3) && Input.GetAxisRaw("P1_Vertical") == 1)
        //{
        //    if (P1_SpecalBottomAction != null)
        //        P1_SpecalBottomAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick1Button3) && Input.GetAxisRaw("P1_Vertical") == -1)
        //{
        //    if (P1_SpecalTopAction != null)
        //        P1_SpecalTopAction();
        //}
        if (Input.GetKeyDown(KeyCode.Joystick1Button3))
        {
            if (p1_OnKeyPressed != null)
                p1_OnKeyPressed(KeyCode.Joystick1Button3);
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button3))
        {
            if (p1_OnKeyReleased != null)
                p1_OnKeyReleased(KeyCode.Joystick1Button3);
        }
        #endregion
        #region Horizontal Axis
        if (Input.GetAxisRaw("P1_Horizontal") == 1)
        {
            if (P1_LeftStickRightAction != null)
            {
                p1_LeftStickZeroed = false;
                P1_LeftStickRightAction();
            }
        }
        else if(Input.GetAxisRaw("P1_Horizontal") == -1)
        {
            if (P1_LeftStickLeftAction != null)
            {
                p1_LeftStickZeroed = false;
                P1_LeftStickLeftAction();
            }
        }
        #endregion
        #region Vertical Axis
        if (Input.GetAxisRaw("P1_Vertical") == 1)
        {
            if (P1_LeftStickDownAction != null)
            {
                p1_LeftStickZeroed = false;
                P1_LeftStickDownAction();
            }
        }
        else if (Input.GetAxisRaw("P1_Vertical") == -1)
        {
            if (P1_LeftStickUpAction != null)
            {
                p1_LeftStickZeroed = false;
                P1_LeftStickUpAction();
            }
        }
        #endregion
        #region LeftStickZero
        if (Input.GetAxis("P1_Horizontal") < 0.3f && Input.GetAxis("P1_Vertical") < 0.3f && Input.GetAxis("P1_Horizontal") > -0.3f && Input.GetAxis("P1_Vertical") > -0.3f)
        {
            if (!p1_LeftStickZeroed && P1_LeftStickZeroAction != null)
            {
                p1_LeftStickZeroed = true;
                P1_LeftStickZeroAction();
            }
        }
        #endregion
        #region TriggerLeft
        if (Input.GetAxisRaw("P1_LEFTTrigger") == 1 && !P1_TriggerStateLeft)
        {
            P1_TriggerStateLeft = true;
            if (P1_LeftTriggerDownAction != null)
                P1_LeftTriggerDownAction();
        }
        else if(Input.GetAxisRaw("P1_LEFTTrigger") == 0 && P1_TriggerStateLeft)
        {
            P1_TriggerStateLeft = false;
            if (P1_LeftTriggerUpAction != null)
                P1_LeftTriggerUpAction();
        }
        #endregion
        #region TriggerRight
        if (Input.GetAxisRaw("P1_RIGHTTrigger") == -1 && !P1_TriggertStateRight)
        {
            P1_TriggertStateRight = true;
            if (P1_RightTiggerDownAction != null)
                P1_RightTiggerDownAction();
        }
        else if (Input.GetAxisRaw("P1_RIGHTTrigger") == 0 && P1_TriggertStateRight)
        {
            P1_TriggertStateRight = false;
            if (P1_RightTiggerUpAction != null)
                P1_RightTiggerUpAction();
        }
        #endregion
        #region START / BACK
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (p1_OnKeyPressed != null)
                p1_OnKeyPressed(KeyCode.Joystick1Button7);
        }
        if(Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            if (p1_OnKeyPressed != null)
                p1_OnKeyPressed(KeyCode.Joystick1Button6);
        }
        #endregion

        //Player 2
        #region A Button
        if (Input.GetKeyDown(KeyCode.Joystick2Button0))
        {
            if (p2_OnKeyPressed != null)
                p2_OnKeyPressed(KeyCode.Joystick2Button0);
        }
        else if (Input.GetKeyUp(KeyCode.Joystick2Button0))
        {
            if (p2_OnKeyReleased != null)
                p2_OnKeyReleased(KeyCode.Joystick2Button0);
        }
        #endregion
        #region B Button
        //if (Input.GetKeyDown(KeyCode.Joystick2Button1) && Input.GetAxisRaw("P2_Horizontal") == 1)
        //{
        //    if (P2_SpecalRightAction != null)
        //        P2_SpecalRightAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick2Button1) && Input.GetAxisRaw("P2_Horizontal") == -1)
        //{
        //    if (P2_SpecalLeftAction != null)
        //        P2_SpecalLeftAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick2Button1) && Input.GetAxisRaw("P2_Vertical") == 1)
        //{
        //    if (P2_SpecalBottomAction != null)
        //        P2_SpecalBottomAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick2Button1) && Input.GetAxisRaw("P2_Vertical") == -1)
        //{
        //    if (P2_SpecalTopAction != null)
        //        P2_SpecalTopAction();
        //}
        if (Input.GetKeyDown(KeyCode.Joystick2Button1))
        {
            if (p2_OnKeyPressed != null)
                p2_OnKeyPressed(KeyCode.Joystick2Button1);
        }
        if (Input.GetKeyUp(KeyCode.Joystick2Button1))
        {
            if (p2_OnKeyReleased != null)
                p2_OnKeyReleased(KeyCode.Joystick2Button1);
        }
        #endregion
        #region X Button
        if (Input.GetKeyDown(KeyCode.Joystick2Button2) && Input.GetAxisRaw("P2_Horizontal") >= 0.6f && Input.GetAxisRaw("P2_Vertical") <= -0.6f)
        {
            if (P2_XButtonDirectionAction != null)
                P2_XButtonDirectionAction(Direction.RightUpAngel);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2) && Input.GetAxisRaw("P2_Horizontal") >= 0.6f && Input.GetAxisRaw("P2_Vertical") >= 0.6f)
        {
            if (P2_XButtonDirectionAction != null)
                P2_XButtonDirectionAction(Direction.RightDownAngel);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2) && Input.GetAxisRaw("P2_Horizontal") <= -0.6f && Input.GetAxisRaw("P2_Vertical") <= -0.6f)
        {
            if (P2_XButtonDirectionAction != null)
                P2_XButtonDirectionAction(Direction.LeftUpAngel);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2) && Input.GetAxisRaw("P2_Horizontal") <= -0.6f && Input.GetAxisRaw("P2_Vertical") >= 0.6f)
        {
            if (P2_XButtonDirectionAction != null)
                P2_XButtonDirectionAction(Direction.LeftDownAngel);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2) && Input.GetAxisRaw("P2_Horizontal") == 1)
        {
            if (P2_XButtonDirectionAction != null)
                P2_XButtonDirectionAction(Direction.Right);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2) && Input.GetAxisRaw("P2_Horizontal") == -1)
        {
            if (P2_XButtonDirectionAction != null)
                P2_XButtonDirectionAction(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2) && Input.GetAxisRaw("P2_Vertical") == 1)
        {
            if (P2_XButtonDirectionAction != null)
                P2_XButtonDirectionAction(Direction.Down);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2) && Input.GetAxisRaw("P2_Vertical") == -1)
        {
            if (P2_XButtonDirectionAction != null)
                P2_XButtonDirectionAction(Direction.Up);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2))
        {
            if (p2_OnKeyPressed != null)
                p2_OnKeyPressed(KeyCode.Joystick2Button2);
            if (P2_XButtonDirectionAction != null)
                P2_XButtonDirectionAction(Direction.None);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick2Button2))
        {
            if (p2_OnKeyReleased != null)
                p2_OnKeyReleased(KeyCode.Joystick2Button2);
        }
        #endregion
        #region Y Button
        //if (Input.GetKeyDown(KeyCode.Joystick2Button3) && Input.GetAxisRaw("P2_Horizontal") == 1)
        //{
        //    if (P2_SpecalRightAction != null)
        //        P2_SpecalRightAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick2Button3) && Input.GetAxisRaw("P2_Horizontal") == -1)
        //{
        //    if (P2_SpecalLeftAction != null)
        //        P2_SpecalLeftAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick2Button3) && Input.GetAxisRaw("P2_Vertical") == 1)
        //{
        //    if (P2_SpecalBottomAction != null)
        //        P2_SpecalBottomAction();
        //}
        //else if (Input.GetKeyDown(KeyCode.Joystick2Button3) && Input.GetAxisRaw("P2_Vertical") == -1)
        //{
        //    if (P2_SpecalTopAction != null)
        //        P2_SpecalTopAction();
        //}
       if (Input.GetKeyDown(KeyCode.Joystick2Button3))
        {
            if (p2_OnKeyPressed != null)
                p2_OnKeyPressed(KeyCode.Joystick2Button3);
        }
        if (Input.GetKeyUp(KeyCode.Joystick2Button3))
        {
            if (p2_OnKeyReleased != null)
                p2_OnKeyReleased(KeyCode.Joystick2Button3);
        }
        #endregion
        #region Horizontal Axis
        if (Input.GetAxisRaw("P2_Horizontal") == 1)
        {
            if (P2_LeftStickRightAction != null)
            {
                p2_LeftStickZeroed = false;
                P2_LeftStickRightAction();
            }
        }
        else if (Input.GetAxisRaw("P2_Horizontal") == -1)
        {
            if (P2_LeftStickLeftAction != null)
            {
                p2_LeftStickZeroed = false;
                P2_LeftStickLeftAction();
            }
        }
        #endregion
        #region Vertical Axis
        if (Input.GetAxisRaw("P2_Vertical") == 1)
        {
            if (P2_LeftStickDownAction != null)
            {
                p2_LeftStickZeroed = false;
                P2_LeftStickDownAction();
            }
        }
        else if (Input.GetAxisRaw("P2_Vertical") == -1)
        {
            if (P2_LeftStickUpAction != null)
            {
                p2_LeftStickZeroed = false;
                P2_LeftStickUpAction();
            }
        }
        #endregion
        #region LeftStickZero
        if (Input.GetAxis("P2_Horizontal") < 0.3f && Input.GetAxis("P2_Vertical") < 0.3f && Input.GetAxis("P2_Horizontal") > -0.3f && Input.GetAxis("P2_Vertical") > -0.3f)
        {
            if (!p2_LeftStickZeroed && P2_LeftStickZeroAction != null)
            {
                p2_LeftStickZeroed = true;
                P2_LeftStickZeroAction();
            }
        }
        #endregion
        #region TriggerLeft
        if (Input.GetAxisRaw("P2_LEFTTrigger") == 1 && !P2_TriggerStateLeft)
        {
            P2_TriggerStateLeft = true;
            if (P2_LeftTriggerDownAction != null)
                P2_LeftTriggerDownAction();
        }
        else if (Input.GetAxisRaw("P2_LEFTTrigger") == 0 && P2_TriggerStateLeft)
        {
            P2_TriggerStateLeft = false;
            if (P2_LeftTriggerUpAction != null)
                P2_LeftTriggerUpAction();
        }
        #endregion
        #region TriggerRight
        if (Input.GetAxisRaw("P2_RIGHTTrigger") == -1 && !P2_TriggertStateRight)
        {
            P2_TriggertStateRight = true;
            if (P2_RightTiggerDownAction != null)
                P2_RightTiggerDownAction();
        }
        else if (Input.GetAxisRaw("P2_RIGHTTrigger") == 0 && P2_TriggertStateRight)
        {
            P2_TriggertStateRight = false;
            if (P2_RightTiggerUpAction != null)
                P2_RightTiggerUpAction();
        }
        #endregion
        #region START / BACK
        if (Input.GetKeyDown(KeyCode.Joystick2Button7))
        {
            if (p2_OnKeyPressed != null)
                p2_OnKeyPressed(KeyCode.Joystick2Button7);
        }
        if (Input.GetKeyDown(KeyCode.Joystick2Button6))
        {
            if (p2_OnKeyPressed != null)
                p2_OnKeyPressed(KeyCode.Joystick2Button6);
        }
        #endregion
    }
}
