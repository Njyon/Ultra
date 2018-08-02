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

    int emissionID;
    int colorID;
    public Renderer rendererCloth;
    [ColorUsageAttribute(false, true)] [SerializeField] Color EndColor;

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
    public MyCharacter.EventDelegate eventDelegate;

    private void Start()
    {
        canDash = true;
        emissionID = Shader.PropertyToID("_EmissionColor");
        colorID = Shader.PropertyToID("_Color");
    }

    public void DashCheck()
    {
        if (myCharacter.isDisabled)
            return;
        if (!canDash)
            return;

        Dodge();

        #region deprecated
        //switch (playerEnum)
        //{
        //    case PlayerEnum.PlayerOne:

        //        if (Input.GetAxisRaw("P1_Horizontal") == 1)
        //        {
        //            //Right
        //            DirectionDash(true);
        //        }
        //        else if (Input.GetAxisRaw("P1_Horizontal") == -1)
        //        {
        //            //Left
        //            DirectionDash(false);
        //        }
        //        else
        //        {
        //            DashStanding();
        //        }
        //        break;
        //    case PlayerEnum.PlayerTwo:
        //        if (Input.GetAxisRaw("P2_Horizontal") == 1)
        //        {
        //            //Right
        //            DirectionDash(true);

        //        }
        //        else if (Input.GetAxisRaw("P2_Horizontal") == -1)
        //        {
        //            //Left
        //            DirectionDash(false);
        //        }
        //        else
        //        {
        //            DashStanding();
        //        }
        //        break;
        //    case PlayerEnum.NotAssigned:
        //        Debug.Log("<color=red> MovementClass -> DashCheck() cant Find the PlayerEnum </color>");
        //        break;
        //}
#endregion
    }

    void Dodge()
    {
        if (this.GetComponent<MyCharacter>().isAttacking)
            return;
        if (!canDash)
            return;

        StartCoroutine(DashCoolDown(dashCoolDown));
        StartCoroutine(DogeTime(dodgeNoDmgTime));
    }
    public void DashStanding()
    {
        if (canDash)
        {
            myCharacter.canGetDamaged = false;
            //canDash = false;
            canMove = false;
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            StartCoroutine(DashCoolDown(dashCoolDown));
            StartCoroutine(StandingDogeTime(dodgeNoDmgTime));
            
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
        if (currentDashes <= maxDashes)
        {
            if (isDashing)
            {
                if (canDash)
                {
                    if (isFalling)
                    {
                        if (this.GetComponent<MyCharacter>().isAttacking)
                            return;

                        //StartCoroutine(DogeTime(dodgeNoDmgTime));
                        //StartCoroutine(DashCoolDown(dashCoolDown));
                    }
                    else
                    {
                        currentDashes = 0;
                        //StartCoroutine(DogeTime(dodgeNoDmgTime));
                        //StartCoroutine(DashCoolDown(dashCoolDown));
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
        canDash = false;
        yield return new WaitForSeconds(time);
        canDash = true;
    }
    IEnumerator DogeTime(float time)
    {
        eventDelegate(EventState.Dodge);
        myCharacter.canGetDamaged = false;
        Color playerColor = Color.white;
        Color clothColor = GetComponent<MyCharacter>().clothColor;
        Color swordColor = GetComponent<MyCharacter>().swordColor;
        PlayerEnum playerEnum = GetComponent<MyCharacter>().playerEnum;

        rendererCloth.materials[0].SetColor(emissionID, EndColor);
        rendererCloth.materials[1].SetColor(emissionID, EndColor);

        switch(playerEnum)
        {
            case PlayerEnum.PlayerOne:
                playerColor = PlayerInfoManager.playerOne.color;
                break;
            case PlayerEnum.PlayerTwo:
                playerColor = PlayerInfoManager.playerTwo.color;
                break;
        }
        while (time > 0)
        {
            //TODO: LERP Color

            time -= Time.deltaTime;
            yield return null;
        }
        myCharacter.canGetDamaged = true;
        GetComponent<MyCharacter>().ReturnColorToNoraml();
        eventDelegate(EventState.DodgeEnd);
        yield return null;
    }
}