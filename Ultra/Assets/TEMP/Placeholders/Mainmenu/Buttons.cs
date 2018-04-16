using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : mmTest {
    
    GameObject[] player;
    Color resetCol;
    Material origMat;



    // Use this for initialization
    void Start () {
		player = GameObject.FindGameObjectsWithTag("player");

        origMat = GetComponent<Renderer>().material;
        resetCol = origMat.color;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter(Collider player)
    {
        ChangeCol(Color.red);
        
    }

    private void OnTriggerExit(Collider trigger)
    {
        ChangeCol(resetCol);
    }

    private void ChangeCol(Color newColor)
    {
        Material rend = GetComponent<Renderer>().material;
        rend.color = newColor;
    }
}
