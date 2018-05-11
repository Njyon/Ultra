using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Helper
    MyCharacter myCharacter;
    Rigidbody rb;
    PlayerEnum playerEnum = PlayerEnum.NotAssigned;
    [HideInInspector] public bool lookToTheRight;
    bool isTurningRight = false;
    bool isTurningLeft = false;
    bool fallStraight = true;
    bool forcingDown = false;
    bool forceDownEnabled = true;
    bool isStunned = false;
    float currentTurntime = 0;
    bool isIdling = true;
    Vector3 lastPos;

    [Header("Time the Character needs to turn")]
    public float maxTurningTime;

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
    int jumps = 0;
    [Range(0, 10)] public float fallSpeed;
    [Range(5, 30)] public float maxFallVelocity;
    [Header("How much Jumps in a Row")] public int maxJumps;
    [HideInInspector] public JumpState jumpState = JumpState.OnGround;

    //Dash
    [Header("Dash")]
    public int maxDashes;
    int currentDashes = 0;
    public float dashLength;
    public float dashTime;
    float currentDashTime;
    float journeyLength;
    float dashWallDistance = 0.6f;
    bool isDashing = false;
    Vector3 dashEndPoint;
    bool canDash = true;
    public float dashCoolDown;
    public float dogeTime;

    //WallSlide
    bool isOnWallRight = false;
    bool isOnWallLeft = false;

                            //       Delegates       //

    public delegate void JumpDelegate(JumpState jumpState);
    public JumpDelegate JumpDelegateAction;

    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    //////////////////////////////////////////////////
    ////////////////       Update       //////////////
    //////////////////////////////////////////////////

    void Update()
    {
        if (myCharacter == null)
            return;
        
        Idle();
        Falling();
        Dash();
        WallSlide();
        Turning();
        Grounded();

        lastPos = this.transform.position;
    }

    //////////////////////////////////////////////////
    ////////////////       Input        //////////////
    //////////////////////////////////////////////////

    public void AssigneInput()
    {
        // GET MyCharacter and SET the Player ENUM
        myCharacter = this.gameObject.GetComponent<MyCharacter>();
        playerEnum = myCharacter.playerEnum;

        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.P1_LeftStickRightAction += MoveRight;
                InputManager.P1_LeftStickLeftAction += MoveLeft;
                InputManager.P1_LeftTriggerDownAction += DashCheck;
                InputManager.P1_RightTiggerDownAction += DashCheck;
                InputManager.P1_LeftStickZeroAction += FallStraight;
                InputManager.P1_LeftStickDownAction += ForceDown;
                InputManager.P1_LeftStickUpAction += LookUp;

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
                InputManager.P2_LeftTriggerDownAction += DashCheck;
                InputManager.P2_RightTiggerDownAction += DashCheck;
                InputManager.P2_LeftStickZeroAction += FallStraight;
                InputManager.P2_LeftStickDownAction += ForceDown;
                InputManager.P2_LeftStickUpAction += LookUp;

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
    /// Resets currents Jumps to Zero
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


    //      Private     //
    void DashCheck()
    {
        if (myCharacter.isDisabled)
            return;

        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                if (Input.GetAxisRaw("P1_Horizontal") == 1)
                {
                    //Right
                    DirectionDash(true);
                }
                else if (Input.GetAxisRaw("P1_Horizontal") == -1)
                {
                    //Left
                    DirectionDash(false);
                }
                else
                {
                    DashStanding();
                }
                break;
            case PlayerEnum.PlayerTwo:
                if (Input.GetAxisRaw("P2_Horizontal") == 1)
                {
                    //Right
                    DirectionDash(true);

                }
                else if (Input.GetAxisRaw("P2_Horizontal") == -1)
                {
                    //Left
                    DirectionDash(false);
                }
                else
                {
                    DashStanding();
                }
                break;
            case PlayerEnum.NotAssigned:
                Debug.Log("<color=red> MovementClass -> DashCheck() cant Find the PlayerEnum </color>");
                break;
        }
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
            currentDashes = 0;
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
            currentDashes = 0;
        }
    }

    void FallingWallDetection()
    {
        if (myCharacter.isDisabled)
            return;

        RaycastHit hit;

        if (this.gameObject.transform.position.x < 0)
        {
            if (lookToTheRight)
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength + 0.1f, 9, QueryTriggerInteraction.Ignore))
                {
                    //WallSlide
                    if (!this.isOnWallLeft && !this.isOnWallRight)
                    {
                        this.isOnWallRight = true;
                        ResetJumps();
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                }
            }
            else
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength + 0.1f, 9, QueryTriggerInteraction.Ignore))
                {
                    //WallSlide
                    if (!this.isOnWallLeft && !this.isOnWallRight)
                    {
                        this.isOnWallLeft = true;
                        ResetJumps();
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                }
            }
        }
        else
        {
            if (lookToTheRight)
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength + 0.1f, 9, QueryTriggerInteraction.Ignore))
                {
                    //WallSlide
                    if (!this.isOnWallLeft && !this.isOnWallRight)
                    {
                        this.isOnWallRight = true;
                        ResetJumps();
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                }
            }
            else
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength + 0.1f, 9, QueryTriggerInteraction.Ignore))
                {
                    //WallSlide
                    if (!this.isOnWallLeft && !this.isOnWallRight)
                    {
                        this.isOnWallLeft = true;
                        ResetJumps();
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                }
            }
        }
    }

    void LookRight()
    {
        if (this.transform.rotation.y != 0)
        {
            this.currentTurntime = this.maxTurningTime;
            this.isTurningRight = true;
            this.lookToTheRight = true;

            this.isTurningLeft = false;
        }
    }

    void LookLeft()
    {
        if (this.transform.rotation.y != 180)
        {
            this.currentTurntime = this.maxTurningTime;
            this.isTurningLeft = true;
            this.lookToTheRight = false;

            this.isTurningRight = false;
        }
    }

    void Turning()
    {
        if (this.isTurningRight)
        {
            if (this.transform.rotation == new Quaternion(0, 0, 0, 1))
            {
                this.isTurningRight = false;
                return;
            }

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, new Quaternion(0, 0, 0, 1), 0.2f);
        }
        else if (this.isTurningLeft)
        {
            if (this.transform.rotation == new Quaternion(0, 1, 0, 0))
            {
                this.isTurningLeft = false;
                return;
            }

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, new Quaternion(0, 1, 0, 0), 0.2f);
        }
    }

    void Idle()
    {
        if (lastPos == this.transform.position)
        {
            if (!isIdling)
                isIdling = true;
        }
        else
        {
            if (isIdling)
                isIdling = false;
        }
    }

    void Grounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(this.gameObject.transform.position, new Vector3(0, -this.transform.position.y, 0), out hit, 1, 9, QueryTriggerInteraction.Ignore))
        {
            if(isFalling)
            {
                this.isOnWallLeft = false;
                this.isOnWallRight = false;
                this.isFalling = false;
                this.canDash = true;
                jumpState = JumpState.OnGround;

                if (jumps > 0)
                    ResetJumps();
                if (currentDashes > 0)
                    currentDashes = 0;
                if (myCharacter.isDisabled)
                    myCharacter.isDisabled = false;
            }
        }
        else
        {
            isFalling = true;
        }
    }
    
    void FallStraight()
    {
        fallStraight = true;
        forcingDown = false;
    }

    void ForceDown()
    {
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
        if (!this.canMove)
            return;

        RaycastHit hit;
        fallStraight = false;
        forcingDown = false;

        LookRight();
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
        if (!this.canMove)
            return;

        RaycastHit hit;
        fallStraight = false;
        forcingDown = false;

        LookLeft();
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
            jumpState = JumpState.OnWallLeft;
            LookRight();
            StartCoroutine(JumpCoolDown());
            StartCoroutine(ForceDownDelay());
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
            jumpState = JumpState.OnWallRight;
            LookLeft();
            StartCoroutine(JumpCoolDown());
            StartCoroutine(ForceDownDelay());
        }
        else if (jumps < maxJumps)
        {
            if (isFalling)
                jumpState = JumpState.InAir;
            this.rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, 0);
            jumps++;
            StartCoroutine(ForceDownDelay());
        }
        JumpDelegateAction(jumpState);
        this.isFalling = true;
    }

    void DashStanding()
    {
        if (canDash)
        {
            myCharacter.canGetDamaged = false;
            canDash = false;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            StartCoroutine(DashCoolDown(dashCoolDown));
            StartCoroutine(DashTime());
            StartCoroutine(DogeTime(dogeTime));
        }
    }

    void DirectionDash(bool directionRight)
    {
        if (!canDash)
            return;

        currentDashTime = dashTime;
        isDashing = true;
        RaycastHit hit;
        StartCoroutine(DashTime());
        currentDashes++;

        if (directionRight)     //Right
        {
            if (this.gameObject.transform.localPosition.x < 0)
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, dashLength, 9, QueryTriggerInteraction.Ignore))            //WALLHIT
                {
                    dashEndPoint = new Vector3(hit.point.x - wallDetectionLength, hit.point.y, hit.point.z);
                }
                else            //NO WALL
                {
                    dashEndPoint = new Vector3(this.gameObject.transform.localPosition.x + dashLength, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
                }
                journeyLength = Vector3.Distance(this.gameObject.transform.localPosition, dashEndPoint);
            }
            else
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, dashLength, 9, QueryTriggerInteraction.Ignore))             //WALLHIT
                {
                    dashEndPoint = new Vector3(hit.point.x - wallDetectionLength, hit.point.y, hit.point.z);
                }
                else                // NO WALL
                {
                    dashEndPoint = new Vector3(this.gameObject.transform.localPosition.x + dashLength, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
                }
                journeyLength = Vector3.Distance(this.gameObject.transform.localPosition, dashEndPoint);
            }
        }
        else                //Left
        {
            if (this.gameObject.transform.localPosition.x < 0)
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(this.gameObject.transform.localPosition.x, 0, 0), out hit, dashLength, 9, QueryTriggerInteraction.Ignore))         //WALLHIT
                {
                    dashEndPoint = new Vector3(hit.point.x + wallDetectionLength, hit.point.y, hit.point.z);
                }
                else                        // NO WALL
                {
                    dashEndPoint = new Vector3(this.gameObject.transform.localPosition.x - dashLength, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
                }
                journeyLength = Vector3.Distance(this.gameObject.transform.localPosition, dashEndPoint);
            }
            else
            {
                if (Physics.Raycast(this.gameObject.transform.position, new Vector3(-this.gameObject.transform.localPosition.x, 0, 0), out hit, dashLength, 9, QueryTriggerInteraction.Ignore))        //WALLHIT
                {
                    dashEndPoint = new Vector3(hit.point.x + wallDetectionLength, hit.point.y, hit.point.z);
                }
                else                        // NO WALL
                {
                    dashEndPoint = new Vector3(this.gameObject.transform.localPosition.x - dashLength, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
                }
                journeyLength = Vector3.Distance(this.gameObject.transform.localPosition, dashEndPoint);
            }
        }
    }

    // Change Gravity for Jump Balancing!  Edit -> Project Setting -> Physics -> Gravity Y
    void Falling()
    {
        if (isDashing)
            return;

        if (this.isOnWallLeft || this.isOnWallRight)
        {
            if (myCharacter.isDisabled)
            {
                this.isOnWallLeft = false;
                this.isOnWallRight = false;
                return;
            }
            if(jumps == 0)
                rb.velocity = Vector3.down * 2;
        }
        else if (isFalling && forcingDown && forceDownEnabled)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed * 2) * Time.deltaTime;
        }
        else if(fallStraight && isFalling)
        {
            if(rb.velocity.x < 0)
            {
                rb.velocity += new Vector3(1, 0, 0);
            }
            else if (rb.velocity.x > 0)
            {
                rb.velocity -= new Vector3(1, 0, 0);
            }
        }
        else if (rb.velocity.y < 0)
        {
            if (!isFalling)
                isFalling = true;
            if (rb.velocity.y > maxFallVelocity)
                rb.velocity = new Vector3(rb.velocity.x, maxFallVelocity, 0);

            //rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed - 1) * Time.deltaTime;
            FallingWallDetection();
        }
        else if (rb.velocity.y > 0)
        {
            if (!isFalling)
                this.isFalling = true;
        }
    }

    void Dash()
    {
        if (currentDashes <= maxDashes)
        {
            if (isDashing)
            {
                if (canDash)
                {
                    canDash = false;
                    if (isFalling)
                    {
                        StartCoroutine(DogeTime(dogeTime));
                        StartCoroutine(DashCoolDown(dashCoolDown));
                    }
                    else
                    {
                        currentDashes = 0;
                        StartCoroutine(DashCoolDown(0.2f));
                    }
                }
                
                currentDashTime -= Time.deltaTime;
                float travel = currentDashTime / journeyLength;

                this.transform.position = Vector3.Lerp(this.gameObject.transform.localPosition, this.dashEndPoint, travel);
            }
        }
    }

    //////////////////////////////////////////////////
    ////////////////    IENUMERATOR     //////////////
    //////////////////////////////////////////////////

    IEnumerator DashTime()
    {
        yield return new WaitForSeconds(0.17f);
        rb.useGravity = true;
        isDashing = false;
    }

    IEnumerator JumpCoolDown()
    {
        this.canMove = false;
        yield return new WaitForSeconds(0.2f);
        this.canMove = true;
    }

    IEnumerator DashCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        canDash = true;
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