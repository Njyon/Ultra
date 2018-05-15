using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buttons : MonoBehaviour {

    int triggerCounter = 0;
    Material material;
    Color resetCol;
    GameObject playerCol;

    [Header("Colors")]
    public Color onHoverColor;
    public Color activationColor;
    public UnityEvent ActivationEvent;



    ////////////////////////////////////////////////////////
    ///////////         Awake - Start         /////////////
    //////////////////////////////////////////////////////

    void Awake()
    {
        if (ActivationEvent == null)
            ActivationEvent = new UnityEvent();
    }

    void Start ()
    {
        playerCol = GameObject.Find("pref_Nav");
        material = gameObject.GetComponent<Renderer>().material;
        resetCol = material.color;
    }


    ////////////////////////////////////////////////////////
    ////////////          On-Trigger         //////////////
    //////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            CollisionScipt colPlayer = other.gameObject.GetComponent<CollisionScipt>();
            MyCharacter collidingPlayer = colPlayer.myCharacter;
            
            switch (collidingPlayer.playerEnum)
            {
                case PlayerEnum.PlayerOne:
                    AssigneInputP1();
                    break;
                case PlayerEnum.PlayerTwo:
                    AssigneInputP2();
                    break;
            }
            if(triggerCounter == 0)
            {
                OnHover();
            }
            triggerCounter++;

            Debug.Log("im in");
        }
        else if (other.gameObject.tag != "player")
        {
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            CollisionScipt colPlayer = other.gameObject.GetComponent<CollisionScipt>();
            MyCharacter collidingPlayer = colPlayer.myCharacter;
            switch (collidingPlayer.playerEnum)
            {
                case PlayerEnum.PlayerOne:
                    RemoveInputP1();
                    break;
                case PlayerEnum.PlayerTwo:
                    RemoveInputP2();
                    break;
            }
            triggerCounter--;
            if(triggerCounter == 0)
            {
                EndHover();
            }
        }else if (other.gameObject.tag != "player")
        {
            return;
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
    ////////////          Functions          //////////////
    //////////////////////////////////////////////////////

    void P1_CheckInputDown(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick1Button2)
            OnButtonDown();
    }

    void P2_CheckInputDown(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick1Button2)
            OnButtonDown();
    }


    void OnButtonDown()
    {
        material.color = activationColor;
        ActivationEvent.Invoke();
        Invoke("EndActivationEvent", 0.2f);
    }

    void OnHover()
    {
        ChangeColor(onHoverColor);
    }

    void EndHover()
    {
        ChangeColor(resetCol);
    }

    void EndActivationEvent()
    {
        if (triggerCounter > 0)
            ChangeColor(onHoverColor);
    }

    void ChangeColor(Color color)
    {
        material.color = color;
    }
}
