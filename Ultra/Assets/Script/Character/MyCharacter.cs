using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacter : MonoBehaviour
{
    [HideInInspector] public PlayerEnum playerEnum = PlayerEnum.NotAssigned;
    [HideInInspector] public bool canGetDamaged = true;
    [HideInInspector] public bool isDisabled = false;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public GameObject enemy;
    public float disabledTime;

    Rigidbody rb;
    Movement movement;

    int lifes = 3;

    float dmgMultiplier = 1.5f;
    float percent = 0;
    

    //////////// Collision ///////////

    [HideInInspector] public bool XNormalHitBox = false;

    //////////// AttackDeleagtes ///////////

    #region X Attack 
    public delegate void XHitNormal();
    public static XHitNormal XAttackNormalAction;
    public delegate void XHitRight();
    public static XHitRight XAttackRightAction;
    public delegate void XHitLeft();
    public static XHitLeft XAttackLeftAction;
    public delegate void XHitUp();
    public static XHitUp XAttackUpAction;
    public delegate void XHitDown();
    public static XHitDown XAttackDownAction;
    #endregion

    #region Y/B Attack

    #endregion

    //////////////////////////////////////////////////
    ////////////////      Initiate      //////////////
    //////////////////////////////////////////////////

    public void Posses()
    {
        movement = gameObject.GetComponent<Movement>();
        movement.AssigneInput();
        rb = GetComponent<Rigidbody>();

        // Input
        switch(playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.P1_XButtonDownAction += XAttackNormal;
                InputManager.P1_XButtonRightAction += XAttackRight;
                InputManager.P1_XButtonLeftAction += XAttackLeft;
                InputManager.P1_XButtonTopAction += XAttackUp;
                InputManager.P1_XButtonBottomAction += XAttackDown;
                break;
            case PlayerEnum.PlayerTwo:
                InputManager.P2_XButtonDownAction += XAttackNormal;
                InputManager.P2_XButtonRightAction += XAttackRight;
                InputManager.P2_XButtonLeftAction += XAttackLeft;
                InputManager.P2_XButtonTopAction += XAttackUp;
                InputManager.P2_XButtonBottomAction += XAttackDown;
                break;
            case PlayerEnum.NotAssigned:
            default:
                Debug.Log("Coult not Assign Input");
                break;
        }
    }

    private void OnDisable()
    {
        // Remove Input
        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.P1_XButtonDownAction -= XAttackNormal;
                InputManager.P1_XButtonRightAction -= XAttackRight;
                InputManager.P1_XButtonLeftAction -= XAttackLeft;
                InputManager.P1_XButtonTopAction -= XAttackUp;
                InputManager.P1_XButtonBottomAction -= XAttackDown;
                break;
            case PlayerEnum.PlayerTwo:
                InputManager.P2_XButtonDownAction -= XAttackNormal;
                InputManager.P2_XButtonRightAction -= XAttackRight;
                InputManager.P2_XButtonLeftAction -= XAttackLeft;
                InputManager.P2_XButtonTopAction -= XAttackUp;
                InputManager.P2_XButtonBottomAction -= XAttackDown;
                break;
            case PlayerEnum.NotAssigned:
            default:
                Debug.Log("Coult not Remove Input");
                break;
        }
    }

    void Awake()
    {

    }

    void Start()
    {

	}

    //////////////////////////////////////////////////
    ////////////////       Update       //////////////
    //////////////////////////////////////////////////

    void Update()
    {

	}

    //////////////////////////////////////////////////
    ////////////////      Functions     //////////////
    //////////////////////////////////////////////////

    //      Public      // 
    /// <summary>
    /// Stunes Character for amount of "time"
    /// </summary>
    /// <param name="time"></param>
    public void Stun(float time)
    {
        if(XNormalHitBox)
            enemy.GetComponent<MyCharacter>().Stun(time);
        movement.Stun(time);
    }

    /// <summary>
    /// Does Damaged to the Player
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        percent += damage;
    }

    /// <summary>
    /// Kick The Player away from the "EnemyPos"
    /// </summary>
    /// <param name="enemyPos"></param>
    public void KickAway(Vector3 enemyPos)
    {
        enemyPos = new Vector3(this.transform.position.x - enemyPos.x, this.transform.position.y / 10 - enemyPos.y / 10, 0);
        rb.AddForce(enemyPos.normalized * percent);
        this.isDisabled = true;
        StartCoroutine(DisabledTimeInAir(disabledTime));
    }

    //      Private      //
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

    
    
    IEnumerator DisabledTimeInAir(float disabledTime)
    {
        yield return new WaitForSeconds(disabledTime);
        this.isDisabled = false;
    }

    //////////////////////////////////////////////////
    ////////////////       Attacks      //////////////
    //////////////////////////////////////////////////

    void XAttackNormal()
    {
        if(XAttackNormalAction != null)
            XAttackNormalAction();
    }

    void XAttackRight()
    {
        if (XAttackRightAction != null)
            XAttackRightAction();
    }

    void XAttackLeft()
    {
        if (XAttackLeftAction != null)
            XAttackRightAction();
    }

    void XAttackUp()
    {
        if (XAttackUpAction != null)
            XAttackUpAction();
    }

    void XAttackDown()
    {
        if (XAttackDownAction != null)
            XAttackDownAction();
    }
}
