using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class Slider : MonoBehaviour {

    //--------------------------LIST-------------------------------//
    #region Variables
    //--Public--//

    public float minValueX;
    public float maxValueX;
    public string mixerParameter;

    public UnityEvent ButtonPressEventX;
    public UnityEvent ButtonPressEventY;

    //--Private--//
    
    //GameObject playerChar;

    float incrementVal = 1.0f;
    float posX;

    #endregion

    //-------------------------START - UPDATE--------------------------------//


    private void Awake()
    {
        if (ButtonPressEventX == null)
            ButtonPressEventX = new UnityEvent();

        if (ButtonPressEventY == null)
            ButtonPressEventY = new UnityEvent();
    }

    private void Start()
    {
        
        
        posX = this.transform.position.x;
    }




    //------------------------ON-TRIGGER--------------------------//

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            //playerChar = other.gameObject;

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
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
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

    // -------------------INPUT -----------------------------------//

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

    // ---------Input Functions -----------------------//

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
        ButtonPressEventX.Invoke();

        //ChangeSliderVolume(1.0f);
    }

    void OnButtonDownY()
    {
        ButtonPressEventY.Invoke();
        //ChangeSliderVolume(2.0f);
    }

    // -------------------OLD FUNCTIONS---------------------------------//

    //void ChangeSliderVolume(float incrementVal)
    //{
    //    MainMenu mainMenu = GameObject.Find("EventSystem").GetComponent<MainMenu>();

    //    if (playerChar.transform.position.x - posX < posX)
    //    {
    //        //increase
    //        this.transform.position += Vector3.right * incrementVal;
    //        mainMenu.ChangeVolume(true, incrementVal * 10, mixerParameter);
    //        Debug.Log("TRUE");

    //        if(posX > maxValueX)
    //        {
    //            posX = maxValueX;
    //        }

    //    }
    //    else if (playerChar.transform.position.x - posX > posX)
    //    {
    //        //decrease
    //        this.transform.position += Vector3.left * incrementVal;
    //        mainMenu.ChangeVolume(false, -incrementVal * 10, mixerParameter);
    //        Debug.Log("posX: " + posX + " playerPos: " + playerChar.transform.position.x);

    //        if(posX < minValueX)
    //        {
    //            posX = minValueX;
    //        }
    //    }

    //    this.transform.position = new Vector3(posX, this.transform.position.y, this.transform.position.z);
    //}



    public void SliderChange(Transform transform)
    {

        if(transform.position.x - this.transform.position.x < this.transform.position.x)
        {
            //Increase
            this.transform.position = new Vector3(this.transform.position.x + incrementVal, this.transform.position.y, 0);
                
                //Vector3.right * incrementVal;

            if (this.transform.position.x > maxValueX)
                this.transform.position = new Vector3(maxValueX, this.transform.position.y, this.transform.position.z);
        }
        else if (transform.position.x - this.transform.position.x > this.transform.position.x)
        {
            //decrease
            this.transform.position = new Vector3(this.transform.position.x - incrementVal, this.transform.position.y, 0);

            if (this.transform.position.x < minValueX)
                this.transform.position = new Vector3(minValueX, this.transform.position.y, this.transform.position.z);
        }

        //this.transform.position = new Vector3(posX, this.transform.position.y, 0);

        Debug.Log("posX: " + this.transform.position.x + " playerPos: " + transform.position.x);
    }


    #region Old Functions
    //private void Activated()
    //{

    //    if (inTrigger == true)
    //    {
    //        if (Input.GetButtonDown("P1_YButton") )
    //        {
    //            //Increase
    //            if (this.gameObject.transform.position.x < maxValueX)
    //            {
    //                this.transform.position += Vector3.right * incrementX;
    //                ChangeVolume(true);
    //            }
    //        }
    //        else if(Input.GetButtonDown("P1_XButton"))
    //        {
    //            //Decrease
    //            if (this.gameObject.transform.position.x > minValueX)
    //            {
    //                this.transform.position += Vector3.left * incrementX;
    //                ChangeVolume(false);
    //            }
    //        }


    //    }

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


