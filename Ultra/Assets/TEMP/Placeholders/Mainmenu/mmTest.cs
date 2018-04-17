using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mmTest : MonoBehaviour {

    private GameObject[] buttons;




    // Use this for initialization
    void Start()
    {

        

        #region AssignInput

        InputManager.P1_BButtonDownAction += Back;
        InputManager.P1_XButtonDownAction += Confirm;
        InputManager.P1_YButtonDownAction += Special;
        InputManager.P1_LeftStickRightAction += Increase;
        InputManager.P1_LeftStickLeftAction += Decrease;

        #endregion



    }

    // Update is called once per frame
    void Update () {
		
	}


    void Back()
    {

    }

    void Confirm()
    {

    }

    void Special()
    {

    }

    void Increase()
    {

    }

    void Decrease()
    {

    }

   
}
