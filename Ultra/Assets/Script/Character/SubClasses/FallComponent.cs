using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallComponent
{
    public bool fallStraight = true;

    bool forceDownEnabled;
    bool forcingDown;
    bool isOnWallLeft;
    bool isOnWallRight;
    bool isFalling;
    bool islookingToTheRight;
    float wallDetectionLength;
    float fallSpeed;
    float maxFallVelocity;
    Transform transform;
    Rigidbody rb;
    Movement mov;
    Dash dash;
    MyCharacter myCharacter;

    //Delegate
    public delegate void EventDelegate(EventState eventState);
    public EventDelegate eventDelegate;

    public FallComponent(bool forceDownEnabled, bool forcingDown, bool isOnWallLeft, bool isOnWallRight, bool isFalling, bool islookingToTheRight, float fallSpeed, float wallDetectionLength, float maxFallVelocity, Transform transform, Rigidbody rb, Movement mov, Dash dash, MyCharacter myCharacter)
    {
        this.forceDownEnabled = forceDownEnabled;
        this.forcingDown = forcingDown;
        this.isOnWallLeft = isOnWallLeft;
        this.isOnWallRight = isOnWallRight;
        this.isFalling = isFalling;
        this.islookingToTheRight = islookingToTheRight;
        this.mov = mov;
        this.fallSpeed = fallSpeed;
        this.wallDetectionLength = wallDetectionLength;
        this.maxFallVelocity = maxFallVelocity;
        this.transform = transform;
        this.rb = rb;
        this.dash = dash;
        this.myCharacter = myCharacter;
    }

    public void Falling()
    {
        if (dash.isDashing)
            return;

        if (this.isOnWallLeft || this.isOnWallRight)
        {
            if (myCharacter.isDisabled)
            {
                this.isOnWallLeft = false;
                this.isOnWallRight = false;
                return;
            }
            if (mov.jumps == 0)
                rb.velocity = Vector3.down * 2;
        }
        else if (isFalling && forcingDown && forceDownEnabled)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed * 2) * Time.deltaTime;
        }
        else if (fallStraight && isFalling)
        {
            if (rb.velocity.x < 0)
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
    } // Change Gravity for Jump Balancing!  Edit -> Project Setting -> Physics -> Gravity Y
    public bool Grounded()
    {
        if (!dash.isDashing)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, new Vector3(0, -transform.position.y, 0), out hit, 1, 9, QueryTriggerInteraction.Ignore))
            {
                if (isFalling)
                    eventDelegate(EventState.Landing);

                this.isOnWallLeft = false;
                this.isOnWallRight = false;
                dash.canDash = true;

                if (mov.jumps > 0)
                    mov.ResetJumps();
                if (dash.currentDashes > 0)
                    dash.currentDashes = 0;
                if (myCharacter.isDisabled)
                    myCharacter.isDisabled = false;
                
                return this.isFalling = false;
            }
            else
            {
                return isFalling = true;
            }
        }
        return true;
    }
    void FallingWallDetection()
    {
        if (myCharacter.isDisabled)
            return;

        RaycastHit hit;

        if (this.transform.position.x < 0)
        {
            if (islookingToTheRight)
            {
                if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength + 0.1f, 9, QueryTriggerInteraction.Ignore))
                {
                    //WallSlide
                    if (!this.isOnWallLeft && !this.isOnWallRight)
                    {
                        this.isOnWallRight = true;
                        mov.ResetJumps();
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                }
            }
            else
            {
                if (Physics.Raycast(this.transform.position, new Vector3(this.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength + 0.1f, 9, QueryTriggerInteraction.Ignore))
                {
                    //WallSlide
                    if (!this.isOnWallLeft && !this.isOnWallRight)
                    {
                        this.isOnWallLeft = true;
                        mov.ResetJumps();
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                }
            }
        }
        else
        {
            if (islookingToTheRight)
            {
                if (Physics.Raycast(this.transform.position, new Vector3(this.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength + 0.1f, 9, QueryTriggerInteraction.Ignore))
                {
                    //WallSlide
                    if (!this.isOnWallLeft && !this.isOnWallRight)
                    {
                        this.isOnWallRight = true;
                        mov.ResetJumps();
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                }
            }
            else
            {
                if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.localPosition.x, 0, 0), out hit, this.wallDetectionLength + 0.1f, 9, QueryTriggerInteraction.Ignore))
                {
                    //WallSlide
                    if (!this.isOnWallLeft && !this.isOnWallRight)
                    {
                        this.isOnWallLeft = true;
                        mov.ResetJumps();
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                }
            }
        }
    }
}
