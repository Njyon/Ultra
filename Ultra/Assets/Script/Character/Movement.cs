using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Components
    Turning turnclass;
    Dash dash;
    FallComponent fallComp;

    // Helper
    [HideInInspector] public bool islookingToTheRight;
    bool forcingDown = false;
    bool forceDownEnabled = true;
    bool isStunned = false;
    bool isIdling = true;
    bool checkForLanding = true;
    Vector3 lastPos;
    MyCharacter myCharacter;
    Rigidbody rb;
    PlayerEnum playerEnum = PlayerEnum.NotAssigned;

    // Movement
    [Header("Movement")]
    public float movementSpeed;
    public float inAirSpeed;
    [HideInInspector] public bool isFalling = false;
    float wallDetectionLength = 0.6f;
    bool canMove = true;
    bool isNotMoving = true;

    // Jump
    public float jumpVelocity;
    [HideInInspector] public int jumps = 0;
    [Range(0, 10)] public float fallSpeed;
    [Range(5, 30)] public float maxFallVelocity;
    [Header("How much Jumps in a Row")] public int maxJumps;

    //WallSlide
    bool isOnWallRight = false;
    bool isOnWallLeft = false;

    //Delegates
    public delegate void EventDelegate(EventState eventState);
    public EventDelegate eventDelegate;

    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    //////////////////////////////////////////////////
    ////////////////       Update       //////////////
    //////////////////////////////////////////////////

    void Update()
    {
        if (myCharacter == null && dash != null && turnclass != null)
            return;

        if (checkForLanding)
            isFalling = fallComp.Grounded();
        Idle();
        fallComp.Falling();
        if(dash.isDashing)
            dash.Dashing(isFalling, transform.position);
        WallSlide();
        turnclass.IUpdate(this.transform);


    }
    private void FixedUpdate()
    {
        lastPos = this.transform.position;
    }

    //////////////////////////////////////////////////
    ////////////////       Input        //////////////
    //////////////////////////////////////////////////

    public void AssigneInput()
    {
        // Set up Vars
        myCharacter = this.gameObject.GetComponent<MyCharacter>();
        playerEnum = myCharacter.playerEnum;
        turnclass = new Turning();
        #region Set up Dash
        dash = GetComponent<Dash>();
        dash.myCharacter = myCharacter;
        dash.playerEnum = playerEnum;
        dash.rb = rb;
        #endregion
        fallComp = new FallComponent(forceDownEnabled, forcingDown, isOnWallLeft, isOnWallRight, isFalling, islookingToTheRight, fallSpeed, wallDetectionLength, maxFallVelocity, transform, rb, this, dash, myCharacter);

        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.P1_LeftStickRightAction += MoveRight;
                InputManager.P1_LeftStickLeftAction += MoveLeft;
                InputManager.P1_LeftTriggerDownAction += dash.DashCheck;
                InputManager.P1_RightTiggerDownAction += dash.DashCheck;
                InputManager.P1_LeftStickZeroAction += FallStraight;
                InputManager.P1_LeftStickDownAction += ForceDown;
                InputManager.P1_LeftStickUpAction += LookUp;

                turnclass.eventDelegate += EventCheck;
                dash.eventDelegate += EventCheck;
                fallComp.eventDelegate += EventCheck;

                InputManager.p1_OnKeyPressed += (KeyCode keyCode) =>
                {
                    if(keyCode == KeyCode.Joystick1Button0 && canMove)
                    {
                        Jump();
                    }
                };

                break;
            case PlayerEnum.PlayerTwo:
                InputManager.P2_LeftStickRightAction += MoveRight;
                InputManager.P2_LeftStickLeftAction += MoveLeft;
                InputManager.P2_LeftTriggerDownAction += dash.DashCheck;
                InputManager.P2_RightTiggerDownAction += dash.DashCheck;
                InputManager.P2_LeftStickZeroAction += FallStraight;
                InputManager.P2_LeftStickDownAction += ForceDown;
                InputManager.P2_LeftStickUpAction += LookUp;

                turnclass.eventDelegate += EventCheck;
                dash.eventDelegate += EventCheck;
                fallComp.eventDelegate += EventCheck;

                InputManager.p2_OnKeyPressed += (KeyCode keyCode) =>
                {
                    if (keyCode == KeyCode.Joystick2Button0 && canMove)
                    {
                        Jump();
                    }
                };

                break;
            case PlayerEnum.NotAssigned:
                Debug.Log("<color=red> MovementClass cant Find the PlayerEnum </color>");

                break;
        }
    }
    void RemoveInput()
    {
        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:

                break;
            case PlayerEnum.PlayerTwo:

                break;
            case PlayerEnum.NotAssigned:
                Debug.Log("<color=red> MovementClass cant Find the PlayerEnum </color>");
                break;
        }
    }

    //////////////////////////////////////////////////
    ////////////////       Helper       //////////////
    //////////////////////////////////////////////////

    //      Public      //
    public void CantMove(float time)
    {
        canMove = false;
        Invoke("CanMove", time);
    }
    public void CantMove()
    {
        canMove = false;
    }
    public void CanMove()
    {
        canMove = true;
    }
    /// <summary>
    /// Resets currents to Zero
    /// </summary>
    public void ResetJumps()
    {
        jumps = 0;
    }
    /// <summary>
    /// Stunes the Player for "time"
    /// </summary>
    /// <param name="time"></param>
    public void Stun(float time)
    {
        if (!isStunned)
        {
            isStunned = true;
            canMove = false;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            Invoke("EndStun", time);
        }
    }
    /// <summary>
    /// Stuns Player Till EndStun()
    /// </summary>
    public void Stun()
    {
        if(!isStunned)
        {
            isStunned = true;
            canMove = false;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
    }
    /// <summary>
    /// Ends the Stun Effect from the Player
    /// </summary>
    public void EndStun()
    {
        isStunned = false;
        canMove = true;
        rb.useGravity = true;
    }
    /// <summary>
    /// Lets Character Look to the Right imidetly
    /// </summary>
    public void LookRightNow()
    {
        transform.rotation = new Quaternion(0, 0, 0, 1);
    }
    /// <summary>
    /// Lets Character Look to the Left imidetly
    /// </summary>
    public void LookLeftNow()
    {
        transform.rotation = new Quaternion(0, 1, 0, 0);
    }
    /// <summary>
    /// Let the Character Jump of the amount of param jumpforce| WARNING: Needed for a SpecialAttack DONT USE ENYWEHERE ELSE!
    /// </summary>
    /// <param name="jumpForce"></param>
    public void SpecialJump(float jumpForce)
    {
        this.rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        StartCoroutine(ForceDownDelay());
        this.isFalling = true;
    }
    /// <summary>
    /// Let the Character Jump | WARNING: Needed for a SpecialAttack DONT USE ENYWEHERE ELSE!
    /// </summary>
    public void SpecialJump()
    {
        this.rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, 0);
        StartCoroutine(ForceDownDelay());
        this.isFalling = true;
    }

    //      Private     //
    void EventCheck(EventState eventState)
    {
        eventDelegate(eventState);
    }

    void WallSlide()
    {
        if (myCharacter.isDisabled)
            return;

        if (this.isOnWallRight)
        {
            RaycastHit hit;
            if (this.gameObject.transform.localPosition.x < 0)
            {
                if (!Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
                {
                    this.isOnWallRight = false;
                }
            }
            else
            {
                if (!Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
                {
                    this.isOnWallRight = false;
                }
            }
            //currentDashes = 0;
        }
        else if (this.isOnWallLeft)
        {
            RaycastHit hit;
            if (this.gameObject.transform.localPosition.x < 0)
            {
                if (!Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
                {
                    this.isOnWallLeft = false;
                }
            }
            else
            {
                if (!Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
                {
                    this.isOnWallLeft = false;
                }
            }
            //currentDashes = 0;
        }
    }
    void Idle()
    {
        if (lastPos == this.transform.position)
        {
            if (!isIdling)
            {
                isIdling = true;
                eventDelegate(EventState.Idle);
            }
        }
        else
        {
            if (isIdling)
                isIdling = false;
        }
    }
    void FallStraight()
    {
        fallComp.fallStraight = true;
        forcingDown = false;
    }
    void ForceDown()
    {
        if (isFalling)
            forcingDown = true;
    }
    void LookUp()
    {
        forcingDown = false;
    }

    //////////////////////////////////////////////////
    //////////////// Movement Functions //////////////
    //////////////////////////////////////////////////

    void MoveRight()
    {
        if (!this.canMove || !dash.canMove)
            return;

        RaycastHit hit;
        islookingToTheRight = true;
        fallComp.fallStraight = false;
        forcingDown = false;

        turnclass.LookRight(this.transform.rotation);
        if (this.gameObject.transform.localPosition.x < 0)              //NEGATIVE
        {
            if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
            {
                //WallSlide
                if (!this.isOnWallLeft && !this.isOnWallRight && this.isFalling && !myCharacter.isDisabled)
                {
                    this.isOnWallRight = true;
                    ResetJumps();
                    rb.velocity = new Vector3(0, 0, 0);
                }
            }
            else
            {
                if (isFalling)
                {
                    if (rb.velocity.x < this.inAirSpeed)
                    {
                        rb.velocity = new Vector3(this.inAirSpeed, this.rb.velocity.y, 0);
                    }
                    else if (rb.velocity.x > this.inAirSpeed)
                    {
                        rb.velocity = new Vector3(this.inAirSpeed, this.rb.velocity.y, 0);
                    }
                }
                else
                {
                    this.gameObject.transform.position += Vector3.right * this.movementSpeed * Time.deltaTime;
                }
            }
        }
        else                                //POSITIVE
        {
            if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
            {
                //WallSlide
                if (!this.isOnWallLeft && !this.isOnWallRight && this.isFalling && !myCharacter.isDisabled)
                {
                    this.isOnWallRight = true;
                    ResetJumps();
                    rb.velocity = new Vector3(0, 0, 0);
                }
            }
            else
            {
                if (isFalling)
                {
                    if (rb.velocity.x < this.inAirSpeed)
                    {
                        rb.velocity = new Vector3(this.inAirSpeed, this.rb.velocity.y, 0);
                    }
                    else if (rb.velocity.x > this.inAirSpeed)
                    {
                        rb.velocity = new Vector3(this.inAirSpeed, this.rb.velocity.y, 0);
                    }
                }
                else
                {
                    this.gameObject.transform.position += Vector3.right * this.movementSpeed * Time.deltaTime;
                }
            }
        }
    }
    void MoveLeft()
    {
        if (!this.canMove || !dash.canMove)
            return;

        RaycastHit hit;
        fallComp.fallStraight = false;
        forcingDown = false;
        islookingToTheRight = false;

        turnclass.LookLeft(this.transform.rotation);
        if (this.gameObject.transform.localPosition.x < 0)          //NEGATIVE
        {
            if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
            {
                //WallSlide
                if (!this.isOnWallLeft && !this.isOnWallRight && this.isFalling && !myCharacter.isDisabled)
                {
                    this.isOnWallLeft = true;
                    ResetJumps();
                    rb.velocity = new Vector3(0, 0, 0);
                }
            }
            else
            {
                if (isFalling)
                {
                    if (rb.velocity.x > -this.inAirSpeed)
                    {
                        rb.velocity = new Vector3(-this.inAirSpeed, this.rb.velocity.y, 0);
                    }
                    else if (rb.velocity.x < -this.inAirSpeed)
                    {
                        rb.velocity = new Vector3(-this.inAirSpeed, this.rb.velocity.y, 0);
                    }
                }
                else
                {
                    this.gameObject.transform.position += Vector3.left * movementSpeed * Time.deltaTime;
                }
            }
        }
        else                            //POSITIVE
        {
            if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength, 9, QueryTriggerInteraction.Ignore))
            {
                //WallSlide
                if (!this.isOnWallLeft && !this.isOnWallRight && this.isFalling && !myCharacter.isDisabled)
                {
                    this.isOnWallLeft = true;
                    ResetJumps();
                    rb.velocity = new Vector3(0, 0, 0);
                }
            }
            else
            {
                if (isFalling)
                {
                    if (rb.velocity.x > -this.inAirSpeed)
                    {
                        rb.velocity = new Vector3(-this.inAirSpeed, this.rb.velocity.y, 0);
                    }
                    else if (rb.velocity.x < -this.inAirSpeed)
                    {
                        rb.velocity = new Vector3(-this.inAirSpeed, this.rb.velocity.y, 0);
                    }
                }
                else
                {
                    this.gameObject.transform.position += Vector3.left * movementSpeed * Time.deltaTime;
                }
            }
        }
    }
    void Jump()
    {
        if (myCharacter.isDisabled)
            return;

        if (this.isOnWallLeft && this.isFalling)
        {
            if (this.gameObject.transform.position.x < 0)
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.right * jumpVelocity;
                jumps++;
            }
            else
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.right * jumpVelocity;
                jumps++;
            }
            turnclass.LookRight(this.transform.rotation);
            StartCoroutine(JumpCoolDown());
            StartCoroutine(ForceDownDelay());
            eventDelegate(EventState.JumpOnWall);
        }
        else if (this.isOnWallRight && this.isFalling)
        {
            if (this.gameObject.transform.position.x < 0)
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.left * jumpVelocity;
                jumps++;
            }
            else
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.left * jumpVelocity;
                jumps++;
            }
            turnclass.LookLeft(this.transform.rotation);
            StartCoroutine(JumpCoolDown());
            StartCoroutine(ForceDownDelay());
            eventDelegate(EventState.JumpOnWall);
        }
        else if (jumps < maxJumps)
        {
            this.rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, 0);
            jumps++;
            StartCoroutine(ForceDownDelay());
            if(isFalling)
            {
                eventDelegate(EventState.JumpAir);
            }
            else
            {
                checkForLanding = false;
                Invoke("CheckForLandingDelay", 0.1f);
                eventDelegate(EventState.Jump);
            }
        }
        this.isFalling = true;
    }

    void CheckForLandingDelay()
    {
        checkForLanding = true;
    }

    IEnumerator JumpCoolDown()
    {
        this.canMove = false;
        yield return new WaitForSeconds(0.2f);
        this.canMove = true;
    }
    IEnumerator DogeTime(float time)
    {
        myCharacter.canGetDamaged = false;
        yield return new WaitForSeconds(time);
        myCharacter.canGetDamaged = true;
    }
    IEnumerator ForceDownDelay()
    {
        forceDownEnabled = false;
        yield return new WaitForSeconds(0.1f);
        forceDownEnabled = true;
    }
}