using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallComponent
{
    bool fallStraight = true;
    bool forcingDown = false;
    bool forceDownEnabled = true;

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
            turnclass.LookRight(this.transform.rotation);
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
            turnclass.LookLeft(this.transform.rotation);
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

    void Grounded()
    {
        RaycastHit hit;

        if (Physics.Raycast(this.gameObject.transform.position, new Vector3(0, -this.transform.position.y, 0), out hit, 1, 9, QueryTriggerInteraction.Ignore))
        {
            if (isFalling)
            {
                this.isOnWallLeft = false;
                this.isOnWallRight = false;
                this.isFalling = false;
                dash.canDash = true;
                jumpState = JumpState.OnGround;

                if (jumps > 0)
                    ResetJumps();
                if (dash.currentDashes > 0)
                    dash.currentDashes = 0;
                if (myCharacter.isDisabled)
                    myCharacter.isDisabled = false;
            }
        }
        else
        {
            isFalling = true;
        }
    }
}
