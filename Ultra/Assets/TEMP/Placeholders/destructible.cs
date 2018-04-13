using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructible : MonoBehaviour {


    public GameObject trigger;
    private GameObject[] reactors;
    private Rigidbody rb;
    private Vector3 pos;


    // Use this for initialization
    void Start () {
        reactors = GameObject.FindGameObjectsWithTag("destruct");
       // ResetPos();
        FreezeMovement();

    }
	
	// Update is called once per frame
	void Update () {
       
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
            reactor.transform.position = pos;
        }
    }

    private void UnfreezeMovement()
    {
        foreach (GameObject reactor in reactors)
        {
            rb = reactor.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
            Debug.Log(rb.constraints);
        }
    }

    private void ResetPos()
    {
        foreach (GameObject reactor in reactors)
        {
            pos = reactor.transform.position;

        }

    }
}
