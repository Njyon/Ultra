using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Dash Variables")]
    // GamePlay Vars
    public int maxDashes;
    public float dashLength;
    public float dashSpeed;
    // Times
    public float dodgeNoDmgTime;
    public float dodgeCoolDown;
    public float dashCoolDown;
    // Curves
    public AnimationCurve dashCurve;

    // Character Vars
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public int currentDashes = 0;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public MyCharacter myCharacter;
    [HideInInspector] public PlayerEnum playerEnum;

    // Dash Vars
    Vector3 dashStartPoint;
    Vector3 dashEndPoint;

    //Delegate
    public delegate void EventDelegate(EventState eventState);
    public EventDelegate eventDelegate;

    public void DashCheck()
    {
        Debug.Log("LOL");

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
    public void DashStanding()
    {
        if (canDash)
        {
            myCharacter.canGetDamaged = false;
            canDash = false;
            canMove = false;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            StartCoroutine(DashCoolDown(dashCoolDown));
            StartCoroutine(StandingDogeTime(dodgeNoDmgTime));

            if (eventDelegate != null)
                eventDelegate(EventState.Dodge);
        }
    }
    public void DirectionDash(bool directionRight)
    {
        if (!canDash)
            return;

        isDashing = true;
        currentDashes++;

        if (directionRight)
        {
            dashEndPoint = MyRayCast.RaycastRight(transform.position, dashLength);
            dashStartPoint = transform.position;
        }
        else
        {
            dashEndPoint = MyRayCast.RaycastLeft(transform.position, dashLength);
            dashStartPoint = transform.position;
        }
    }

    float travel = 0;
    public void Dashing(bool isFalling)
    {
        Debug.Log(currentDashes);
        if (currentDashes <= maxDashes)
        {
            if (isDashing)
            {
                if (canDash)
                {
                    canDash = false;
                    if (isFalling)
                    {
                        StartCoroutine(DogeTime(dodgeNoDmgTime));
                        StartCoroutine(DashCoolDown(dashCoolDown));

                        if (eventDelegate != null)
                            eventDelegate(EventState.Dodge);
                    }
                    else
                    {
                        currentDashes = 0;
                        StartCoroutine(DashCoolDown(dashCoolDown));

                        if (eventDelegate != null)
                            eventDelegate(EventState.Dash);
                    }
                }
                if (MyEpsilon.Epsilon(transform.position.x, dashEndPoint.x, 0.5f))
                {
                    rb.useGravity = true;
                    isDashing = false;
                    travel = 0;
                    
                    return;
                }
                travel += dashSpeed * Time.deltaTime;
                float curvePercent = dashCurve.Evaluate(travel);
                this.transform.position = Vector3.LerpUnclamped(dashStartPoint, dashEndPoint, curvePercent);
            }
        }
    }

    IEnumerator StandingDogeTime(float time)
    {
        myCharacter.canGetDamaged = false;
        yield return new WaitForSeconds(time);
        myCharacter.canGetDamaged = true;
        rb.useGravity = true;
        isDashing = false;
        canMove = true;
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
}