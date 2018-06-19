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
    public float X;
    
    [HideInInspector] public PlayerEnum playerEnum = PlayerEnum.NotAssigned;
    [HideInInspector] public bool canGetDamaged = true;
    [HideInInspector] public bool isDisabled = false;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public GameObject enemy;
    [HideInInspector] public MyCharacter enemyCharacter;
    public float disabledTime;

    [Header("Animator")]
    public Animator animator;
    int animIsFalling;
    int animDisabled;
    int animGroundBlend;
    int animAirBlend;
    int animAttackBlend;
    int animJumpBlend;

    protected Rigidbody rb;
    protected Movement movement;

    int lifes = 3;
    
    int percent = 0;

    Vector3 spawnPos;
    InGameUI ui;

    protected GameObject lastBounceObj;

    [SerializeField] protected ParticleData pD;

    [HideInInspector] public int hitCounter = 0;
    [HideInInspector] public int combo = 0;
    [HideInInspector] public int multiplier = 1;
    [HideInInspector] public int score = 0;

    //////////// Collision ///////////

    [HideInInspector] public bool xNormalHitBox = false;
    [HideInInspector] public bool xUpHitBox = false;
    [HideInInspector] public bool xDownHitBox = false;
    [HideInInspector] public bool xUpHitAngeldBox = false;
    [HideInInspector] public bool xDownHitAngeldBox = false;
    [HideInInspector] public bool sUpHitBox = false;
    [HideInInspector] public bool sDownHitBox = false;

    //////////// Menu ///////////

    [HideInInspector] public bool hasSlider = false;
    [HideInInspector] public GameObject slider;

    //////////// Deleagtes ///////////

    protected delegate void EventDelegate(EventState eventState);
    protected EventDelegate eventDelegate;

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
            animGroundBlend = Animator.StringToHash("GroundBlend");
            animAirBlend = Animator.StringToHash("AirBlend");
            animAttackBlend = Animator.StringToHash("AttackBlend");
            animJumpBlend = Animator.StringToHash("JumpBlend");
        }
        if (ui == null)
        {
            Debug.Log("No UI Element Found");
        }
        else
        {
            UIStart();
        }
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

    void EventCheck(EventState eventState)
    {
        switch (eventState)
        {
            case EventState.Idle:
                animator.SetFloat(animGroundBlend, (float)GroundState.Idle);

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
                //CancelInvoke();
                //Invoke("Falling", 0.3f);
                Instantiate(pD.ps_JumpOnGround, new Vector3(this.transform.position.x, this.transform.position.y + 1f, 0),  Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y - 90, this.transform.rotation.z));  // Spawn Particle
                break;
            case EventState.JumpInAir:
                animator.SetFloat(animAirBlend, (float)FallState.Jump);
                animator.SetFloat(animJumpBlend, (float)JumpState.JumpInAir);
                //CancelInvoke();
                //Invoke("Falling", 0.3f);
                Instantiate(pD.ps_JumpInAir, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0), Quaternion.identity);  // Spawn Particle
                break;
            case EventState.JumpInAir2:
                animator.SetFloat(animAirBlend, (float)FallState.Jump);
                animator.SetFloat(animJumpBlend, (float)JumpState.JumpInAirTwo);
                //CancelInvoke();
                //Invoke("Falling", 0.3f);
                Instantiate(pD.ps_JumpInAir, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0), Quaternion.identity);  // Spawn Particle
                break;
            case EventState.Fall:
                animator.SetFloat(animAirBlend, (float)FallState.Fall);

                break;
            case EventState.Landing:
                animator.SetBool(animIsFalling, false);
                animator.SetFloat(animGroundBlend, (float)GroundState.Landing);
                Instantiate(pD.ps_Landing, new Vector3(this.transform.position.x, this.transform.position.y - 1f, 0), Quaternion.Euler(this.transform.rotation.x + 90, this.transform.rotation.y, this.transform.rotation.z));  // Spawn Particle
                break;
            case EventState.Dodge:
                animator.SetFloat(animAirBlend, (float)FallState.Dodge);

                break;
            case EventState.GetHit:

                Instantiate(pD.ps_GetDamaged, new Vector3(this.transform.position.x, this.transform.position.y, 3), Quaternion.identity);  // Spawn Particle
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
            default:
                Debug.Log("Coundnt Find State! Character: " + gameObject.name);
                break;

        }
    }
    
    void Awake()
    {
        spawnPos = transform.position;
        pD.trail.SetActive(false);
        pD.ps_Disabled.Stop();
    }

    void Start()
    {

	}

    //////////////////////////////////////////////////
    ////////////////       Update       //////////////
    //////////////////////////////////////////////////

    void Update()
    {
        //Bounce();
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

    /// <summary>
    /// Lets the Character not be able to move
    /// </summary>
    protected void IsAttacking()
    {
        movement.CantMove();
    }
    /// <summary>
    /// Lets the Character be able to move again
    /// </summary>
    protected void EndAttacking()
    {
        movement.CanMoveTrue();
    }
    /// <summary>
    /// Disables the own Character
    /// </summary>
    public void Disable()
    {
        // SHow the Combo Counter
        enemyCharacter.ui.ShowCombo();
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

        // SHow the Combo Counter
        enemyCharacter.ui.ShowCombo();
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
    }
    /// <summary>
    /// Ends the Disable Effect on the own Character
    /// </summary>
    public void EndDisable()
    {
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
      
        Instantiate(pD.ps_GetDamaged, transform.position, Quaternion.identity);

        if (dir.x < 0)                             // Direction = right
        {
            // Deprecated
            if (hard)
            {
                rb.AddForce(new Vector3(-Mathf.Pow(Mathf.Sqrt(basisWert * percent), potenz) * xFactor, enemyPos.y, 0) * gesamtFactor);
            }
            else
            {
                rb.AddForce(new Vector3(-Mathf.Pow(Mathf.Sqrt(basisWert * percent) + startForce, potenz), dir.y, 0));
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
                rb.AddForce(new Vector3(Mathf.Pow(Mathf.Sqrt(basisWert * percent) + startForce, potenz), dir.y, 0));
            }
        }

        Debug.DrawLine(transform.position, dir, Color.red, 5);
        if (!IsFalling())
        {
            RaycastHit hit;
            if(MyRayCast.RayCastInDirection(transform.position, -dir, out hit, 2))
            {

            }
        }

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
        enemyCharacter.Disable(disabledTime);
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
                // Update Combo
                ui.UpdateCombo(combo);
                // Update Combo Multiplier
                ui.UpdateMultiplier(multiplier);
                break;
        }
        // Update Combo
    }
    /// <summary>
    /// Ends the Combo and add the Points to the Score
    /// </summary>
    public void EndCombo()
    {
        // Calc new Score
        combo = hitCounter * combo;
        score += combo * multiplier;

        //Safe Data for EndScreen
        playerDataAction(playerEnum, combo, multiplier, score);

        // Update the new Score
        ui.UpdateScore(score);

        // Reset the Combo vars back to Start
        ResetComboValues();

        // Hidde Combo Counter
        ui.HiddeCombo();

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
    /// <summary>
    /// Let the Character Reflect from Objects (Bounce)
    /// </summary>
    protected void Bounce()
    {
        if (isDisabled)
        {
            // Bounce Ray
            RaycastHit hit;
            if (MyRayCast.RayCastInDirection(transform.position, new Vector3(rb.velocity.x, rb.velocity.y, 0), out hit, 2))
            {
                // End Bounce
                if (lastBounceObj != null && lastBounceObj == hit.transform.gameObject)
                {
                    EndDisable();
                    return;
                }

                // ToDo: Check if the Character Bounced against some thing Special 

                // Count enemy Combo Up
                enemyCharacter.Combo(ComboState.Bounce);
                // COunt All Bounces for the EndScreen
                enemyCharacter.bounceAction(enemyCharacter.playerEnum);
                // Spawn Bounce Particle
                Instantiate(pD.bounce, hit.point, Quaternion.identity);
                // New Bounce Direction
                Vector3 direction = Vector3.Reflect(new Vector3(rb.velocity.x, rb.velocity.y, 0), new Vector3(hit.normal.x, hit.normal.y, 0));
                Debug.DrawLine(transform.position, hit.point);
                rb.velocity = direction;
                // Safe Obj to Check at the next bounce if the Obj is the Same
                lastBounceObj = hit.transform.gameObject;
            }
            else if (MyEpsilon.Epsilon(rb.velocity.x, 0, 1) && MyEpsilon.Epsilon(rb.velocity.y, 0, 1))
            {
                EndDisable();
            }
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
        combo = combo ^ combo;
        multiplier = 1;
        hitCounter = hitCounter ^ hitCounter;
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
            ui.HiddeCombo();
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
}
