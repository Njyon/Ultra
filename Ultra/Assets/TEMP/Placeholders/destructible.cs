using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructible : MonoBehaviour {

    //Private
    private GameObject[] reactors;
    private Rigidbody rb;
    private Renderer rend;


    public float MoveSpeed = 10;
    public float RotateSpeed = 40;
    private Vector3 origPos;
    private float origPosX;


    //Public
    public GameObject trigger;
    public GameObject AButton;
    public GameObject BButton;
    public GameObject XButton;
    public GameObject YButton;
    public GameObject JoyLeft;
    //public GameObject JoyRight;



    // Use this for initialization
    void Start () {
        //float MoveForward = Input.GetAxis("Horizontal");
        
        reactors = GameObject.FindGameObjectsWithTag("destruct");
        FreezeMovement();
        origPos = new Vector3(JoyLeft.transform.position.x, JoyLeft.transform.position.y, JoyLeft.transform.position.z);
    }

    // Update is called once per frame
    void Update() {

        //A
        if (Input.GetButtonDown("P1_AButton"))
        {
            ChangeColor(AButton, Color.green);
            Debug.Log("A");
        }
        else if (Input.GetButtonUp("P1_AButton"))
        {
            ResetCol(AButton);
        }


        //B
        if (Input.GetButtonDown("P1_BButton"))
        {
            ChangeColor(BButton, Color.red);
            Debug.Log("B");
        }
        else if (Input.GetButtonUp("P1_BButton"))
        {
            ResetCol(BButton);
        }


        //X
        if (Input.GetButtonDown("P1_XButton"))
        {
            ChangeColor(XButton, Color.blue);
            Debug.Log("X");
        }
        else if (Input.GetButtonUp("P1_XButton"))
        {
            ResetCol(XButton);
        }


        //Y
        if (Input.GetButtonDown("P1_YButton"))
        {
            ChangeColor(YButton, Color.yellow);
            Debug.Log("Y");
        }
        else if (Input.GetButtonUp("P1_YButton"))
        {
            ResetCol(YButton);
        }

        //Horizontal
        if (Input.GetAxis("P1_Horizontal") == 1)
        {
            Debug.Log("joyLeft.x");
            float MoveForward = Input.GetAxis("P1_Horizontal");
            JoyLeft.transform.Translate(Vector3.right * MoveForward);
            ChangeColor(JoyLeft, Color.white);
        }
        else if (Input.GetAxis("P1_Horizontal") == -1)
        {
            float MoveForward = Input.GetAxis("P1_Horizontal");
            JoyLeft.transform.Translate(Vector3.right / MoveForward);
            ChangeColor(JoyLeft, Color.white);
        }
        else
        {
            JoyLeft.transform.position = origPos;
            ResetCol(JoyLeft);
        }


        //Vertical
        if (Input.GetAxis("P1_AButton") == -1)
        {
            Debug.Log("jump");
            float MoveSide = Input.GetAxis("P1_AButton");
            JoyLeft.transform.Translate((Vector3.up * MoveSide)*2);
            ChangeColor(JoyLeft, Color.white);
        }
        else if (Input.GetAxis("P1_AButton") == 1)
        {
            float MoveSide = Input.GetAxis("P1_AButton");
            JoyLeft.transform.Translate((Vector3.up / MoveSide)*2);
            ChangeColor(JoyLeft, Color.white);
        }
        else
        {
            JoyLeft.transform.position = origPos;
            ResetCol(JoyLeft);
        }
    }


    private void OnTriggerEnter(Collider trigger)
    {
        UnfreezeMovement();
    }

    private void OnTriggerExit(Collider trigger)
    {
        FreezeMovement();
    }

    private void FreezeMovement()
    {
        foreach (GameObject reactor in reactors)
        {

            rb = reactor.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeAll;
            //reactor.transform.position = pos;
        }
    }

    private void UnfreezeMovement()
    {
        foreach (GameObject reactor in reactors)
        {
            rb = reactor.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    private void ResetPos()
    {
        //foreach (GameObject reactor in reactors)
        //{
        //    for (int i, i <= reactors[], i++) {
        //        pos = reactor.transform.position;
             
        //    }
        //}

    }

    private void ChangeColor(GameObject button, Color newColor)
    {
        //button = button.GetComponent<Renderer>();
        Material rend = button.GetComponent<Renderer>().material;
        rend.color = newColor;
    } 

    private void ResetCol(GameObject button)
    {
            ChangeColor(button, Color.grey);
    }
}
