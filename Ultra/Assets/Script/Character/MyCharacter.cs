using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacter : MonoBehaviour
{
    [Header("Kick Away Vars")]
    public float potenz;
    public float basisWert;
    public float xFactor;
    public float gesamtFactor;
    public float UpDivider;
    public float startForce;
    public float startForceUp;
    public float X;
    public float freezTimeBounce;
    public float freezTimeHit;

    [Header("Renderer")]
    public Renderer bodyRenderer;
    public Renderer clothRenderer;
    
    [HideInInspector] public PlayerEnum playerEnum = PlayerEnum.NotAssigned;
    [HideInInspector] public bool canGetDamaged = true;
    [HideInInspector] public bool isDisabled = false;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public GameObject enemy;
    [HideInInspector] public MyCharacter enemyCharacter;
    [SerializeField] CapsuleCollider col;
    public float disabledTime;

    [Header("Animator")]
    public Animator animator;
    int animIsFalling;
    int animDisabled;
    protected int animIsHiting;
    int animGroundBlend;
    int animAirBlend;
    int animAttackBlend;
    int animJumpBlend;
    int animDodge;

    [HideInInspector] public bool isDodgeing = false;
    [HideInInspector] public bool isAttacking = false;
    protected Rigidbody rb;
    protected Movement movement;
    
    int percent = 0;
    [Header("Dash")]
    [SerializeField] protected float dashCoolDown;
    [SerializeField] protected int maxDashes;
    protected int currentDashes = 0;

    [Header("Stuff")]
    [SerializeField] GameObject meshController;
    Vector3 spawnPos;
    protected InGameUI ui;

    //Material change Vars
    int emissionID;
    int colorID;
    Color bodyColor;
    [HideInInspector] public Color clothColor;
    [HideInInspector] public Color swordColor;
    
    [Header("PhysikMaterial")]
    [SerializeField] PhysicMaterial mat_Metal;
    [SerializeField] PhysicMaterial mat_Concrete;
    [SerializeField] PhysicMaterial mat_Stone;
    [SerializeField] PhysicMaterial mat_Rubber;
    [SerializeField] PhysicMaterial mat_Glass;

    [Header("AudioComp")]
    [SerializeField] AudioEvents audioEvents;

    protected GameObject lastBounceObj;

    [SerializeField] protected ParticleData pD;

    [HideInInspector] public int hitCounter = 1;
    [HideInInspector] public int combo = 0;
    [HideInInspector] public int multiplier = 1;
    [HideInInspector] public int score = 0;
    [HideInInspector] public bool inCombo = false;

    [HideInInspector] public OneVsOneGameMode gameMode;
    [HideInInspector] public bool inComeBackMode = false;

    //////////// Collision ///////////

    [HideInInspector] public bool xNormalHitBox = false;
    [HideInInspector] public bool xUpHitBox = false;
    [HideInInspector] public bool xDownHitBox = false;
    [HideInInspector] public bool xUpHitAngeldBox = false;
    [HideInInspector] public bool xDownHitAngeldBox = false;
    [HideInInspector] public bool sUpHitBox = false;
    [HideInInspector] public bool sDownHitBox = false;

    //////////// Deleagtes ///////////

    public delegate void EventDelegate(EventState eventState);
    public EventDelegate eventDelegate;

    public delegate void EndGame();
    public static EndGame endGameAction;

    public delegate void FreezCam();
    public FreezCam freezCamAction;

    public delegate void PlayerDataDelegate(PlayerEnum pE, int combo, int multiplier, int score);
    public PlayerDataDelegate playerDataAction;

    public delegate void DodgeDelegate(PlayerEnum pE);
    public DodgeDelegate dodgeAction;

    public delegate void BounceDelegate(PlayerEnum pE);
    public BounceDelegate bounceAction;

    public delegate void ShakeCamera(bool isHeavy);
    public ShakeCamera shakeCameraAction;

    public delegate void ParryDelegate();
    public ParryDelegate parryDelegate;
    
    // Deprecated
    #region X Attack 
    protected delegate void XHitNormal();
    protected XHitNormal XAttackNormalAction;
    protected delegate void XHitRight();
    protected XHitRight XAttackRightAction;
    protected delegate void XHitLeft();
    protected XHitLeft XAttackLeftAction;
    protected delegate void XHitUp();
    protected XHitUp XAttackUpAction;
    protected delegate void XHitDown();
    protected XHitDown XAttackDownAction;
    #endregion
    // Deprecated
    #region Y/B Attack
    protected delegate void SpecialNormal();
    protected SpecialNormal SpecialNormalAction;
    protected delegate void SpecialRight();
    protected SpecialRight SpecialRightAction;
    protected delegate void SpecialLeft();
    protected SpecialLeft SpecialLeftAction;
    protected delegate void SpecialUp();
    protected SpecialUp SpecialUpAction;
    protected delegate void SpecialDown();
    protected SpecialDown SpecialDownAction;

    protected delegate void SpecialRelease();
    protected SpecialRelease SpecialReleaseAction;
    #endregion

    //////////////////////////////////////////////////
    ////////////////      Initiate      //////////////
    //////////////////////////////////////////////////

    public void Posses()
    {
        pD.ps_Disabled.Stop();

        movement = gameObject.GetComponent<Movement>();
        movement.AssigneInput();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;

        // Input
        switch(playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.p1_OnKeyPressed += InputDownCheck;
                InputManager.p1_OnKeyReleased += InputUpCheck;
                InputManager.P1_SpecalRightAction += SpecailAttackRight;
                InputManager.P1_SpecalLeftAction += SpecailAttackLeft;
                InputManager.P1_SpecalBottomAction += SpecialAttackDown;
                InputManager.P1_SpecalTopAction += SpecialAttackUp;

                movement.eventDelegate += EventCheck;
                eventDelegate += EventCheck;

                break;
            case PlayerEnum.PlayerTwo:
                InputManager.p2_OnKeyPressed += InputDownCheck;
                InputManager.p2_OnKeyReleased += InputUpCheck;
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
        Check();
    }
    public void DePosses()
    {
        // Remove Input
        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.p1_OnKeyPressed -= InputDownCheck;
                InputManager.p1_OnKeyReleased -= InputUpCheck;
                InputManager.P1_SpecalRightAction -= SpecailAttackRight;
                InputManager.P1_SpecalLeftAction -= SpecailAttackLeft;
                InputManager.P1_SpecalBottomAction -= SpecialAttackDown;
                InputManager.P1_SpecalTopAction -= SpecialAttackUp;

                movement.eventDelegate -= EventCheck;
                eventDelegate -= EventCheck;

                movement.RemoveInput();
                break;
            case PlayerEnum.PlayerTwo:
                InputManager.p2_OnKeyPressed -= InputDownCheck;
                InputManager.p2_OnKeyReleased -= InputUpCheck;
                InputManager.P2_SpecalRightAction -= SpecailAttackRight;
                InputManager.P2_SpecalLeftAction -= SpecailAttackLeft;
                InputManager.P2_SpecalBottomAction -= SpecialAttackDown;
                InputManager.P2_SpecalTopAction -= SpecialAttackUp;

                movement.eventDelegate -= EventCheck;
                eventDelegate -= EventCheck;

                movement.RemoveInput();
                break;
            case PlayerEnum.NotAssigned:
            default:
                Debug.Log("Coult not Assign Input");
                break;
        }
        Destroy(this);
    }
    
    void Check()
    {
        if (animator == null)
        {
            Debug.Log("No Animator Attached on " + gameObject.name);
        }
        else
        {
            animIsFalling = Animator.StringToHash("isFalling");
            animDisabled = Animator.StringToHash("isDisabled");
            animIsHiting = Animator.StringToHash("isHiting");
            animGroundBlend = Animator.StringToHash("GroundBlend");
            animAirBlend = Animator.StringToHash("AirBlend");
            animAttackBlend = Animator.StringToHash("DashBlend");
            animJumpBlend = Animator.StringToHash("JumpBlend");
            animDodge = Animator.StringToHash("isDodgeing");
        }
        if (ui == null)
        {
            Debug.Log("No UI Element Found");
        }
        else
        {
            UIStart();
        }

        emissionID = Shader.PropertyToID("_EmissionColor");
        colorID = Shader.PropertyToID("_Color");
        bodyColor = bodyRenderer.material.color;
        clothColor = clothRenderer.materials[0].color;
        swordColor = clothRenderer.materials[1].color;


        spawnPos = transform.position;
        pD.ps_Disabled.Stop();
    }

    #region InputCheck#
    /// <summary>
    /// Deprecated
    /// </summary>
    /// <param name="keyCode"></param>
    void InputDownCheck(KeyCode keyCode)
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
    /// <summary>
    /// Deprecated
    /// </summary>
    /// <param name="keyCode"></param>
    void InputUpCheck(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick1Button1 || keyCode == KeyCode.Joystick1Button3)
        {
            if (SpecialReleaseAction != null)
                SpecialReleaseAction();
        }
    }
    #endregion

    public void EventCheck(EventState eventState)
    {
        switch (eventState)
        {
            case EventState.Idle:
                animator.SetFloat(animGroundBlend, (float)GroundState.Idle);
                animator.SetBool(animIsFalling, false);

                break;
            case EventState.Walking:
                animator.SetFloat(animGroundBlend, (float)GroundState.Walking);
                break;
            case EventState.OnWall:
                animator.SetFloat(animAirBlend, (float)FallState.OnWall);
                break;
            case EventState.JumpOnGround:
                animator.SetFloat(animAirBlend, (float)FallState.Jump);
                animator.SetFloat(animJumpBlend, (float)JumpState.JumpOnGround);
                Instantiate(pD.ps_JumpOnGround, new Vector3(this.transform.position.x, this.transform.position.y + 1f, 0),  Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y - 90, this.transform.rotation.z));  // Spawn Particle
                break;
            case EventState.JumpInAir:
                animator.SetFloat(animAirBlend, (float)FallState.Jump);
                animator.SetFloat(animJumpBlend, (float)JumpState.JumpInAir);
                Instantiate(pD.ps_JumpInAir, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0), Quaternion.identity);  // Spawn Particle
                break;
            case EventState.JumpInAir2:
                animator.SetFloat(animAirBlend, (float)FallState.Jump);
                animator.SetFloat(animJumpBlend, (float)JumpState.JumpInAirTwo);
                Instantiate(pD.ps_JumpInAir2, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0), Quaternion.identity);  // Spawn Particle
                break;
            case EventState.WallJump:
                animator.SetFloat(animAirBlend, (float)FallState.Jump);
                animator.SetFloat(animJumpBlend, (float)JumpState.JumpOnGround);
                switch (IsLookingRight())
                {
                    case true:
                        Instantiate(pD.ps_OnWall_Right, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y - 90, this.transform.rotation.z));  // Spawn Particle
                        break;
                    case false:
                        Instantiate(pD.ps_OnWall_Left, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y - 90, this.transform.rotation.z));  // Spawn Particle
                        break;
                }
                break;
            case EventState.Fall:
                if (isAttacking)
                    return;

                animator.SetFloat(animAirBlend, (float)FallState.Fall);
                break;
            case EventState.Landing:
                animator.SetBool(animIsHiting, false);
                animator.SetBool(animIsFalling, false);
                animator.SetFloat(animGroundBlend, (float)GroundState.Landing);
                animator.SetFloat(animAirBlend, (float)FallState.Fall);
                Instantiate(pD.ps_Landing, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.Euler(this.transform.rotation.x + 90, this.transform.rotation.y, this.transform.rotation.z));  // Spawn Particle
                break;
            case EventState.Dodge:
                animator.SetBool(animDodge, true);
                isDodgeing = true;
                break;
            case EventState.DodgeEnd:
                animator.SetBool(animDodge, false);
                isDodgeing = false;
                break;
            case EventState.GetHit:
                Instantiate(pD.ps_GetHit, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);  // Spawn Particle
                Instantiate(pD.ps_GetDamaged, transform.position, Quaternion.identity);  // Spawn Particle
                break;
            case EventState.isDisabled:
                animator.SetBool(animDisabled, true);
                pD.ps_Disabled.Play();     // Activate Particle Effect
                break;
            case EventState.isFalling:
                animator.SetBool(animIsFalling, true);
                break;
            case EventState.EndDisabled:
                animator.SetBool(animDisabled, false);
                break;
            case EventState.ChangeDirectionLeft:
                Instantiate(pD.turnAroundLeft, new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                break;
            case EventState.ChangeDirectionRight:
                Instantiate(pD.turnAroundRight, new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z), Quaternion.identity);
                break;
            case EventState.AttackUp:
                animator.SetFloat(animAirBlend, (float)FallState.Attack);
                animator.SetFloat(animAttackBlend, (float)AttackState.AttackUp);
                break;
            case EventState.AttackUpAngled:
                animator.SetFloat(animAirBlend, (float)FallState.Attack);
                animator.SetFloat(animAttackBlend, (float)AttackState.AttackAngledUp);
                break;
            case EventState.AttackSide:
                animator.SetFloat(animAirBlend, (float)FallState.Attack);
                animator.SetFloat(animAttackBlend, (float)AttackState.AttackSide);
                break;
            case EventState.AttackDownAngled:
                animator.SetFloat(animAirBlend, (float)FallState.Attack);
                animator.SetFloat(animAttackBlend, (float)AttackState.AttackAngledDown);
                break;
            case EventState.AttackDown:
                animator.SetFloat(animAirBlend, (float)FallState.Attack);
                animator.SetFloat(animAttackBlend, (float)AttackState.AttackDown);
                break;
            case EventState.AttackEnd:
                if(IsFalling())
                {
                    animator.SetBool(animIsFalling, true);
                    animator.SetFloat(animAirBlend, (float)FallState.Fall);
                }
                else
                {
                    animator.SetBool(animIsFalling, false);
                    animator.SetFloat(animGroundBlend, (float)GroundState.Idle);
                }
                break;
            case EventState.AttackHit:
                animator.SetBool(animIsHiting, true);
                break;
            case EventState.ResetDashes:
                currentDashes = 0;
                break;
            case EventState.Bounce:

                break;
            default:
                Debug.Log("Coundnt Find State! " + "Event: " + eventState + " Character: " + gameObject.name);
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

    public void PartilceSlash()
    {
        if(IsLookingRight())
        {
            Instantiate(pD.ps_Slash, new Vector3(transform.position.x + 0.5f, transform.position.y + 0, 0), Quaternion.Euler(0, 180, 60), transform);
        }
        else
        {
            Instantiate(pD.ps_Slash, new Vector3(transform.position.x + 0.5f, transform.position.y + 0, 0), Quaternion.Euler(180, 0, 60), transform);
        }
    }

    public void SpecialBounce(float time)
    {
        if (isDisabled)
            return;

        // Disable movement
        movement.CantMove();
        // Set the disable state to true
        //isDisabled = true;
        // Invoke the EndDisable() after param:"time"
        Invoke("EndDisable", time);
    }
    /// <summary>
    /// Lets the Character not be able to move
    /// </summary>
    protected void IsAttacking()
    {
        movement.CantMove();
        isAttacking = true;
    }
    /// <summary>
    /// Lets the Character be able to move again
    /// </summary>
    protected void EndAttacking()
    {
        movement.CanMoveTrue();
        isAttacking = false;
    }
    /// <summary>
    /// Disables the own Character
    /// </summary>
    public void Disable()
    {

        // Scale Hitbox up
        col.radius = 1;

        // SHow the Combo Counter
        if(!ui.GetComboState())
        {
            enemyCharacter.ui.ShowCombo();
            enemyCharacter.ui.UpdateMultiplier(1);
        }
        // Disable movement for the Character
        movement.CantMove();
        // Set the disable state to true
        isDisabled = true;
        // Play the Disables Particle Effect
        pD.ps_Disabled.Play();
    }
    /// <summary>
    /// Disables the own character for the amount of param time
    /// </summary>
    /// <param name="time"></param>
    public void Disable(float time)
    {
        // Cancel all Invokes if player gets disabled again and restart the time below
        if (IsInvoking())
            CancelInvoke();

        // Scale Hitbox up
        col.radius = 1;

        // SHow the Combo Counter
        if (!ui.GetComboState())
        {
            enemyCharacter.ui.ShowCombo();
            enemyCharacter.ui.UpdateMultiplier(1);
        }
        // Disable movement
        movement.CantMove();
        // Set the disable state to true
        isDisabled = true;
        // Invoke the EndDisable() after param:"time"
        Invoke("EndDisable", time);
        // Start the Disabled Particle Effect
        pD.ps_Disabled.Play();
        // Set Animations, sound etc
        EventCheck(EventState.isDisabled);
        RotateToFlyDirection();

        StartCoroutine(Blink());
        StartCoroutine(FreezCharacter(true, freezTimeHit, false));
    }
    /// <summary>
    /// Ends the Disable Effect on the own Character
    /// </summary>
    public void EndDisable()
    {
        // Scale Hitbox back to normal
        col.radius = 0.5f;
        // End the Enemy Combo
        enemyCharacter.EndCombo();
        // Remove the last Bounce Object
        lastBounceObj = null;
        // get the Player the Control of the Player back
        movement.CanMoveTrue();
        // Set the Disable State to false
        isDisabled = false;
        // Stop the Disable Particle Effect
        pD.ps_Disabled.Stop();
        // Set Animations, sound etc
        EventCheck(EventState.EndDisabled);
        // Reset Rotation and Position
        meshController.transform.localPosition = Vector3.down;
        meshController.transform.localRotation = Quaternion.identity;
        
    }
    /// <summary>
    /// Starts the Stun effect this Character
    /// </summary>
    public void Stun()
    {
        movement.Stun();
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
    public void KickAway(Vector3 enemyPos, bool hard)
    {

        //float hight = 300;
        //if (MyEpsilon.Epsilon(this.transform.position.y, enemyPos.y, 1f))                                                                // Is Hight is Near
        //{
        //    enemyPos = new Vector3(this.transform.position.x - enemyPos.x, 5, 0);
        //}

        Vector3 dir = new Vector3(this.transform.position.x - enemyPos.x, this.transform.position.y - enemyPos.y, 0);
      
        //Instantiate(pD.ps_GetDamaged, transform.position, Quaternion.identity);
        eventDelegate(EventState.GetHit);

        if (dir.x < 0)                             // Direction = right
        {
            // Deprecated
            if (hard)
            {
                rb.AddForce(new Vector3(-Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) * xFactor, enemyPos.y, 0) * gesamtFactor);
            }
            else
            {
                rb.velocity = new Vector3(-Mathf.Pow(Mathf.Sqrt(basisWert * percent) + startForce, potenz), dir.y, 0);
            }
        }
        else                                            // Direction = Left
        {
            // Deprecated
            if (hard)
            {
                rb.AddForce(new Vector3(Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) * xFactor, enemyPos.y, 0) * gesamtFactor);
            }
            else
            {
                rb.velocity = new Vector3(Mathf.Pow(Mathf.Sqrt(basisWert * percent) + startForce, potenz), dir.y, 0);
            }
        }
        if (shakeCameraAction != null)
            shakeCameraAction(true);

        float time = X * Mathf.Sqrt(percent);
        Disable(time);
    }
    /// <summary>
    /// Kicks Player Up in the Air
    /// </summary>
    /// <param name="character"></param>
    /// <param name="enemyPos"></param>
    /// <param name="hard"></param>
    public void KickUp(Vector3 enemyPos, bool hard)
    {

        Vector3 dir = new Vector3(this.transform.position.x - enemyPos.x, this.transform.position.y - enemyPos.y, 0);

        //Instantiate(pD.ps_GetDamaged, transform.position, Quaternion.identity);
        eventDelegate(EventState.GetHit);
        if (enemyPos.x < 0)                         // Direction = Right
        {
            if (hard)
            {
                rb.AddForce(new Vector3(-enemyPos.x, Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) * xFactor, 0) * gesamtFactor);
            }
            else
            {
                rb.velocity = new Vector3(-enemyPos.x, Mathf.Pow(Mathf.Sqrt(basisWert * percent) + startForce, potenz), 0);
            }
        }
        else                                    // Direction = Left
        {
            if (hard)
            {
                rb.AddForce(new Vector3(enemyPos.x, Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) * xFactor, 0) * gesamtFactor);
            }
            else
            {
                rb.velocity = new Vector3(enemyPos.x, Mathf.Pow(Mathf.Sqrt(basisWert * percent) + startForce, potenz), 0);
            }
        }
        if (shakeCameraAction != null)
            shakeCameraAction(true);

        float time = X * Mathf.Sqrt(percent);
        Disable(time);
    }
    /// <summary>
    /// Kick the character down to the ground
    /// </summary>
    /// <param name="character"></param>
    /// <param name="enemyPos"></param>
    /// <param name="hard"></param>
    public void KickDown(Vector3 enemyPos, bool hard)
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
        enemyCharacter.Disable(disabledTime);
    }
    /// <summary>
    /// returns if the Character is falling or not
    /// </summary>
    /// <returns></returns>
    public bool IsFalling()
    {
        return movement.IsFalling();
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
    /// <summary>
    /// Sets the UI with the param
    /// </summary>
    /// <param name="ui"></param>
    public void SetUI(InGameUI ui)
    {
        this.ui = ui;
    }
    /// <summary>
    /// Change Combo to new comboState
    /// </summary>
    /// <param name="comboState"></param>
    public void Combo(ComboState comboState)
    {
        inCombo = true;
        switch(comboState)
        {
            case ComboState.Bounce:
                // Count Combo Up
                combo++;
                // Update Combo
                ui.UpdateCombo(combo);
                break;

            case ComboState.BounceSpecial:
                // Count Combo Up
                combo++;
                // Multiplie the Multiplier
                multiplier *= 5;
                // Update Combo
                ui.UpdateCombo(combo);
                // Update Multiplier
                ui.UpdateMultiplier(multiplier);
                break;

            case ComboState.Dodge:
                // Add Dodge Points to Score
                score += 1000;
                // Update Score
                ui.UpdateScore(score);
                break;

            case ComboState.Hit:
                // Count Combo Up
                combo++;
                // Count HitCounter Up
                hitCounter++;
                // Update Combo
                ui.UpdateCombo(combo);
                break;

            case ComboState.HitStrak:
                // Count Combo Up
                combo++;
                // Count HitCounter Up
                hitCounter++;
                // Count Multiplier Up
                multiplier++;
                // Update Combo
                ui.UpdateCombo(combo);
                // Update Combo Multiplier
                ui.UpdateMultiplier(multiplier);
                break;
        }
    }
    /// <summary>
    /// Ends the Combo and add the Points to the Score
    /// </summary>
    public void EndCombo()
    {
        inCombo = false;

        // Calc new Score
        float interimResult = Mathf.Pow(combo, 2) * hitCounter;
        interimResult += (int)interimResult * multiplier;

        if(inComeBackMode)
        {
            score += (int)interimResult * 2;
            CheckIfComebackModeShouldEnd();
        }
        else
        {
            score += (int)interimResult;
        }   
                    
        //Safe Data for EndScreen
        playerDataAction(playerEnum, combo, multiplier, score);

        // Update the new Score
        ui.UpdateScore(score);

        // Reset the Combo vars back to Start
        ResetComboValues();

        // Hidde Combo Counter
        ui.HiddeCombo();
    }
    public void CheckIfComebackModeShouldEnd()
    {
        if(inComeBackMode && !gameMode.comeBackActive && !inCombo)
        {
            inComeBackMode = false;
        }
    }

    #region Deprecated
    /*
    /// <summary>
    /// Respawns Player
    /// </summary>
    public void Respawn()
    {
        movement.ResetValues();
        percent = 0;
        transform.position = spawnPos;
        if(lifes > 1)
        {
            lifes--;
        }
        else if(lifes <= 1)
        {
            if(endGameAction != null)
                endGameAction();
        }
        UpdateUI();
    }
    */
    #endregion

    //      Private      //
    protected void RotateToFlyDirection()
    {
        Vector3 difference = rb.velocity - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        meshController.transform.rotation = Quaternion.Euler(0,0, (rotationZ - 90));
    }
    /// <summary>
    /// Let the Character Reflect from Objects (Bounce)
    /// </summary>
    protected void Bounce()
    {
        if (isDisabled)
        {
            // Bounce Ray
            RaycastHit hit;
            if (MyRayCast.RayCastInDirection(transform.position, new Vector3(rb.velocity.x, rb.velocity.y, 0), out hit, 0.9f))
            {
                // End Bounce
                //if (lastBounceObj != null && lastBounceObj == hit.transform.gameObject)
                //{
                //    EndDisable();
                //    return;
                //}

                // ToDo: Check if the Character Bounced against some thing Special 

                BounceType bounceType = GetPhysiksMaterial(hit);

                audioEvents.Bounce(bounceType);

                // Count enemy Combo Up
                enemyCharacter.Combo(ComboState.Bounce);
                eventDelegate(EventState.Bounce);
                // COunt All Bounces for the EndScreen
                enemyCharacter.bounceAction(enemyCharacter.playerEnum);
                // Spawn Bounce Particle
                Instantiate(pD.bounce, hit.point, Quaternion.identity);
                // New Bounce Direction
                Vector3 direction = Vector3.Reflect(new Vector3(rb.velocity.x, rb.velocity.y, 0), new Vector3(hit.normal.x, hit.normal.y, 0));
                // Set Velocity to the new direction
                rb.velocity = direction;
                // Change Rotation to Fly direction
                RotateToFlyDirection();

                StartCoroutine(FreezCharacter(false, freezTimeBounce, false));

                if(shakeCameraAction != null)
                    shakeCameraAction(false);
            }
            //else if (MyEpsilon.Epsilon(rb.velocity.x, 0, 1) && MyEpsilon.Epsilon(rb.velocity.y, 0, 1))
            //{
            //    EndDisable();
            //}
        }
    }

    BounceType GetPhysiksMaterial(RaycastHit hit)
    {
        if(hit.collider.material.name == mat_Metal.name + " (Instance)")
        {
            return BounceType.Metal;
        }
        else if(hit.collider.material.name == mat_Concrete.name + " (Instance)")
        {
            return BounceType.Concrete;
        }
        else if (hit.collider.material.name == mat_Stone.name + " (Instance)")
        {
            return BounceType.Stone;
        }
        else if (hit.collider.material.name == mat_Rubber.name + " (Instance)")
        {
            return BounceType.Rubber;
        }
        else if (hit.collider.material.name == mat_Glass.name + " (Instance)")
        {
            return BounceType.Glass;
        }
        else
        {
            return BounceType.Concrete;
        }

    }

    /// <summary>
    ///  Resets the Combo Vars
    /// </summary>
    void ResetComboValues()
    {
        // XOR Vars to reset them to 0
        // Just 4 fun, heared it should be faster than var = 0
        combo = combo ^ combo;
        multiplier = 1;
        hitCounter = 1;
    }

    /// <summary>
    /// Updates all Changes to the UI
    /// </summary>
    void UIStart()
    {
        if (ui != null)
        {
            ui.UpdateScore(score);
            ui.UpdateMultiplier(multiplier);
            ui.UpdateCombo(combo);
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

    [ColorUsageAttribute(false, true)] public Color blendRed;
    [ColorUsageAttribute(false, true)] public Color blendColor;
    IEnumerator Blink()
    {
        float speed = 10;
        
        while (isDisabled)
        {
            bodyRenderer.material.SetColor(colorID, Color.Lerp(bodyColor, blendRed, Mathf.PingPong(Time.time * speed, 1)));
            clothRenderer.materials[0].SetColor(colorID, Color.Lerp(clothColor, blendRed, Mathf.PingPong(Time.time * speed, 1)));
            clothRenderer.materials[1].SetColor(colorID, Color.Lerp(swordColor, blendRed, Mathf.PingPong(Time.time * speed, 1)));
            
            yield return null;
        }

        ReturnColorToNoraml();

        StartCoroutine(Recovery());
    }
    IEnumerator Recovery()
    {
        bodyRenderer.material.SetColor(colorID, blendColor);
        clothRenderer.materials[0].SetColor(colorID, blendColor);
        clothRenderer.materials[1].SetColor(colorID, blendColor);

        bodyRenderer.material.SetColor(emissionID, blendColor);
        clothRenderer.materials[0].SetColor(emissionID, blendColor);
        clothRenderer.materials[1].SetColor(emissionID, blendColor);

        float time = 1f;
        Color playerColor = Color.white;


        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                playerColor = PlayerInfoManager.playerOne.color;
                break;
            case PlayerEnum.PlayerTwo:
                playerColor = PlayerInfoManager.playerTwo.color;
                break;
        }
        while(time > 0f)
        {
            bodyRenderer.material.SetColor(emissionID, Color.Lerp(blendColor / 10, blendColor, time));

            time -= Time.deltaTime * 2;
        }
        
        ReturnColorToNoraml();
        yield return null;
    }
    protected IEnumerator FreezCharacter(bool wigle, float maxTime, bool hitting)
    {

        float time = 0;
        float speed = 15;
        Vector3 vel = this.rb.velocity;
        Vector3 pos = this.transform.position;
        
        if (hitting)
            animator.speed = 0;

        this.rb.velocity = Vector3.zero;
        this.rb.useGravity = false;
        while(time < maxTime)
        {
            if (wigle)
            {
                this.transform.position = new Vector3(pos.x + Mathf.PingPong(Time.time * speed, 0.5f),
                    pos.y,
                    pos.z);
            }
            else
            {
                this.transform.position = pos;
            }

            time += Time.deltaTime;
            yield return null;
        }
        this.transform.position = pos;

        if(hitting)
            animator.speed = 1;

        this.rb.useGravity = true;
        this.rb.velocity = vel;
        yield return null;
    }
    public void ReturnColorToNoraml()
    {
        bodyRenderer.material.SetColor(colorID, bodyColor);
        clothRenderer.materials[0].SetColor(colorID, clothColor);
        clothRenderer.materials[1].SetColor(colorID, swordColor);

        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                bodyRenderer.material.SetColor(emissionID, Color.white);
                clothRenderer.materials[0].SetColor(emissionID, PlayerInfoManager.playerOne.color);
                clothRenderer.materials[1].SetColor(emissionID, PlayerInfoManager.playerOne.color);
                break;
            case PlayerEnum.PlayerTwo:
                bodyRenderer.material.SetColor(emissionID, Color.white);
                clothRenderer.materials[0].SetColor(emissionID, PlayerInfoManager.playerTwo.color);
                clothRenderer.materials[1].SetColor(emissionID, PlayerInfoManager.playerTwo.color);
                break;
        }
    }
}

