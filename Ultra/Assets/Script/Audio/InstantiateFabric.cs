using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFabric : MonoBehaviour {

    public GameObject AudioManager;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Audio") == null) {
            var audioObject = Instantiate(AudioManager, Vector3.zero, Quaternion.identity);
            audioObject.name = "Audio";
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
