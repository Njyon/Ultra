using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MySlider : MonoBehaviour {
    #region Variables
    //--Public--//

    public float minValueX;
    public float maxValueX;
    public string mixerParameter;
    [HideInInspector] public bool increase;

    public UnityEvent ButtonPressEventX;
    public UnityEvent ButtonPressEventY;

    //--Private--//


    //Transform playerTransform;
    //float incrementVal = 1.0f;
    [HideInInspector] public bool increased;
    

    #endregion

    ////////////////////////////////////////////////////////
    ////////////            Awake            //////////////
    //////////////////////////////////////////////////////

    private void Awake()
    {
        if (ButtonPressEventX == null)
            ButtonPressEventX = new UnityEvent();

        if (ButtonPressEventY == null)
            ButtonPressEventY = new UnityEvent();
    }

    ////////////////////////////////////////////////////////
    ////////////          On-Trigger         //////////////
    //////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "player")
        {
            //CollisionScipt colPlayer = other.gameObject.GetComponent<CollisionScipt>();
            //MyCharacter collidingPlayer = colPlayer.myCharacter;
            //switch (collidingPlayer.playerEnum)
            //{
            //    case PlayerEnum.PlayerOne:
            //        AssigneInputP1();
            //        break;
            //    case PlayerEnum.PlayerTwo:
            //        AssigneInputP2();
            //        break;
            //}
        }

        if (this.transform.position.x > other.transform.position.x)
        {
            increased = true;
        }
        else if(this.transform.position.x < other.transform.position.x)
        {
            increased = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            //CollisionScipt colPlayer = other.gameObject.GetComponent<CollisionScipt>();
            //MyCharacter collidingPlayer = colPlayer.myCharacter;
            //switch (collidingPlayer.playerEnum)
            //{
            //    case PlayerEnum.PlayerOne:
            //        RemoveInputP1();
            //        break;
            //    case PlayerEnum.PlayerTwo:
            //        RemoveInputP2();
            //        break;
            //}
        }

    }

    ////////////////////////////////////////////////////////
    ////////////            Input            //////////////
    //////////////////////////////////////////////////////

    void AssigneInputP1()
    {
        InputManager.p1_OnKeyPressed += P1_CheckInputDown;
    }

    void AssigneInputP2()
    {
        InputManager.p2_OnKeyPressed += P2_CheckInputDown;
    }

    void RemoveInputP1()
    {
        InputManager.p1_OnKeyPressed -= P1_CheckInputDown;
    }

    void RemoveInputP2()
    {
        InputManager.p2_OnKeyPressed -= P2_CheckInputDown;
    }

    ////////////////////////////////////////////////////////
    ///////////        Input Functions        /////////////
    //////////////////////////////////////////////////////

    void P1_CheckInputDown(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick1Button2)
            OnButtonDownX();

        if (keyCode == KeyCode.Joystick1Button3)
            OnButtonDownY();
    }

    void P2_CheckInputDown(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick2Button2)
            OnButtonDownX();

        if (keyCode == KeyCode.Joystick1Button3)
            OnButtonDownY();
    }

    void OnButtonDownX()
    {
        SliderChange(1.0f);
        ButtonPressEventX.Invoke();
       
    }

    void OnButtonDownY()
    {
        SliderChange(2.0f);
        ButtonPressEventY.Invoke();
    }

    ////////////////////////////////////////////////////////
    ////////////          Functions          //////////////
    //////////////////////////////////////////////////////

    #region SliderChange

    public void SliderChange(float incrementVal)  //Transform transform)
    {

        if (increased == true)      //this.transform.position.x > transform.position.x)
        {
            //Increase
            this.transform.position = new Vector3(this.transform.position.x + incrementVal, this.transform.position.y, 0);

            if (this.transform.position.x >= maxValueX)
            {
                this.transform.position = new Vector3(maxValueX, this.transform.position.y, this.transform.position.z);
            }
        }
        else if (increased == false)    //this.transform.position.x < transform.position.x)
        {
            //decrease
            this.transform.position = new Vector3(this.transform.position.x - incrementVal, this.transform.position.y, 0);

            if (this.transform.position.x <= minValueX)
            {
                this.transform.position = new Vector3(minValueX, this.transform.position.y, this.transform.position.z);
            }
        }

    }
    #endregion
    
}


