using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Helper
    MyCharacter myCharacter;
    Rigidbody rb;
    PlayerEnum playerEnum = PlayerEnum.NotAssigned;
    bool lookToTheRight;
    bool isTurningRight = false;
    bool isTurningLeft = false;
    float currentTurntime = 0;

    Vector3 lastPos;

    [Header("Time the Character needs to turn")]
    public float maxTurningTime;

    // Movement
    [Header("Movement")]
    public float movementSpeed;
    public float inAirSpeed;
    float wallDetectionLength = 0.6f;
    bool isFalling = false;
    bool canMove = true;
    bool isNotMoving = true;
   
    // Jump
    public float jumpVelocity;
    [Range(0, 10)]
    public float fallSpeed;
    int jumps = 0;
    [Header("How much Jumps in a Row")]
    public int maxJumps;

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

    //WallSlide
    bool isOnWallRight = false;
    bool isOnWallLeft = false;
    
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
                InputManager.P1_AButtonDownAction += Jump;
                InputManager.P1_LeftTriggerDownAction += DashCheck;
                InputManager.P1_RightTiggerDownAction += DashCheck;
                InputManager.P1_LeftStickZeroAction += LeftStickZeroed;

                break;
            case PlayerEnum.PlayerTwo:
                InputManager.P2_LeftStickRightAction += MoveRight;
                InputManager.P2_LeftStickLeftAction += MoveLeft;
                InputManager.P2_AButtonDownAction += Jump;
                InputManager.P2_LeftTriggerDownAction += DashCheck;
                InputManager.P2_RightTiggerDownAction += DashCheck;
                InputManager.P2_LeftStickZeroAction += LeftStickZeroed;

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
    void LeftStickZeroed()
    {
        if(isNotMoving)
            isNotMoving = true;
    }

    //////////////////////////////////////////////////
    ////////////////       Helper       //////////////
    //////////////////////////////////////////////////

    public void ResetJumps()
    {
        jumps = 0;
    }
    
    void DashInvinceble()
    {
        myCharacter.canGetDamaged = true;
    }

    void DashCheck()
    {
        if (myCharacter.isDisabled)
            return;

        switch(playerEnum)
        {
            case PlayerEnum.PlayerOne:
                if(Input.GetAxisRaw("P1_Horizontal") == 1)
                {
                    //Right
                    DirectionDash(true);
                }
                else if(Input.GetAxisRaw("P1_Horizontal") == -1)
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

        if(this.gameObject.transform.position.x < 0)
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
            if(lookToTheRight)
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
        if(this.isTurningRight)
        {
            if(this.transform.rotation == new Quaternion(0, 0, 0, 1))
            {
                this.isTurningRight = false;
                return;
            }

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, new Quaternion(0,0,0,1), 0.2f);
        }
        else if(this.isTurningLeft)
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
        if (isNotMoving)
            isNotMoving = true;
    }

    void Grounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(this.gameObject.transform.position, new Vector3(0, -this.transform.position.y, 0), out hit, 1, 9, QueryTriggerInteraction.Ignore) && isFalling)
        {
            this.isOnWallLeft = false;
            this.isOnWallRight = false;
            if (jumps > 0)
                ResetJumps();
            if (currentDashes > 0)
                currentDashes = 0;
            if (myCharacter.isDisabled)
                myCharacter.isDisabled = false;
            this.isFalling = false;
        }
    }

    //////////////////////////////////////////////////
    //////////////// Movement Functions //////////////
    //////////////////////////////////////////////////

    void MoveRight()
    {
        if (!this.canMove)
            return;

        RaycastHit hit;

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
                        rb.velocity += Vector3.right * this.inAirSpeed;
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
                        rb.velocity += Vector3.right * this.inAirSpeed;
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
                        rb.velocity += Vector3.left * this.inAirSpeed;
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
                        rb.velocity += Vector3.left * this.inAirSpeed;
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
            if(this.gameObject.transform.position.x < 0)
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.right * jumpVelocity;
                jumps++;
            }
            else
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.right * jumpVelocity;
                jumps++;
            }
            LookRight();
            StartCoroutine(JumpCoolDown());
        }
        else if(this.isOnWallRight && this.isFalling)
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
            LookLeft();
            StartCoroutine(JumpCoolDown());
        }
        else if (jumps < maxJumps)
        {
            this.rb.velocity = new Vector3(rb.velocity.x, jumpVelocity , 0);
            jumps++;
        }
        this.isFalling = true;
    }

    void DashStanding()
    {
        if(canDash)
        {
            myCharacter.canGetDamaged = false;
            canDash = false;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            StartCoroutine(DashCoolDown());
            StartCoroutine(DashTime());
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

            if (jumps == 0)
                rb.velocity = Vector3.down * 2;
        }
        else if (rb.velocity.y < 0)
        {
            if (!isFalling)
                isFalling = true;
            rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed - 1) * Time.deltaTime;
            FallingWallDetection();
        }
        else if (rb.velocity.y > 0)
        {
            if (!isFalling)
                this.isFalling = true;
        }
        Grounded();
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
                    rb.velocity = new Vector3(this.rb.velocity.x, 0, 0);
                    StartCoroutine(DashCoolDown());
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
        yield return new WaitForSeconds(0.1f);
        this.canMove = true;
    }

    IEnumerator DashCoolDown()
    {
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}
