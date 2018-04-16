using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : mmTest {

    private GameObject[] player;
    //public GameObject slider;
    private int sliderCurrent;
    private int sliderRange = 10;
    public int sliderStart = 0;
    private Vector3 sliderStep;

    private bool inTrigger = false;
    private bool plus;




    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectsWithTag("player");

        sliderCurrent = sliderStart;
        sliderStep = new Vector3(1.0f, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxisRaw("P1_RIGHTTrigger") == 1)
        {
            Debug.Log("trigger_RIGHT");
            Activated(true);
        }else if (Input.GetAxisRaw("P1_LEFTTrigger") == 1)
        {
            Debug.Log("trigger_LEFT");
            Activated(false);
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        //Debug.Log("Enter");
        inTrigger = true;

           
    }

    private void OnTriggerExit(Collider player)
    {
        //Debug.Log("Exit");
        inTrigger = false;

    }

    private void Activated(bool add)
    {
        if(this.transform.position.x >= -5 && this.transform.position.x <= 5 && add == true)
        {
            this.transform.position += sliderStep;
            //sliderCurrent++;
            Debug.Log("1");

        }else if (this.transform.position.x >= -5  && this.transform.position.x <= 5 && add == false)
        {
            Debug.Log("2");
            this.transform.position -= sliderStep;
            //sliderCurrent--;
           
        }
        else
        {
            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            Debug.Log("not met");
        }
    } 
}
