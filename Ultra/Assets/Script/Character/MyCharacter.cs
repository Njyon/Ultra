using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacter : MonoBehaviour
{
    [Header("Math Stuff")]
    public float potenz;
    public float basisWert;
    public float xFactor;
    public float gesamtFactor;
    public float UpDivider;
    public float startForce;
    public float startForceUp;

    [HideInInspector] public PlayerEnum playerEnum = PlayerEnum.NotAssigned;
    [HideInInspector] public bool canGetDamaged = true;
    [HideInInspector] public bool isDisabled = false;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public GameObject enemy;
    [HideInInspector] public MyCharacter enemyCharacter;
    public float disabledTime;

    [Header("Animator")]
    public Animator animator;
    int animState;

    Rigidbody rb;
    Movement movement;

    int lifes = 3;

    float dmgMultiplier = 1.5f;
    float percent = 0;

    //////////// Particles ////////////

    [Header("JumpParticle")]
    public GameObject ps_JumpOnGround;
    public GameObject ps_JumpInAir;
    public GameObject ps_Landing;

    [Header("Dash")]
    public GameObject ps_DashCloudLeft;
    public GameObject ps_DashCloudRight;

    [Header("Teleport")]
    public GameObject ps_Teleport;

    [Header("GetDamage")]
    public GameObject ps_GetDamaged;
    public ParticleSystem ps_Disabled;

    //////////// Collision ///////////

    [HideInInspector] public bool xNormalHitBox = false;
    [HideInInspector] public bool xUpHitBox = false;
    [HideInInspector] public bool xDownHitBox = false;
    [HideInInspector] public bool sUpHitBox = false;
    [HideInInspector] public bool sDownHitBox = false;

    //////////// Menu ///////////

    [HideInInspector] public bool hasSlider = false;
    [HideInInspector] public GameObject slider;

    //////////// Deleagtes ///////////

    protected delegate void EventDelegate(EventState eventState);
    protected EventDelegate eventDelegate;

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
    public delegate void SpecialNormal();
    public SpecialNormal SpecialNormalAction;
    public delegate void SpecialRight();
    public SpecialRight SpecialRightAction;
    public delegate void SpecialLeft();
    public SpecialLeft SpecialLeftAction;
    public delegate void SpecialUp();
    public SpecialUp SpecialUpAction;
    public delegate void SpecialDown();
    public SpecialDown SpecialDownAction;

    public delegate void SpecialRelease();
    public SpecialRelease SpecialReleaseAction;
    #endregion

    //////////////////////////////////////////////////
    ////////////////      Initiate      //////////////
    //////////////////////////////////////////////////

    public void Posses()
    {
        ps_Disabled.Stop();

        movement = gameObject.GetComponent<Movement>();
        movement.AssigneInput();
        rb = GetComponent<Rigidbody>();

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
                InputManager.P1_SpecalRightAction += SpecailAttackRight;
                InputManager.P1_SpecalLeftAction += SpecailAttackLeft;
                InputManager.P1_SpecalBottomAction += SpecialAttackDown;
                InputManager.P1_SpecalTopAction += SpecialAttackUp;

                movement.eventDelegate += EventCheck;
                eventDelegate += EventCheck;

                break;
            case PlayerEnum.PlayerTwo:
                InputManager.p2_OnKeyPressed += P2_InputDownCheck;
                InputManager.p2_OnKeyReleased += P2_InputUpCheck;
                InputManager.P2_XButtonRightAction += XAttackRight;
                InputManager.P2_XButtonLeftAction += XAttackLeft;
                InputManager.P2_XButtonTopAction += XAttackUp;
                InputManager.P2_XButtonBottomAction += XAttackDown;
                InputManager.P2_SpecalRightAction += SpecailAttackRight;
                InputManager.P2_SpecalLeftAction += SpecailAttackLeft;
                InputManager.P2_SpecalBottomAction += SpecialAttackDown;
                InputManager.P2_SpecalTopAction += SpecialAttackUp;

                movement.eventDelegate += EventCheck;
                eventDelegate += EventCheck;
                break;
            case PlayerEnum.NotAssigned:
            default:
                Debug.Log("Coult not Assign Input");
                break;
        }
        if (animator == null)
        {
            Debug.Log("No Animator Attached on " + gameObject.name);
        }
        else
        {
            animState = Animator.StringToHash("State");
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

                eventDelegate += EventCheck;
                break;
            case PlayerEnum.PlayerTwo:
                InputManager.p2_OnKeyPressed -= P2_InputDownCheck;
                InputManager.p2_OnKeyReleased -= P2_InputUpCheck;
                InputManager.P2_XButtonRightAction -= XAttackRight;
                InputManager.P2_XButtonLeftAction -= XAttackLeft;
                InputManager.P2_XButtonTopAction -= XAttackUp;
                InputManager.P2_XButtonBottomAction -= XAttackDown;

                eventDelegate += EventCheck;
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
        if(keyCode == KeyCode.Joystick1Button1 || keyCode == KeyCode.Joystick1Button3)
        {
            if (SpecialNormalAction != null)
                SpecialNormalAction();
        }
    }
    void P1_InputUpCheck(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick1Button1 || keyCode == KeyCode.Joystick1Button3)
        {
            if (SpecialReleaseAction != null)
                SpecialReleaseAction();
        }
    }

    void P2_InputDownCheck(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick2Button2)
        {
            if (XAttackNormalAction != null)
                XAttackNormalAction();
        }

        if (keyCode == KeyCode.Joystick2Button1 || keyCode == KeyCode.Joystick2Button3)
        {
            if (SpecialNormalAction != null)
                SpecialNormalAction();
        }
    }
    void P2_InputUpCheck(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick2Button1 || keyCode == KeyCode.Joystick2Button3)
        {
            if (SpecialReleaseAction != null)
                SpecialReleaseAction();
        }
    }
    #endregion

    void EventCheck(EventState eventState)
    {
        switch (eventState)
        {
            case EventState.Respawn:
                animator.SetInteger(animState, (int)EventState.Respawn);    // Set Animation
                break;
            case EventState.Idle:
                animator.SetInteger(animState, (int)EventState.Idle);    // Set Animation
                break;
            case EventState.Move:
                animator.SetInteger(animState, (int)EventState.Move);    // Set Animation
                break;
            case EventState.Turn:
                animator.SetInteger(animState, (int)EventState.Turn);    // Set Animation
                break;
            case EventState.OnWall:
                animator.SetInteger(animState, (int)EventState.OnWall);    // Set Animation
                break;
            case EventState.Jump:
                animator.SetInteger(animState, (int)EventState.Jump);    // Set Animation
                Instantiate(ps_JumpOnGround, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0),  Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y - 90, this.transform.rotation.z));  // Spawn Particle
                break;
            case EventState.JumpAir:
                animator.SetInteger(animState, (int)EventState.JumpAir);    // Set Animation
                Invoke("JumpAirFix", 0.1f);
                Instantiate(ps_JumpOnGround, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0), Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y - 90, this.transform.rotation.z));  // Spawn Particle
                break;
            case EventState.JumpOnWall:
                animator.SetInteger(animState, (int)EventState.JumpOnWall);    // Set Animation
                break;
            case EventState.Falling:
                animator.SetInteger(animState, (int)EventState.Falling);    // Set Animation
                break;
            case EventState.Landing:
                animator.SetInteger(animState, (int)EventState.Landing);    // Set Animation
                Instantiate(ps_Landing, new Vector3(this.transform.position.x, this.transform.position.y - 0.7f, 0), Quaternion.Euler(this.transform.rotation.x + 90, this.transform.rotation.y, this.transform.rotation.z));  // Spawn Particle
                break;
            case EventState.GetDamaged:
                animator.SetInteger(animState, (int)EventState.GetDamaged);    // Set Animation
                Instantiate(ps_GetDamaged, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.Euler(
                        this.transform.position.x - enemy.transform.position.x,
                        this.transform.position.y - enemy.transform.position.y,
                        0));  // Spawn Particle
                break;
            case EventState.Disable:
                animator.SetInteger(animState, (int)EventState.Disable);    // Set Animation
                ps_Disabled.Play();     // Activate Particle Effect
                break;
            case EventState.Dodge:
                animator.SetInteger(animState, (int)EventState.Dodge);    // Set Animation
                break;
            case EventState.Dash:
                animator.SetInteger(animState, (int)EventState.Dash);    // Set Animation
                if(IsLookingRight())
                {
                    Instantiate(ps_DashCloudRight, new Vector3(this.transform.position.x - 0.5f, this.transform.position.y - 0.5f, 0), this.transform.rotation);  // Spawn Particle
                }
                else
                {
                    Instantiate(ps_DashCloudLeft, new Vector3(this.transform.position.x - 0.5f, this.transform.position.y - 0.5f, 0), this.transform.rotation);  // Spawn Particle
                }
                break;
            case EventState.Teleport:
                animator.SetInteger(animState, (int)EventState.Teleport);    // Set Animation
                Instantiate(ps_Teleport,new Vector3(this.transform.position.x, this.transform.position.y, 0), this.transform.rotation);  // Spawn Particle
                break;
            case EventState.LightHit:
                animator.SetInteger(animState, (int)EventState.LightHit);    // Set Animation
                break;
            case EventState.LightHitSide:
                animator.SetInteger(animState, (int)EventState.LightHitSide);    // Set Animation
                break;
            case EventState.LightHitDown:
                animator.SetInteger(animState, (int)EventState.LightHitDown);    // Set Animation
                break;
            case EventState.LightHitAir:
                animator.SetInteger(animState, (int)EventState.LightHitAir);    // Set Animation
                break;
            case EventState.LightHitAirSide:
                animator.SetInteger(animState, (int)EventState.LightHitAirSide);    // Set Animation
                break;
            case EventState.LightHitAirDown:
                animator.SetInteger(animState, (int)EventState.LightHitAirDown);    // Set Animation
                break;
            case EventState.Charge:
                animator.SetInteger(animState, (int)EventState.Charge);    // Set Animation
                break;
            case EventState.ChargeSide:
                animator.SetInteger(animState, (int)EventState.ChargeSide);    // Set Animation
                break;
            case EventState.ChargeDown:
                animator.SetInteger(animState, (int)EventState.ChargeDown);    // Set Animation
                break;
            case EventState.SpecialUperCut:
                animator.SetInteger(animState, (int)EventState.SpecialUperCut);    // Set Animation
                break;
            case EventState.Special:
                animator.SetInteger(animState, (int)EventState.Special);    // Set Animation
                break;
            case EventState.SpecialSideFirstHit:
                animator.SetInteger(animState, (int)EventState.SpecialSideFirstHit);    // Set Animation
                break;
            case EventState.SpecialSide:
                animator.SetInteger(animState, (int)EventState.SpecialSide);    // Set Animation
                break;
            case EventState.SpecialDown:
                animator.SetInteger(animState, (int)EventState.SpecialDown);    // Set Animation
                break;
            case EventState.SpecialAir:
                animator.SetInteger(animState, (int)EventState.SpecialAir);    // Set Animation
                break;
            case EventState.SpecialAirDown:
                animator.SetInteger(animState, (int)EventState.SpecialAirDown);    // Set Animation
                break;
            default:
                Debug.Log("Coundnt Find State! Character: " + gameObject.name);
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
    /// Starts the Stun effect this Character
    /// </summary>
    public void Stun()
    {
        movement.Stun();
    }
    /// <summary>
    /// End the Stun from the Player and Enemy if "isStunned"
    /// </summary>
    public void EndStun(MyCharacter character)
    {
        character.movement.EndStun();
    }
    /// <summary>
    /// End the Stun effect on this Character
    /// </summary>
    public void EndStun()
    {
        movement.EndStun();
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
        Debug.Log(percent);

        float hight = 300;
        if(MyEpsilon.Epsilon(this.transform.position.y, enemyPos.y, 1f))                                                                // Is Hight is Near
        {
            enemyPos = new Vector3(this.transform.position.x - enemyPos.x, 5, 0);
        }
        else                                                                                                                            // Position in Y is High enough
        {
            enemyPos = new Vector3(this.transform.position.x - enemyPos.x, this.transform.position.y - enemyPos.y, 0);
        }

        if (enemyPos.x < 0)                             // Direction = right
        {
            if (hard)
            {
                rb.AddForce(new Vector3(-Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) * xFactor, hight, 0) * gesamtFactor);
                Debug.Log("LOL");
            }
            else
            {
                rb.AddForce(new Vector3(-Mathf.Pow(Mathf.Sqrt(basisWert * percent) + startForce, potenz), hight, 0));
            }
        }
        else                                            // Direction = Left
        {
            if (hard)
            {
                rb.AddForce(new Vector3(Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) * xFactor, hight, 0) * gesamtFactor);
                Debug.Log("LOL");
            }
            else
            {
                rb.AddForce(new Vector3(Mathf.Pow(Mathf.Sqrt(basisWert * percent) + startForce, potenz), hight, 0));
            }
        }
        Disable(character, disabledTime);
    }
    /// <summary>
    /// Kicks Player Up in the Air
    /// </summary>
    /// <param name="character"></param>
    /// <param name="enemyPos"></param>
    /// <param name="hard"></param>
    public void KickUp(MyCharacter character, Vector3 enemyPos, bool hard)
    {
        Debug.Log (Mathf.Pow(Mathf.Sqrt(30 * percent), potenz));
        float direction = 300;

        enemyPos = new Vector3(this.transform.position.x - enemyPos.x, this.transform.position.y - enemyPos.y, 0);

        if (enemyPos.x < 0)                         // Direction = Right
        {
            if (hard)
            {
                rb.AddForce(new Vector3(-direction, Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) * xFactor, 0) * gesamtFactor);
            }
            else
            {
                rb.AddForce(new Vector3(-direction, Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) / UpDivider + startForceUp, 0));
            }
        }
        else                                    // Direction = Left
        {
            if (hard)
            {
                rb.AddForce(new Vector3(direction, Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) * xFactor, 0) * gesamtFactor);
            }
            else
            {
                rb.AddForce(new Vector3(direction, Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) / UpDivider + startForceUp, 0));
            }
        }
        Disable(character, disabledTime);
    }
    /// <summary>
    /// Kick the character down to the ground
    /// </summary>
    /// <param name="character"></param>
    /// <param name="enemyPos"></param>
    /// <param name="hard"></param>
    public void KickDown(MyCharacter character, Vector3 enemyPos, bool hard)
    {
        Debug.Log(Mathf.Sqrt(enemyPos.normalized.x * 10 * percent));
        float direction = 300;
        enemyPos = new Vector3(this.transform.position.x - enemyPos.x, this.transform.position.y - enemyPos.y, 0);

        if (enemyPos.x < 0)                 // Direction = Right
        {
            if (hard)
            {
                rb.AddForce(new Vector3(-direction, -Mathf.Pow(Mathf.Sqrt(30 * percent), potenz) * xFactor, 0) * gesamtFactor);
            }
            else
            {
                rb.AddForce(new Vector3(-direction, -Mathf.Pow(Mathf.Sqrt(30 * percent), potenz), 0));
            }
        }
        else                                // Direction = Left
        {
            if (hard)
            {
                rb.AddForce(new Vector3(direction, -Mathf.Pow(Mathf.Sqrt(30 * percent), potenz) * xFactor, 0) * gesamtFactor);
            }
            else
            {
                rb.AddForce(new Vector3(direction, -Mathf.Pow(Mathf.Sqrt(30 * percent), potenz), 0));
            }
        }
        Disable(character, disabledTime);
    }
    /// <summary>
    /// returns if the Character is falling or not
    /// </summary>
    /// <returns></returns>
    public bool IsFalling()
    {
        return movement.isFalling;
    }
    /// <summary>
    /// Returns Look direction (True == Right | False == Left)
    /// </summary>
    /// <returns></returns>
    public bool IsLookingRight()
    {
        return movement.islookingToTheRight;
    }
    /// <summary>
    /// Lets the Character Look to the Right emiditly
    /// </summary>
    public void LookRight()
    {
        movement.LookRightNow();
    }
    /// <summary>
    /// Lets the Character Look to the Left emiditly
    /// </summary>
    public void LookLeft()
    {
        movement.LookLeftNow();
    }
    /// <summary>
    /// Let the Character Jump | WARNING: Needed for a SpecialAttack DONT USE ENYWEHERE ELSE!
    /// </summary>
    public void SpecialJump()
    {
        movement.SpecialJump();
    }
    /// <summary>
    /// Let the Character Jump of the amount of param jumpforce| WARNING: Needed for a SpecialAttack DONT USE ENYWEHERE ELSE!
    /// </summary>
    /// <param name="jumpForce"></param>
    public void SpecialJump(float jumpForce)
    {
        movement.SpecialJump(jumpForce);
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
    
    void SpecailAttackRight()
    {
        if (SpecialRightAction != null)
            SpecialRightAction();
    }
    void SpecailAttackLeft()
    {
        if (SpecialLeftAction != null)
            SpecialLeftAction();
    }
    void SpecialAttackDown()
    {
        if (SpecialDownAction != null)
            SpecialDownAction();
    }
    void SpecialAttackUp()
    {
        if (SpecialUpAction != null)
            SpecialUpAction();
    }

    void JumpAirFix()
    {
        animator.SetInteger(animState, (int)EventState.Falling);    // Set Animation
    }
}
