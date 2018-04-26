using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour {

    private MainMenu mainMenu;

    void Start () {

        mainMenu = GameObject.Find("EventSystem").GetComponent<MainMenu>();
    }


	void Update () {
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mainMenu.inTrigger = true;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        mainMenu.inTrigger = true;
    //    }
    //}


    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            mainMenu.inTrigger = false;
        }
    }



    //private void HitButton()
    //{
    //    if (inTrigger == true && gameObject.tag == "ScrollButton")
    //    {
    //        if (Input.GetButtonDown("P1_XButton"))
    //        {
    //            anim.SetBool("onButtonPressed", true);
    //            anim.Play("anim_buttonWiggle");
    //            //ChangeCol(Color.green);

    //        }
    //        else if (Input.GetButtonUp("P1_XButton"))
    //        {
    //            anim.SetBool("onButtonPressed", false);
    //            //ChangeCol(resetCol);
    //        }
    //    }else if (inTrigger == true && gameObject.tag != "ScrollButton")
    //    {
    //        //ChangeCol(Color.red);
    //    }
    //}
}
