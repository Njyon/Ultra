using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav2 : MyCharacter
{
    [Header("Dash Variablen")]
    [SerializeField] int damage;
    [SerializeField] float dashSpeed;
    [SerializeField] float attackLength;
    [Header("Dash Times")]
    [SerializeField] float timeTillAtackStarts;
    [SerializeField] float attackTime;
    [SerializeField] float coolDownTime;
    [Header("Dash Curve")]
    [SerializeField] AnimationCurve curve;

    Direction direction;

    bool isUsingAbility = false;
    Vector3 dashStartPosition;
    Vector3 dashEndPosition;
    Ability abilities;

    private void Start()
    {
        DefineAbilities();
        
        switch(playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.P1_XButtonDirectionAction += DirctionCheck;
                break;
            case PlayerEnum.PlayerTwo:
                InputManager.P2_XButtonDirectionAction += DirctionCheck;
                break;
        }
        
        XAttackNormalAction += LightAttack;
    }
    private void OnDisable()
    {
        XAttackNormalAction -= LightAttack;

        switch (playerEnum)
        {
            case PlayerEnum.PlayerOne:
                InputManager.P1_XButtonDirectionAction -= DirctionCheck;
                break;
            case PlayerEnum.PlayerTwo:
                InputManager.P2_XButtonDirectionAction -= DirctionCheck;
                break;
        }
    }

    /// <summary>
    /// Check the Input in witch direction the player should dash
    /// </summary>
    /// <param name="direction"></param>
    void DirctionCheck(Direction direction)
    {
        if (isUsingAbility)
            return;

        this.direction = direction;
        switch (direction)
        {
            case Direction.Right:
                LightAttackRight();
                break;
            case Direction.Left:
                LightAttackLeft();
                break;
            case Direction.Up:
                LightAttackUp();
                break;
            case Direction.Down:
                LightAttackDown();
                break;
            case Direction.RightUpAngel:
                LightAttackRightUp();
                break;
            case Direction.RightDownAngel:
                LightAttackRightDown();
                break;
            case Direction.LeftUpAngel:
                LightAttackLefttUp();
                break;
            case Direction.LeftDownAngel:
                LightAttackLeftDown();
                break;
        }
    }
    
    float travel = 0;
    /// <summary>
    /// Define all Abilitys
    /// </summary>
    void DefineAbilities()
    {
        #region Ability Definiton
        abilities = new Ability(
            "Light Attack",
            "A Light Attack in view Direktion, its fast and does some Damage",
            damage,
            timeTillAtackStarts,
            attackTime,
            coolDownTime,
            false
            );
        #endregion

        #region Ability's

        #region Ability X
        // Standert "X" Attack with left, right or without direction Input
        abilities.onAbilityStart = () => 
        {
            if (currentDashes <= maxDashes)
                currentDashes++;
            else
            {
                abilities.Cancel();
                return;
            }

            isAttacking = true;
            isUsingAbility = true;
            StartCoroutine(DashCooldown());
            IsAttacking();
            
            switch (direction)
            {
                case Direction.Right:
                case Direction.Left:
                    eventDelegate(EventState.AttackSide);
                    
                    // Find dash End and Start point
                    if (IsLookingRight())
                    {
                        dashEndPosition = MyRayCast.RaycastRight(transform.position, attackLength);
                        dashStartPosition = transform.position;
                        Instantiate(pD.attackRight, new Vector3(transform.position.x - 0.2f, transform.position.y, 0), Quaternion.identity, this.transform);
                    }
                    else
                    {
                        dashEndPosition = MyRayCast.RaycastLeft(transform.position, attackLength);
                        dashStartPosition = transform.position;
                        Instantiate(pD.attackLeft, new Vector3(transform.position.x - 0.2f, transform.position.y, 0), Quaternion.identity, this.transform);
                    }
                    break;
                case Direction.Up:
                    eventDelegate(EventState.AttackUp);

                    // Find dash End and Start point
                    dashEndPosition = MyRayCast.RaycastUp(transform.position, attackLength);
                    dashStartPosition = transform.position;
                    Instantiate(pD.attackUp, new Vector3(transform.position.x - 0.2f, transform.position.y, 0), Quaternion.identity, this.transform);
                    break;
                case Direction.Down:
                    eventDelegate(EventState.AttackDown);

                    // Find dash End and Start point
                    dashEndPosition = MyRayCast.RaycastDown(transform.position, attackLength);
                    dashStartPosition = transform.position;
                    Instantiate(pD.attackDown, new Vector3(transform.position.x - 0.2f, transform.position.y, 0), Quaternion.identity, this.transform);
                    break;
                case Direction.RightDownAngel:
                case Direction.LeftDownAngel:
                    eventDelegate(EventState.AttackDownAngled);

                    if (IsLookingRight())
                    {
                        Instantiate(pD.attackRightDown, new Vector3(transform.position.x - 0.2f, transform.position.y, 0), Quaternion.identity, this.transform);
                    }
                    else
                    {
                        Instantiate(pD.attackLeftDown, new Vector3(transform.position.x - 0.2f, transform.position.y, 0), Quaternion.identity, this.transform);
                    }
                    // Find dash End and Start point
                    dashEndPosition = MyRayCast.RayCastDownAngeled(transform, attackLength, IsLookingRight());
                    dashStartPosition = transform.position;
                    break;
                case Direction.RightUpAngel:
                case Direction.LeftUpAngel:
                    eventDelegate(EventState.AttackUpAngled);

                    if (IsLookingRight())
                    {
                        Instantiate(pD.attackRightUp, new Vector3(transform.position.x - 0.2f, transform.position.y, 0), Quaternion.identity, this.transform);
                    }
                    else
                    {
                        Instantiate(pD.attackLeftUp, new Vector3(transform.position.x - 0.2f, transform.position.y, 0), Quaternion.identity, this.transform);
                    }
                    // Find dash End and Start point
                    dashEndPosition = MyRayCast.RayCastUpAngeled(transform, attackLength, IsLookingRight());
                    dashStartPosition = transform.position;
                    break;
            }

            //PartilceSlash();
            rb.useGravity = false;
        };
        abilities.onAbilityUpdate = () =>
        {
            // Dash Ends without Hit
            switch (direction)
            {
                case Direction.Right:
                case Direction.Left:
                    if (MyEpsilon.Epsilon(transform.position.x, dashEndPosition.x, 0.5f))
                    {
                        abilities.End();
                        return;
                    }
                    break;
                case Direction.Down:
                case Direction.Up:
                    if (MyEpsilon.Epsilon(transform.position.y, dashEndPosition.y, 0.5f))
                    {
                        abilities.End();
                        return;
                    }
                    break;
                case Direction.RightDownAngel:
                case Direction.RightUpAngel:
                case Direction.LeftUpAngel:
                case Direction.LeftDownAngel:
                    if (MyEpsilon.Epsilon(transform.position.x, dashEndPosition.x, 0.5f) && MyEpsilon.Epsilon(transform.position.y, dashEndPosition.y, 0.5f))
                    {
                        abilities.End();
                        return;
                    }
                    break;
            }
            // Dash Ends with hitting the enemy 
            if(!abilities.hitObject)
            {
                switch(direction)
                {
                    case Direction.Right:
                    case Direction.Left:
                        if(xNormalHitBox)
                        {
                            DoDamage();
                            return;
                        }
                        break;
                    case Direction.Up:
                        if(xUpHitBox)
                        {
                            DoDamage();
                            return;
                        }
                        break;
                    case Direction.Down:
                        if(xDownHitBox)
                        {
                            DoDamage();
                            return;
                        }
                        break;
                    case Direction.RightDownAngel:
                    case Direction.LeftDownAngel:
                        if (xDownHitAngeldBox)
                        {
                            DoDamage();
                            return;
                        }
                        break;
                    case Direction.RightUpAngel:
                    case Direction.LeftUpAngel:
                        if (xUpHitAngeldBox)
                        {
                            DoDamage();
                            return;
                        }
                        break;
                }
            }
            // Lerp the Dash
            travel += dashSpeed * Time.deltaTime;
            float curvePercent = curve.Evaluate(travel);
            this.transform.position = Vector3.LerpUnclamped(dashStartPosition, dashEndPosition, curvePercent);
        };
        abilities.onAbilityCancel = () =>
        {
            isAttacking = false;
            rb.useGravity = true;
            EndAttacking();
            travel = 0;
        };
        abilities.onAbilityEnd = () =>
        {
            isAttacking = false;
            if (abilities.hitObject)
            {
                eventDelegate(EventState.AttackHit);
                Invoke("AnimHitSwitch", 0.1f);
            }
            else
            {
                eventDelegate(EventState.AttackEnd);
            }

            rb.velocity = Vector3.zero;
            rb.useGravity = false;
            travel = 0;
        };
        abilities.onAbilityReady = () => 
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
            EndAttacking();
        };
        #endregion

        #region Copie
        //abilities[2].onAbilityStart = () => { };
        //abilities[2].onAbilityUpdate = () => { };
        //abilities[2].onAbilityCancel = () => { };
        //abilities[2].onAbilityEnd = () => { };
        //abilities[2].onAbilityReady = () => { };
        #endregion

        #endregion
    }

    void AnimHitSwitch()
    {
        animator.SetBool(animIsHiting, false);
    }

    /// <summary>
    /// Damage the enemy and Kick him away if he is not dodging
    /// </summary>
    void DoDamage()
    {
        // Hitted the enemy
        abilities.hitObject = true;

        // if enemy is dogding dont attack
        if (!enemyCharacter.canGetDamaged)
        {
            enemyCharacter.Combo(ComboState.Dodge);
            // Count All Dodges for the ENdScreen
            enemyCharacter.dodgeAction(enemyCharacter.playerEnum);
            return;
        }
        if(enemyCharacter.isAttacking)
        {
            StartCoroutine(FreezCharacter(false, freezTimeHit, false));
            parryDelegate();
            return;
        }

        StartCoroutine(FreezCharacter(false, freezTimeHit, true));
        if (!enemyCharacter.isDisabled)
        {
            // Count up the enemy Perzent (Damaged Amount)
            enemyCharacter.Damage(abilities.GetDamage());
            // Kick the Enemy away from you
            switch(direction)
            {
                case Direction.RightUpAngel:
                case Direction.LeftUpAngel:
                case Direction.Up:
                    enemyCharacter.KickUp(transform.position, false);
                    break;
                case Direction.Down:
                case Direction.Left:
                case Direction.LeftDownAngel:
                case Direction.Right:
                case Direction.RightDownAngel:
                    enemyCharacter.KickAway(transform.position, false);
                    break;
            }

            // Update the Combo
            Combo(ComboState.Hit);
        }
        else
        {
            // Kick the Enemy away from you
            switch (direction)
            {
                case Direction.RightUpAngel:
                case Direction.LeftUpAngel:
                case Direction.Up:
                    enemyCharacter.KickUp(transform.position, false);
                    break;
                case Direction.Down:
                case Direction.Left:
                case Direction.LeftDownAngel:
                case Direction.Right:
                case Direction.RightDownAngel:
                    enemyCharacter.KickAway(transform.position, false);
                    break;
            }

            // Update the Combo
            Combo(ComboState.HitStrak);
        }
        //eventDelegate(EventState.Slash);
        // Freez the Cam // in the Moment deactive
        //freezCamAction();
        // End the Dash
        abilities.End();
    }

    void Update()
    {
        Bounce();
        abilities.Update();
        //if (Input.GetKeyDown(KeyCode.Escape))
        //    UnityEditor.EditorApplication.isPaused = true;
        //if (Input.GetKeyDown(KeyCode.KeypadEnter))
        //    UnityEditor.EditorApplication.isPaused = false;
    }

    
    void LightAttack()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled && !isDodgeing)
        {
            abilities.Activate();
        }
    }
    void LightAttackRight()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled && !isDodgeing)
        {
            LookRight();
            abilities.Activate();
        }
    }
    void LightAttackLeft()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled && !isDodgeing)
        {
            LookLeft();
            abilities.Activate();
        }
    }
    void LightAttackUp()
    {
        if (!isUsingAbility && IsFalling() && !isDisabled && !isDodgeing)
            abilities.Activate();
    }
    void LightAttackDown()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled && !isDodgeing)
            abilities.Activate();
    }
    void LightAttackRightUp()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled && !isDodgeing)
        {
            LookRight();
            abilities.Activate();
        }
    }
    void LightAttackRightDown()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled && !isDodgeing)
        {
            LookRight();
            abilities.Activate();
        }
    }
    void LightAttackLefttUp()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled && !isDodgeing)
        {
            LookLeft();
            abilities.Activate();
        }
    }
    void LightAttackLeftDown()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled && !isDodgeing)
        {
            LookLeft();
            abilities.Activate();
        }
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCoolDown);
        isUsingAbility = false;
    }
}
