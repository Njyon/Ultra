using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacter : MonoBehaviour
{
    [Header("JumpParticle")]
    public GameObject p_JumpOnGround;

    [HideInInspector] public PlayerEnum playerEnum = PlayerEnum.NotAssigned;
    [HideInInspector] public bool canGetDamaged = true;
    [HideInInspector] public bool isDisabled = false;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public GameObject enemy;
    [HideInInspector] public MyCharacter enemyCharacter;
    public float disabledTime;

    Rigidbody rb;
    Movement movement;

    int lifes = 3;

    float dmgMultiplier = 1.5f;
    float percent = 0;
    

    //////////// Collision ///////////

    [HideInInspector] public bool xNormalHitBox = false;

    //////////// Menu ///////////

    [HideInInspector] public bool hasSlider = false;
    [HideInInspector] public GameObject slider;

    //////////// AttackDeleagtes ///////////

    #region X Attack 
    public delegate void XHitNormal();
    public  XHitNormal XAttackNormalAction;
    public delegate void XHitRight();
    public  XHitRight XAttackRightAction;
    public delegate void XHitLeft();
    public  XHitLeft XAttackLeftAction;
    public delegate void XHitUp();
    public  XHitUp XAttackUpAction;
    public delegate void XHitDown();
    public  XHitDown XAttackDownAction;
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

        // Movement Delegates Sub
        movement.JumpDelegateAction += JumpCheck;

        // Input
        switch(playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.p1_OnKeyPressed += P1_InputDownCheck;
                InputManager.p1_OnKeyReleased += P1_InputUpCheck;
                InputManager.P1_XButtonRightAction += XAttackRight;
                InputManager.P1_XButtonLeftAction += XAttackLeft;
                InputManager.P1_XButtonTopAction += XAttackUp;
                InputManager.P1_XButtonBottomAction += XAttackDown;
                break;
            case PlayerEnum.PlayerTwo:
                InputManager.p2_OnKeyPressed += P2_InputDownCheck;
                InputManager.p2_OnKeyReleased += P2_InputUpCheck;
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
                InputManager.p1_OnKeyPressed -= P1_InputDownCheck;
                InputManager.p1_OnKeyReleased -= P1_InputUpCheck;
                InputManager.P1_XButtonRightAction -= XAttackRight;
                InputManager.P1_XButtonLeftAction -= XAttackLeft;
                InputManager.P1_XButtonTopAction -= XAttackUp;
                InputManager.P1_XButtonBottomAction -= XAttackDown;
                break;
            case PlayerEnum.PlayerTwo:
                InputManager.p2_OnKeyPressed -= P2_InputDownCheck;
                InputManager.p2_OnKeyReleased -= P2_InputUpCheck;
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

    #region InputCheck
    void P1_InputDownCheck(KeyCode keyCode)
    {
        if(keyCode == KeyCode.Joystick1Button2)
        {
            if (XAttackNormalAction != null)
                XAttackNormalAction();
        }
    }
    void P1_InputUpCheck(KeyCode keyCode)
    {

    }

    void P2_InputDownCheck(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick2Button2)
        {
            if (XAttackNormalAction != null)
                XAttackNormalAction();
        }
    }
    void P2_InputUpCheck(KeyCode keyCode)
    {

    }
    #endregion

    #region Delegate Check
    void JumpCheck(JumpState jumpState)
    {
        switch (jumpState)
        {
            case JumpState.InAir:

                break;
            case JumpState.OnGround:
                Instantiate(p_JumpOnGround, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0), this.transform.rotation);
                break;
            case JumpState.OnWallLeft:

                break;
            case JumpState.OnWallRight:

                break;
        }
    }

    #endregion

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
    /// Disables a specific Character for amount of param time
    /// </summary>
    /// <param name="character"></param>
    /// <param name="time"></param>
    public void Disable(MyCharacter character, float time)
    {
        character.movement.CantMove(time);
        character.isDisabled = true;
        Invoke("EndDisable", time);
    }
    /// <summary>
    /// Disable a Specific Character
    /// </summary>
    /// <param name="character"></param>
    public void Disable(MyCharacter character)
    {
        character.movement.CantMove();
        character.isDisabled = true;
    }
    /// <summary>
    /// Disables the own Character
    /// </summary>
    public void Disable()
    {
        movement.CantMove();
        isDisabled = true;
    }
    /// <summary>
    /// Disables the own character for the amount of param time
    /// </summary>
    /// <param name="time"></param>
    public void Disable(float time)
    {
        movement.CantMove();
        isDisabled = true;
        Invoke("EndDisable", time);
    }
    /// <summary>
    /// Ends the disable effect a specific character
    /// </summary>
    /// <param name="character"></param>
    public void EndDisable(MyCharacter character)
    {
        character.movement.CanMove();
        character.isDisabled = false;
    }
    /// <summary>
    /// Ends the Disable Effect on the own Character
    /// </summary>
    public void EndDisable()
    {
        movement.CanMove();
        isDisabled = false;
    }
    /// <summary>
    /// Stunes enemy for amount of "time"
    /// </summary>
    /// <param name="time"></param>
    public void Stun(MyCharacter character, float time)
    {
        character.movement.Stun(time);
    }
    /// <summary>
    /// Stunes Character till EndStun() & Stunes Enemy if in reach
    /// </summary>
    public void Stun(MyCharacter character)
    {
        character.movement.Stun();
    }
    /// <summary>
    /// End the Stun from the Player and Enemy if "isStunned"
    /// </summary>
    public void EndStun(MyCharacter character)
    {
        character.movement.EndStun();
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
    /// Kick The Player away from the Vector param
    /// Kick power depence on LifePercent
    /// </summary>
    /// <param name="enemyPos"></param>
    public void KickAway(MyCharacter character, Vector3 enemyPos, bool hard)
    {
        if(MyEpsilon.Epsilon(this.transform.position.y, enemyPos.y, 1f))                                                                // Is Hight is Near
        {
            enemyPos = new Vector3(this.transform.position.x - enemyPos.x, 5, 0);
        }
        else                                                                                                                            // Position in Y is High enough
        {
            enemyPos = new Vector3(this.transform.position.x - enemyPos.x, this.transform.position.y / 10 - enemyPos.y / 10, 0);
        }

        if (hard)
        {
            if (percent <= 30)
            {
                rb.AddForce(enemyPos.normalized * 300);
            }
            else if (percent <= 90)
            {
                rb.AddForce(enemyPos.normalized * 600);
            }
            else if (percent <= 140)
            {
                rb.AddForce(enemyPos.normalized * 1000);
            }
        }
        else
        {
            if (percent <= 30)
            {
                rb.AddForce(enemyPos.normalized * 500);
            }
            else if (percent <= 90)
            {
                rb.AddForce(enemyPos.normalized * 900);
            }
            else if (percent <= 140)
            {
                rb.AddForce(enemyPos.normalized * 1500);
            }
        }

        
        Disable(enemyCharacter, disabledTime);
    }
    /// <summary>
    /// returns if the Character is falling or not
    /// </summary>
    /// <returns></returns>
    public bool IsFalling()
    {
        return movement.isFalling;
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

    //////////////////////////////////////////////////
    ////////////////       Attacks      //////////////
    //////////////////////////////////////////////////
    
    void XAttackRight()
    {
        if (XAttackRightAction != null)
            XAttackRightAction();
    }

    void XAttackLeft()
    {
        if (XAttackLeftAction != null)
            XAttackLeftAction();
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
