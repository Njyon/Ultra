using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Components
    Turning turnclass;
    Dash dash;
    [HideInInspector] public FallComponent fallComp;

    // Helper
    [HideInInspector] public bool islookingToTheRight;
    bool forcingDown = false;
    bool forceDownEnabled = true;
    bool isStunned = false;
    bool isIdling = true;
    bool checkForLanding = true;
    bool checkForIdeling = true;
    Vector3 lastPos;
    MyCharacter myCharacter;
    [HideInInspector] public Rigidbody rb;
    PlayerEnum playerEnum = PlayerEnum.NotAssigned;

    // Movement
    [Header("Movement")]
    public float movementSpeed;
    public float maxInAirSpeed;
    public float inAirAccelerate;
    public float maxWalkAngel;
    float wallDetectionLength = 0.6f;
    bool canMove = true;
    bool isNotMoving = true;

    // Jump
    public float jumpVelocity;
    [HideInInspector] public int jumps = 0;
    [Range(0, 10)] public float fallSpeed;
    [Range(5, 30)] public float maxFallVelocity;

    [Header("How much Jumps in a Row")]
    public int maxJumps;

    [Header("Velocity")]
    [SerializeField] float maxVelocityX;
    [SerializeField] float maxVelocityY;

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
        if (myCharacter == null && dash == null && turnclass == null && fallComp == null)
            return;

        VelocityCheck();

        fallComp.Falling();
        dash.Dashing(fallComp.isFalling);
        //WallSlide();
        turnclass.IUpdate(this.transform);

    }
    private void LateUpdate()
    {
        if (myCharacter == null && dash == null && turnclass == null && fallComp == null)
            return;

        if (checkForLanding)
            fallComp.isFalling = fallComp.Grounded();
    }
    private void FixedUpdate()
    {
        if (myCharacter == null && dash == null && turnclass == null && fallComp == null)
            return;

        if (!myCharacter.isDisabled)
        {
            Idle();
        }
        else
        {
            isIdling = false;
        }
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
        fallComp = new FallComponent(forceDownEnabled, forcingDown, fallSpeed, wallDetectionLength, maxFallVelocity, transform, rb, this, dash, myCharacter);

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


                InputManager.p1_OnKeyPressed += CheckInputDown;

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

                InputManager.p2_OnKeyPressed += CheckInputDown;

                break;
            case PlayerEnum.NotAssigned:
                Debug.Log("<color=red> MovementClass cant Find the PlayerEnum </color>");

                break;
        }
    }
    public void RemoveInput()
    {
        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.P1_LeftStickRightAction -= MoveRight;
                InputManager.P1_LeftStickLeftAction -= MoveLeft;
                InputManager.P1_LeftTriggerDownAction -= dash.DashCheck;
                InputManager.P1_RightTiggerDownAction -= dash.DashCheck;
                InputManager.P1_LeftStickZeroAction -= FallStraight;
                InputManager.P1_LeftStickDownAction -= ForceDown;
                InputManager.P1_LeftStickUpAction -= LookUp;

                turnclass.eventDelegate -= EventCheck;
                dash.eventDelegate -= EventCheck;
                fallComp.eventDelegate -= EventCheck;


                InputManager.p1_OnKeyPressed -= CheckInputDown;
                break;
            case PlayerEnum.PlayerTwo:
                InputManager.P2_LeftStickRightAction -= MoveRight;
                InputManager.P2_LeftStickLeftAction -= MoveLeft;
                InputManager.P2_LeftTriggerDownAction -= dash.DashCheck;
                InputManager.P2_RightTiggerDownAction -= dash.DashCheck;
                InputManager.P2_LeftStickZeroAction -= FallStraight;
                InputManager.P2_LeftStickDownAction -= ForceDown;
                InputManager.P2_LeftStickUpAction -= LookUp;

                turnclass.eventDelegate -= EventCheck;
                dash.eventDelegate -= EventCheck;
                fallComp.eventDelegate -= EventCheck;

                InputManager.p2_OnKeyPressed -= CheckInputDown;
                break;
            case PlayerEnum.NotAssigned:
                Debug.Log("<color=red> MovementClass cant Find the PlayerEnum </color>");
                break;
        }
    }

    void CheckInputUp(KeyCode keyCode)
    {
        
    }
    void CheckInputDown(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Joystick1Button0 || keyCode == KeyCode.Joystick2Button0)
        {
            if(canMove)
                Jump();
        }
    }

    //////////////////////////////////////////////////
    ////////////////       Helper       //////////////
    //////////////////////////////////////////////////

    //      Public      //

    public bool IsOnWall()
    {
        if(fallComp.isOnWallLeft || fallComp.isOnWallRight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CanMove()
    {
        return canMove;
    }
    public void CantMove(float time)
    {
        canMove = false;
        Invoke("CanMove", time);
    }
    public void CantMove()
    {
        canMove = false;
    }
    public void CanMoveTrue()
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
        fallComp.isFalling = true;
    }
    /// <summary>
    /// Let the Character Jump | WARNING: Needed for a SpecialAttack DONT USE ENYWEHERE ELSE!
    /// </summary>
    public void SpecialJump()
    {
        this.rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, 0);
        StartCoroutine(ForceDownDelay());
        fallComp.isFalling = true;
    }
    /// <summary>
    /// Resets movement Values
    /// </summary>
    public void ResetValues()
    {
        jumps = 0;
        dash.currentDashes = 0;
        fallComp.isOnWallLeft = false;
        fallComp.isOnWallRight = false;
        rb.velocity = Vector3.zero;
    }
    /// <summary>
    /// Retruns if the player is falling or not
    /// </summary>
    /// <returns></returns>
    public bool IsFalling()
    {
        return fallComp.isFalling;
    }

    //      Private     //
    void EventCheck(EventState eventState)
    {
        switch(eventState)
        {
            case EventState.Landing:
                checkForIdeling = false;
                Invoke("CheckIdeling", 0.2f);
                break;
        }

        eventDelegate(eventState);
    }
    void CheckIdeling()
    {
        checkForIdeling = true;
    }

    void WallSlide()
    {
        if (myCharacter.isDisabled)
            return;

        if (fallComp.isOnWallRight)
        {
            if(!MyRayCast.RayCastHitRight(transform.position, wallDetectionLength))
            {
                fallComp.isOnWallRight = false;
            }
            dash.currentDashes = 0;
            eventDelegate(EventState.OnWall);
        }
        else if (fallComp.isOnWallLeft)
        {
            if (!MyRayCast.RayCastHitLeft(transform.position, wallDetectionLength))
            {
                fallComp.isOnWallLeft = false;
            }
            dash.currentDashes = 0;
            eventDelegate(EventState.OnWall);
        }
    }
    void Idle()
    {
        if (!checkForIdeling)
            return;

        if (MyEpsilon.Epsilon(lastPos.x, transform.position.x, 0.01f) && MyEpsilon.Epsilon(lastPos.y, transform.position.y, 0.01f))
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
        if (fallComp.isFalling)
            forcingDown = true;
    }
    void LookUp()
    {
        forcingDown = false;
    }
    void VelocityCheck()
    {
        if(rb.velocity.x > maxVelocityX)
        {
            rb.velocity = new Vector3(maxVelocityX, rb.velocity.y, 0);
        }
        if(rb.velocity.y > maxVelocityY)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxVelocityY, 0);
        }
    }

    //////////////////////////////////////////////////
    //////////////// Movement Functions //////////////
    //////////////////////////////////////////////////

    void MoveRight()
    {
        if (!this.canMove || !dash.canMove)
            return;

        if(!islookingToTheRight)
        {
            if(!IsFalling())
                eventDelegate(EventState.ChangeDirectionRight);
            islookingToTheRight = true;
        }
        fallComp.fallStraight = false;
        forcingDown = false;
        fallComp.isOnWallLeft = false;

        turnclass.LookRight(this.transform.rotation);
     
        if (MyRayCast.RayCastHitRight(transform.position, wallDetectionLength))
        {
            //WallSlide
            if (!fallComp.isOnWallLeft && !fallComp.isOnWallRight && fallComp.isFalling && !myCharacter.isDisabled)
            {
                fallComp.isOnWallRight = true;
                ResetJumps();
                if (eventDelegate != null)
                {
                    eventDelegate(EventState.ResetDashes);
                    eventDelegate(EventState.OnWall);
                }
                rb.velocity = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (fallComp.isFalling)
            {
                if (rb.velocity.x < maxInAirSpeed)
                {
                    rb.velocity += Vector3.right * inAirAccelerate;   //new Vector3(inAirSpeed, rb.velocity.y, 0);
                }
                else if (rb.velocity.x > maxInAirSpeed)
                {
                    rb.velocity = new Vector3(maxInAirSpeed, rb.velocity.y, 0);
                }
            }
            else
            {
                float winkel = 0;
                float vL = 0;
                float uL = 0;
                Vector3 u = Vector3.zero;
                Vector3 v = new Vector3(0, 1, 0);
                Vector3 dir = Vector3.zero;
                RaycastHit hit;
                // Cast a Ray to find the Normal
                if (MyRayCast.RayCastHitDown(transform.position, 1.5f, out hit))
                {
                    // switch x & y from the normal to get the direction
                    // inventier X from dir to get the right Direction
                    dir = new Vector3(hit.normal.y, hit.normal.x * -1, 0);
                    u = new Vector3(hit.normal.y, hit.normal.x * -1, 0);
                }
                // Get the Dot product from the direction and the gound ( Ground always (0,1,0))
                float dot = Vector3.Dot(u, v);

                // Get Lenght from u and v
                uL = Mathf.Sqrt(Mathf.Pow(u.x, 2) + Mathf.Pow(u.y, 2) + 0);
                vL = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + 0);

                // Get the Angle (Angle between two Vectors (u & v))
                winkel = Mathf.Acos(dot / (uL * vL));
                // winkel = amount between 0 and Pi
                // Change winkel to an amount between 0 and 360
                winkel = winkel * 180 / Mathf.PI;
                // flip the winkel 90°
                winkel -= 90;
                // change winkel above 90° so the result is the same on each side
                if (winkel > 90)
                {
                    winkel = 180 - winkel;
                }

                // if the angel is to hight, dont move forward
                if (Mathf.Abs(winkel) < maxWalkAngel)
                {
                    this.gameObject.transform.position += dir * movementSpeed * Time.deltaTime;
                    eventDelegate(EventState.Walking);
                }
            }
        }
    }
    void MoveLeft()
    {
        if (!this.canMove || !dash.canMove)
            return;

        if (islookingToTheRight)
        {
            if(!IsFalling())
                eventDelegate(EventState.ChangeDirectionLeft);
            islookingToTheRight = false;
            
        }

        fallComp.fallStraight = false;
        forcingDown = false;
        fallComp.isOnWallRight = false;

        turnclass.LookLeft(this.transform.rotation);
      
        if (MyRayCast.RayCastHitLeft(transform.position, wallDetectionLength))
        {
            //WallSlide
            if (!fallComp.isOnWallLeft && !fallComp.isOnWallRight && fallComp.isFalling && !myCharacter.isDisabled)
            {
                fallComp.isOnWallLeft = true;
                ResetJumps();
                if (eventDelegate != null)
                {
                    eventDelegate(EventState.ResetDashes);
                    eventDelegate(EventState.OnWall);
                }
                rb.velocity = new Vector3(0, 0, 0);
            }
        }
        else
        {
            if (fallComp.isFalling)
            {
                if (rb.velocity.x > -maxInAirSpeed)
                {
                    rb.velocity += Vector3.left * inAirAccelerate;  //new Vector3(-inAirSpeed, rb.velocity.y, 0);
                }
                else if (rb.velocity.x < -maxInAirSpeed)
                {
                    rb.velocity = new Vector3(-maxInAirSpeed, rb.velocity.y, 0);
                }
            }
            else
            {
                float winkel = 0;
                float vL = 0;
                float uL = 0;
                Vector3 u = Vector3.zero;
                Vector3 v = new Vector3(0,1,0);
                Vector3 dir = Vector3.zero;
                RaycastHit hit;
                // Cast a Ray to find the Normal
                if (MyRayCast.RayCastHitDown(transform.position, 1.5f, out hit))
                {
                    // switch x & y from the normal to get the direction
                    // inventier X from dir to get the right Direction
                    dir = new Vector3(hit.normal.y * -1, hit.normal.x, 0);
                    u = new Vector3(hit.normal.y * -1, hit.normal.x, 0);
                }
                // Get the Dot product from the direction and the gound ( Ground always (0,1,0))
                float dot = Vector3.Dot(u, v);

                // Get Lenght from u and v
                uL = Mathf.Sqrt(Mathf.Pow(u.x, 2) + Mathf.Pow(u.y, 2) + 0);
                vL = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + 0);

                // Get the Angle (Angle between two Vectors (u & v))
                winkel = Mathf.Acos(dot / (uL * vL));
                // winkel = amount between 0 and Pi
                // Change winkel to an amount between 0 and 360
                winkel = winkel * 180 / Mathf.PI;
                // flip the winkel 90°
                winkel -= 90;
                // change winkel above 90° so the result is the same on each side
                if (winkel > 90)
                {
                    winkel = 180 - winkel;
                }

                // if the angel is to hight, dont move forward
                if (Mathf.Abs(winkel) < maxWalkAngel)
                {
                    this.gameObject.transform.position += dir * movementSpeed * Time.deltaTime;
                    eventDelegate(EventState.Walking);
                }
            }
        }
    }
    void Jump()
    {
        if (myCharacter.isDisabled)
            return;
        Debug.Log(jumps);

        if (fallComp.isOnWallLeft && fallComp.isFalling)
        {
            if (this.gameObject.transform.position.x < 0)
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.right * jumpVelocity / 1.3f;
                jumps++;
            }
            else
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.right * jumpVelocity / 1.3f;
                jumps++;
            }
            turnclass.LookRight(this.transform.rotation);
            StartCoroutine(JumpCoolDown());
            StartCoroutine(ForceDownDelay());
            fallComp.isOnWallLeft = false;
            eventDelegate(EventState.WallJump);
        }
        else if (fallComp.isOnWallRight && fallComp.isFalling)
        {
            if (this.gameObject.transform.position.x < 0)
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.left * jumpVelocity / 1.3f;
                jumps++;
            }
            else
            {
                this.rb.velocity = Vector3.up * jumpVelocity + Vector3.left * jumpVelocity / 1.3f;
                jumps++;
            }
            turnclass.LookLeft(this.transform.rotation);
            StartCoroutine(JumpCoolDown());
            StartCoroutine(ForceDownDelay());
            fallComp.isOnWallRight = false;
            eventDelegate(EventState.WallJump);
        }
        else if (jumps < maxJumps)
        {
            this.rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, 0);
            jumps++;
            StartCoroutine(ForceDownDelay());
            if(fallComp.isFalling)
            {
                if(jumps == 2)
                    eventDelegate(EventState.JumpInAir);
                if(jumps >= 3)
                    eventDelegate(EventState.JumpInAir2);

            }
            else
            {
                checkForLanding = false;
                Invoke("CheckForLandingDelay", 0.1f);
                eventDelegate(EventState.JumpOnGround);
            }
        }
        fallComp.isFalling = true;
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