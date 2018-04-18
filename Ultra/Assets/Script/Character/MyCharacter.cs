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

    Rigidbody rb;

    int lifes = 3;

    float dmgMultiplier = 1.5f;
    float percent = 0;


    GameObject tEST;

    public void Posses()
    {
        this.gameObject.GetComponent<Movement>().AssigneInput();
        rb = GetComponent<Rigidbody>();
    }

    void Awake()
    {
        tEST = GameObject.Find("TEST");
        InputManager.P1_YButtonDownAction += TEST;
    }

    void Start ()
    {

	}

    void TEST()
    {
        Damage(tEST.transform.position, 20);
    }

    //////////////////////////////////////////////////
    ////////////////       Update       //////////////
    //////////////////////////////////////////////////

    void Update ()
    {
		
	}

    //////////////////////////////////////////////////
    ////////////////      Functions     //////////////
    //////////////////////////////////////////////////

    void Respawn()
    {
        
    }

    void Dead()
    {
        if(lifes > 0)
        {
            lifes--;
            Respawn();
        }
    }

    void Damage(Vector3 enemyTran, int damage)
    {
        Debug.Log("LOL");
        percent = percent * dmgMultiplier + damage;

        enemyTran = new Vector3(enemyTran.x * this.transform.position.x, enemyTran.y * this.transform.position.y, enemyTran.z * this.transform.position.z);
        Debug.DrawLine(this.transform.position, enemyTran, Color.red);
        rb.AddForce(enemyTran * percent);
    }
    
}
