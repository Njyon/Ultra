using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallComponent
{
    public bool fallStraight = true;
    public bool isOnWallLeft = false;
    public bool isOnWallRight = false;
    public bool isFalling = true;

    bool forceDownEnabled;
    bool forcingDown;
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

    public FallComponent(bool forceDownEnabled, bool forcingDown, bool islookingToTheRight, float fallSpeed, float wallDetectionLength, float maxFallVelocity, Transform transform, Rigidbody rb, Movement mov, Dash dash, MyCharacter myCharacter)
    {
        this.forceDownEnabled = forceDownEnabled;
        this.forcingDown = forcingDown;
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
        if (dash.isDashing || !isFalling)
            return;

        if (isOnWallLeft || isOnWallRight)
        {
            if (myCharacter.isDisabled)
            {
                this.isOnWallLeft = false;
                this.isOnWallRight = false;
                return;
            }
            FallingWallDetection();
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

            if (eventDelegate != null)
                eventDelegate(EventState.Falling);
            if (rb.velocity.y > maxFallVelocity)
                rb.velocity = new Vector3(rb.velocity.x, maxFallVelocity, 0);

            //rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed - 1) * Time.deltaTime;
            FallingWallDetection();
        }
        else if (rb.velocity.y > 0)
        {
            if (!isFalling)
                isFalling = true;
        }
    } // Change Gravity for Jump Balancing!  Edit -> Project Setting -> Physics -> Gravity Y
    public bool Grounded()
    {
        if (!dash.isDashing && !myCharacter.isDisabled)
        {
            if (MyRayCast.RayCastHitDown(transform.position, 1))
            {
                if (isFalling && rb.velocity.y <= 0)
                    if(eventDelegate != null)
                        eventDelegate(EventState.Landing);

                this.isOnWallLeft = false;
                this.isOnWallRight = false;
                dash.canDash = true;

                if (mov.jumps > 0)
                    mov.ResetJumps();
                if (dash.currentDashes > 0)
                    dash.currentDashes = 0;

                //myCharacter.isDisabled = false;
                
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
        if (myCharacter.isDisabled && !isFalling && !mov.CanMove())
            return;

        if(islookingToTheRight)
        {
            if (MyRayCast.RayCastHitRight(transform.position, wallDetectionLength))
            {
                //WallSlide
                if (!isOnWallLeft && !isOnWallRight)
                {
                    isOnWallRight = true;
                    mov.ResetJumps();
                }
            }
            else
            {
                isOnWallRight = false;
                isOnWallLeft = false;
            } 
        }
        else
        {
            if (MyRayCast.RayCastHitLeft(transform.position, wallDetectionLength))
            {
                //WallSlide
                if (!isOnWallLeft && !isOnWallRight)
                {
                    isOnWallLeft = true;
                    mov.ResetJumps();
                }
            }
            else
            {
                isOnWallRight = false;
                isOnWallLeft = false;
            }
        }
    }
}
