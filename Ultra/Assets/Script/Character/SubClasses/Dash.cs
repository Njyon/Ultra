using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [Header("Dash Variables")]
    public int maxDashes;
    public float dashCoolDown;
    public float dogeTime;
    public float dashLength;
    public float standingDogeTime;
    public float dashSpeed;

    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool canDash = true;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public int currentDashes = 0;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public MyCharacter myCharacter;
    [HideInInspector] public PlayerEnum playerEnum;

    float currentDashTime;
    float journeyLength;
    float dashWallDistance = 0.6f;
    float wallDetectionLength = 0.6f;
    Vector3 dashEndPoint;

    //Delegate
    public delegate void EventDelegate(EventState eventState);
    public EventDelegate eventDelegate;

    public void DashCheck()
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
            StartCoroutine(StandingDogeTime(standingDogeTime));
            StartCoroutine(DogeTime(dogeTime));
        }
    }
    public void DirectionDash(bool directionRight)
    {
        if (!canDash)
            return;

        currentDashTime = standingDogeTime;
        isDashing = true;
        RaycastHit hit;
        StartCoroutine(StandingDogeTime(standingDogeTime));
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

    float travel = 0;
    public void Dashing(bool isFalling, Vector3 position)
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
                        StartCoroutine(DogeTime(dashCoolDown));
                        StartCoroutine(DashCoolDown(dashCoolDown));
                        eventDelegate(EventState.Dodge);
                    }
                    else
                    {
                        currentDashes = 0;
                        StartCoroutine(DashCoolDown(dogeTime));
                        eventDelegate(EventState.Dash);
                    }
                }
                if (MyEpsilon.Epsilon(position.x, dashEndPoint.x, 0.5f))
                {
                    rb.useGravity = true;
                    isDashing = false;
                    travel = 0;

                    return;
                }

                travel += dashSpeed * Time.deltaTime;

                this.transform.position = Vector3.Lerp(position, dashEndPoint, travel);
            }
        }
    }

    IEnumerator StandingDogeTime(float time)
    {
        yield return new WaitForSeconds(time);
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