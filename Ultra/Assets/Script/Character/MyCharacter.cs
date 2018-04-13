using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacter : MonoBehaviour
{
    [HideInInspector]
    public PlayerEnum playerEnum = PlayerEnum.NotAssigned;
    [HideInInspector]
    public bool canGetDamaged = true;
    [HideInInspector]
    public bool isDisabled = false;
    [HideInInspector]
    public bool isStunned = false;

    public void Posses()
    {
        this.gameObject.GetComponent<Movement>().AssigneInput();
    }

    void Awake()
    {
       
    }

    void Start ()
    {

	}

	void Update ()
    {
		
	}
    

    void Spawn()
    {

    }

    void Respawn()
    {

    }

    void Dead()
    {

    }
}
