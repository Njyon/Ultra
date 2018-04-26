using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Buttons : MonoBehaviour {

    int triggerCounter = 0;
    MainMenu mainMenu;
    Material material;
    Color resetCol;

    [Header("Colors")]
    public Color onHoverColor;
    public Color activationColor;
    public UnityEvent ActivationEvent;


    void Awake()
    {
        if (ActivationEvent == null)
            ActivationEvent = new UnityEvent();
    }

    void Start ()
    {
        material = gameObject.GetComponent<Renderer>().material;
        resetCol = material.color;

        mainMenu = GameObject.Find("EventSystem").GetComponent<MainMenu>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            MyCharacter collidingPlayer = other.gameObject.GetComponent<MyCharacter>();
            switch(collidingPlayer.playerEnum)
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
            triggerCounter--;
            if(triggerCounter == 0)
            {
                EndHover();
            }
        }
    }

    //----------------Input--------------------//

    void AssigneInputP1()
    {
        InputManager.P1_XButtonDownAction += OnButtonDown;
    }

    void AssigneInputP2()
    {
        InputManager.P2_XButtonDownAction += OnButtonDown;
    }

    void RemoveInputP1()
    {
        InputManager.P1_XButtonDownAction -= OnButtonDown;
    }

    void RemoveInputP2() 
    {
        InputManager.P2_XButtonDownAction -= OnButtonDown;
    }

    //----------------Functions--------------------//

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
