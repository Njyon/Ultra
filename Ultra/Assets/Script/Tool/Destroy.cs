using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float destroyTimer;
    
	void Start ()
    {
        Destroy(this.gameObject, destroyTimer);
	}
}
