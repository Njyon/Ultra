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

    GameObject lastBounceObj;

    bool isUsingAbility = false;
    Vector3 dashStartPosition;
    Vector3 dashEndPosition;
    Ability[] abilities = new Ability[5];

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

    void DirctionCheck(Direction direction)
    {
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
    void DefineAbilities()
    {
        #region Ability Definiton
        // X Normal direction = Look direction
        abilities[0] = new Ability(
            "Light Attack",
            "A Light Attack in view Direktion, its fast and does some Damage",
            damage,
            timeTillAtackStarts,
            attackTime,
            coolDownTime,
            false
            );

        // X Up
        abilities[1] = new Ability(
            "Light Attack",
            "A Light Attack in Up",
            damage,
            timeTillAtackStarts,
            attackTime,
            coolDownTime,
            false
            );

        // X Down
        abilities[2] = new Ability(
            "Light Attack",
            "A Light Attack in Down",
            damage,
            timeTillAtackStarts,
            attackTime,
            coolDownTime,
            false
            );

        // X Down Angel
        abilities[3] = new Ability(
            "Light Attack",
            "A Light Attack in Down",
            damage,
            timeTillAtackStarts,
            attackTime,
            coolDownTime,
            false
            );

        // X Up Angel
        abilities[4] = new Ability(
            "Light Attack",
            "A Light Attack in Down",
            damage,
            timeTillAtackStarts,
            attackTime,
            coolDownTime,
            false
            );
        #endregion

        #region Ability's

        #region Ability Normal Dash
        // Standert "X" Attack with left, right or without direction Input
        abilities[0].onAbilityStart = () => 
        {
            isUsingAbility = true;
            IsAttacking();

            trail.SetActive(true);
            
            // Find dash End and Start point
            if (IsLookingRight())
            {
                dashEndPosition = MyRayCast.RaycastRight(transform.position, attackLength);
                dashStartPosition = transform.position;
            }
            else
            {
                dashEndPosition = MyRayCast.RaycastLeft(transform.position, attackLength);
                dashStartPosition = transform.position;
            }

            PartilceSlash();
            eventDelegate(EventState.LightHit);
            rb.useGravity = false;
        };
        abilities[0].onAbilityUpdate = () => 
        {
            // Dash Ends without Hit
            if (MyEpsilon.Epsilon(transform.position.x, dashEndPosition.x, 0.5f))
            {
                abilities[0].End();
                return;
            }
            // Dash Ends with hitting the enemy 
            if(!abilities[0].hitObject && xNormalHitBox)
            {
                abilities[0].hitObject = true;
                enemyCharacter.Damage(abilities[0].GetDamage());
                enemyCharacter.KickAway(transform.position, false);

                freezCamAction();
                abilities[0].End();
                return;
            }
            // Lerp the Dash
            travel += dashSpeed * Time.deltaTime;
            float curvePercent = curve.Evaluate(travel);
            this.transform.position = Vector3.LerpUnclamped(dashStartPosition, dashEndPosition, curvePercent);
        };
        abilities[0].onAbilityCancel = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[0].onAbilityEnd = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[0].onAbilityReady = () => { };
        #endregion
        #region Ability Up Dash
        // "X" Attack Up
        abilities[1].onAbilityStart = () =>
        {
            isUsingAbility = true;
            IsAttacking();

            // Find dash End and Start point
            dashEndPosition = MyRayCast.RaycastUp(transform.position, attackLength);
            dashStartPosition = transform.position;

            trail.SetActive(true);

            PartilceSlash();
            eventDelegate(EventState.LightHit);
            rb.useGravity = false;
        };
        abilities[1].onAbilityUpdate = () =>
        {
            // Dash Ends without Hit
            if (MyEpsilon.Epsilon(transform.position.y, dashEndPosition.y, 0.5f))
            {
                abilities[1].End();
                return;
            }
            // Dash Ends with hitting the enemy 
            if (!abilities[1].hitObject && xNormalHitBox)
            {
                abilities[1].hitObject = true;
                enemyCharacter.Damage(abilities[1].GetDamage());
                enemyCharacter.KickAway(transform.position, false);

                freezCamAction();
                abilities[1].End();
                return;
            }
            // Lerp the Dash
            travel += dashSpeed * Time.deltaTime;
            float curvePercent = curve.Evaluate(travel);
            this.transform.position = Vector3.LerpUnclamped(dashStartPosition, dashEndPosition, curvePercent);
        };
        abilities[1].onAbilityCancel = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[1].onAbilityEnd = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[1].onAbilityReady = () => { };
        #endregion
        #region Ability Down Dash
        // "X" Attack Up
        abilities[2].onAbilityStart = () =>
        {
            isUsingAbility = true;
            IsAttacking();

            // Find dash End and Start point
            dashEndPosition = MyRayCast.RaycastDown(transform.position, attackLength);
            dashStartPosition = transform.position;

            trail.SetActive(true);

            PartilceSlash();
            eventDelegate(EventState.LightHit);
            rb.useGravity = false;
        };
        abilities[2].onAbilityUpdate = () =>
        {
            // Dash Ends without Hit
            if (MyEpsilon.Epsilon(transform.position.y, dashEndPosition.y, 0.5f))
            {
                abilities[2].End();
                return;
            }
            // Dash Ends with hitting the enemy 
            if (!abilities[2].hitObject && xNormalHitBox)
            {
                abilities[2].hitObject = true;
                enemyCharacter.Damage(abilities[2].GetDamage());
                enemyCharacter.KickAway(transform.position, false);

                freezCamAction();
                abilities[2].End();
                return;
            }
            // Lerp the Dash
            travel += dashSpeed * Time.deltaTime;
            float curvePercent = curve.Evaluate(travel);
            this.transform.position = Vector3.LerpUnclamped(dashStartPosition, dashEndPosition, curvePercent);
        };
        abilities[2].onAbilityCancel = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[2].onAbilityEnd = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[2].onAbilityReady = () => { };
        #endregion
        #region Ability Down Angeled
        // "X" Attack down Angeld in View Direction
        abilities[3].onAbilityStart = () =>
        {
            isUsingAbility = true;
            IsAttacking();

            // Find dash End and Start point
            dashEndPosition = MyRayCast.RayCastDownAngeled(transform, attackLength, IsLookingRight());
            dashStartPosition = transform.position;

            trail.SetActive(true);

            PartilceSlash();
            eventDelegate(EventState.LightHit);
            rb.useGravity = false;
        };
        abilities[3].onAbilityUpdate = () =>
        {
            // Dash Ends without Hit
            if (MyEpsilon.Epsilon(transform.position.x, dashEndPosition.x, 0.5f))
            {
                abilities[3].End();
                return;
            }
            // Dash Ends with hitting the enemy 
            if (!abilities[3].hitObject && xNormalHitBox)
            {
                abilities[3].hitObject = true;
                enemyCharacter.Damage(abilities[3].GetDamage());
                enemyCharacter.KickAway(transform.position, false);

                freezCamAction();
                abilities[3].End();
                return;
            }
            // Lerp the Dash
            travel += dashSpeed * Time.deltaTime;
            float curvePercent = curve.Evaluate(travel);
            this.transform.position = Vector3.LerpUnclamped(dashStartPosition, dashEndPosition, curvePercent);
        };
        abilities[3].onAbilityCancel = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[3].onAbilityEnd = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[3].onAbilityReady = () => { };
        #endregion
        #region Ability Up Angeled
        // "X" Attack Up Angeld in View Direction
        abilities[4].onAbilityStart = () =>
        {
            isUsingAbility = true;
            IsAttacking();

            // Find dash End and Start point
            dashEndPosition = MyRayCast.RayCastUpAngeled(transform, attackLength, IsLookingRight());
            dashStartPosition = transform.position;

            trail.SetActive(true);

            PartilceSlash();
            eventDelegate(EventState.LightHit);
            rb.useGravity = false;
        };
        abilities[4].onAbilityUpdate = () =>
        {
            // Dash Ends without Hit
            if (MyEpsilon.Epsilon(transform.position.x, dashEndPosition.x, 0.5f))
            {
                abilities[4].End();
                return;
            }
            // Dash Ends with hitting the enemy 
            if (!abilities[4].hitObject && xNormalHitBox)
            {
                abilities[4].hitObject = true;
                enemyCharacter.Damage(abilities[4].GetDamage());
                enemyCharacter.KickAway(transform.position, false);

                freezCamAction();
                abilities[4].End();
                return;
            }
            // Lerp the Dash
            travel += dashSpeed * Time.deltaTime;
            float curvePercent = curve.Evaluate(travel);
            this.transform.position = Vector3.LerpUnclamped(dashStartPosition, dashEndPosition, curvePercent);
        };
        abilities[4].onAbilityCancel = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[4].onAbilityEnd = () =>
        {
            trail.SetActive(false);

            rb.useGravity = true;
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[4].onAbilityReady = () => { };
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

    void Update()
    {
        if (isDisabled)
        {
            // Bounce Ray
            RaycastHit hit;
            if (Physics.Raycast(transform.position, new Vector3(rb.velocity.x, rb.velocity.y, 0), out hit, 2, 9, QueryTriggerInteraction.Ignore))
            {
                // End Bounce
                if (lastBounceObj == hit.transform.gameObject)
                {
                    lastBounceObj = null;
                    EndDisable();
                    return;
                }
                // Spawn Bounce Particle
                Instantiate(bounce, hit.point, Quaternion.identity);
                // New Bounce Direction
                Vector3 direction = Vector3.Reflect(new Vector3(rb.velocity.x, rb.velocity.y, 0), new Vector3(hit.normal.x, hit.normal.y, 0));
                Debug.DrawLine(transform.position, hit.point);
                rb.velocity = direction;
                // Safe Obj to Check at the next bounce if the Obj is the Same
                lastBounceObj = hit.transform.gameObject;
            }
            else if (MyEpsilon.Epsilon(rb.velocity.x, 0, 1) && MyEpsilon.Epsilon(rb.velocity.y, 0, 1))
            {
                EndDisable();
            }
        }

        // Update each ability every frame
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Update();
        }
    }

    
    void LightAttack()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled)
        {
            abilities[0].Activate();
        }
    }
    void LightAttackRight()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled)
        {
            LookRight();
            abilities[0].Activate();
        }
    }
    void LightAttackLeft()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled)
        {
            LookLeft();
            abilities[0].Activate();
        }
    }
    void LightAttackUp()
    {
        if (!isUsingAbility && IsFalling() && !isDisabled)
            abilities[1].Activate();
    }
    void LightAttackDown()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled)
            abilities[2].Activate();
    }
    void LightAttackRightUp()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled)
        {
            LookRight();
            abilities[4].Activate();
        }
    }
    void LightAttackRightDown()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled)
        {
            LookRight();
            abilities[3].Activate();
        }
    }
    void LightAttackLefttUp()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled)
        {
            LookLeft();
            abilities[4].Activate();
        }
    }
    void LightAttackLeftDown()
    {
        if (IsFalling() && !isUsingAbility && !isDisabled)
        {
            LookLeft();
            abilities[3].Activate();
        }
    }
}
