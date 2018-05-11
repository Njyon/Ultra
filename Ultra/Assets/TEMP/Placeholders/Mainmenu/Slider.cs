using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slider : MonoBehaviour {
    #region Variables
    //--Public--//

    public float minValueX;
    public float maxValueX;
    public string mixerParameter;
    [HideInInspector] public bool increase;

    public UnityEvent ButtonPressEventX;
    public UnityEvent ButtonPressEventY;

    //--Private--//

    float incrementVal = 1.0f;
    

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
            MyCharacter collidingPlayer = other.gameObject.GetComponent<MyCharacter>();
            switch (collidingPlayer.playerEnum)
            {
                case PlayerEnum.PlayerOne:
                    AssigneInputP1();
                    break;
                case PlayerEnum.PlayerTwo:
                    AssigneInputP2();
                    break;
            }
        }else if (other == null)
        {
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            MyCharacter collidingPlayer = other.gameObject.GetComponent<MyCharacter>();
            switch (collidingPlayer.playerEnum)
            {
                case PlayerEnum.PlayerOne:
                    RemoveInputP1();
                    break;
                case PlayerEnum.PlayerTwo:
                    RemoveInputP2();
                    break;
            }
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
        incrementVal = 1.0f;
        ButtonPressEventX.Invoke();
        Debug.Log("x");
    }

    void OnButtonDownY()
    {
        incrementVal = 2.0f;
        ButtonPressEventY.Invoke();
        Debug.Log("y");

    }

    ////////////////////////////////////////////////////////
    ////////////          Functions          //////////////
    //////////////////////////////////////////////////////

    #region SliderChange

    public void SliderChange(Transform transform, bool pressingX)
    {
        if (pressingX == true)
        {
            OnButtonDownX();
        }
        else if (pressingX == false)
        {
            OnButtonDownY();
        }


        if (this.transform.position.x > transform.position.x)
        {
            //Increase
            this.transform.position = new Vector3(this.transform.position.x + incrementVal, this.transform.position.y, 0);

            if (this.transform.position.x >= maxValueX)
            {
                this.transform.position = new Vector3(maxValueX, this.transform.position.y, this.transform.position.z);
            }
        }
        else if (this.transform.position.x < transform.position.x)
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

    #region Old Functions
    //private void Activated()
    //{


    //            if (transform.position.x - this.transform.position.x<posX)
    //        {
    //            //Increase

    //            this.transform.position = new Vector3(posX + incrementVal, this.transform.position.y, 0);
    //    Debug.Log("+");
    //            if (posX > maxValueX)
    //            {
    //                this.transform.position = new Vector3(maxValueX, this.transform.position.y, this.transform.position.z);
    //}
    //        }
    //        else if (transform.position.x - this.transform.position.x > posX)
    //        {
    //    //decrease
    //    this.transform.position = new Vector3(posX - incrementVal, this.transform.position.y, 0);
    //    Debug.Log("-");
    //    if (posX < minValueX)
    //    {
    //        this.transform.position = new Vector3(minValueX, this.transform.position.y, this.transform.position.z);
    //    }
    //}
    //        else
    //        {
    //    return;
    //}


    //} 

    //private void ChangeVolume(bool add)
    //{
    //    currentAudio = Mathf.Round(currentAudio * 10f) / 10f;

    //    float incrementer = ((minValueX - maxValueX) / (minAudio - maxAudio)) * 100;

    //    if (add == true)
    //    {
    //        //Increase
    //        currentAudio = currentAudio + incrementer;

    //        if (currentAudio > maxAudio)
    //        {
    //            currentAudio = maxAudio;
    //        }

    //    }
    //    else if (add == false)
    //    {
    //        //Decrease
    //        currentAudio = currentAudio - incrementer;

    //        if (currentAudio < minAudio)
    //        {
    //            currentAudio = minAudio;
    //        }
    //    }

    //    currentMixerVol = currentAudio;
    //    aMixer.SetFloat(mixerParameter, currentMixerVol);
    //}


    #endregion Old Functions
}


