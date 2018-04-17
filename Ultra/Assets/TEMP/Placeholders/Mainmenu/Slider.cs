using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour {

    private GameObject playerObj;
    private float xValue = 0.0f;
    private float incrementX = 1.0f;

    private bool inTrigger = false;




    // Use this for initialization
    void Start () {

        playerObj = GameObject.Find("player");

        return;
    }
	
	// Update is called once per frame
	void Update () {




        //if (Input.GetButtonDown("P1_YButton"))
        //{
        //    Debug.Log("trigger_Y");
        //    Activated();
        //}
        //else if (Input.GetButtonDown("P1_XButton"))
        //{
        //    Debug.Log("trigger_X");
        //    Activated();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        Activated();
        //how to acces a specific collider that collides with this trigger?
    }

    private void OnTriggerExit(Collider other)
    {
        inTrigger = false;
    }

    private void Activated()
    {

        if (xValue >= -5 && xValue <= 5)
        {
            if(Input.GetButtonDown("P1_YButton"))
            {
                xValue += incrementX;
                Debug.Log("Y");

                transform.Translate(xValue, 0, 0);
                Debug.Log(transform.position.x + "  :  " + xValue);
            }
            else if(Input.GetButtonDown("P1_XButton"))
            {
                xValue -= incrementX;
                Debug.Log("X");

                transform.Translate(xValue, 0, 0);
                Debug.Log(transform.position.x + "  :  " + xValue);
            }
        }
        else
        {
            Debug.Log("not met");
        }
    } 
}
