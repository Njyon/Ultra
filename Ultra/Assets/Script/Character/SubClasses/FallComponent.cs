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

    public FallComponent(bool forceDownEnabled, bool forcingDown, float fallSpeed, float wallDetectionLength, float maxFallVelocity, Transform transform, Rigidbody rb, Movement mov, Dash dash, MyCharacter myCharacter)
    {
        this.forceDownEnabled = forceDownEnabled;
        this.forcingDown = forcingDown;
        this.mov = mov;
        this.fallSpeed = fallSpeed;
        this.wallDetectionLength = wallDetectionLength;
        this.maxFallVelocity = maxFallVelocity;
        this.transform = transform;
        this.rb = rb;
        this.dash = dash;
        this.myCharacter = mov.GetComponent<MyCharacter>();
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
        //else if (isFalling && forcingDown && forceDownEnabled)
        //{
        //    rb.velocity += Vector3.up * Physics.gravity.y * (fallSpeed * 2) * Time.deltaTime;
        //}
        //else if (fallStraight && isFalling)
        //{
        //    if (rb.velocity.x < 0)
        //    {
        //        rb.velocity += new Vector3(1, 0, 0);
        //    }
        //    else if (rb.velocity.x > 0)
        //    {
        //        rb.velocity -= new Vector3(1, 0, 0);
        //    }
        //}
        else if (rb.velocity.y < 0)
        {
            isFalling = true;

            if (eventDelegate != null)
                eventDelegate(EventState.Fall);
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
            if (MyRayCast.RayCastHitDown(transform.position, 0.8f))
            {
                if (isFalling && rb.velocity.y <= 0)
                    if(eventDelegate != null)
                        eventDelegate(EventState.Landing);

                this.isOnWallLeft = false;
                this.isOnWallRight = false;
                dash.canDash = true;

                if (mov.jumps > 0)
                    mov.ResetJumps();
                if(eventDelegate != null)
                    eventDelegate(EventState.ResetDashes);
                if (dash.currentDashes > 0)
                    dash.currentDashes = 0;

                //myCharacter.isDisabled = false;

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

                if(winkel < 70)
                {
                    rb.velocity = Vector3.zero; // Deactive Velocity
                }

                return this.isFalling = false;
            }
            else
            {
                if (eventDelegate != null)
                    eventDelegate(EventState.isFalling);

                return isFalling = true;
            }

        }
        return false;
    }
    void FallingWallDetection()
    {
        if (myCharacter.isDisabled && !isFalling && !mov.CanMove())
            return;

        if(mov.islookingToTheRight)
        {
            if (MyRayCast.RayCastHitRight(transform.position, wallDetectionLength + 0.2f))
            {
                //WallSlide
                if (!isOnWallLeft && !isOnWallRight)
                {
                    isOnWallRight = true;
                    mov.ResetJumps();
                    if (eventDelegate != null)
                    {
                        eventDelegate(EventState.ResetDashes);
                        eventDelegate(EventState.OnWall);
                    }
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
            if (MyRayCast.RayCastHitLeft(transform.position, wallDetectionLength + 0.2f))
            {
                //WallSlide
                if (!isOnWallLeft && !isOnWallRight)
                {
                    isOnWallLeft = true;
                    mov.ResetJumps();
                    if (eventDelegate != null)
                    {
                        eventDelegate(EventState.ResetDashes);
                        eventDelegate(EventState.OnWall);
                    }
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
