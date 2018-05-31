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

    bool isUsingAbility = false;
    Vector3 dashStartPosition;
    Vector3 dashEndPosition;
    Ability[] abilities = new Ability[5];

    private void Start()
    {
        DefineAbilities();
        XAttackNormalAction += LightAttack;
        XAttackRightAction += LightAttackRight;
        XAttackLeftAction += LightAttackLeft;
        XAttackUpAction += LightAttackUp;
        XAttackDownAction += LightAttackDown;
    }
    private void OnDisable()
    {
        XAttackNormalAction -= LightAttack;
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



        #endregion

        #region Ability's

        #region Ability Normal Dash
        abilities[0].onAbilityStart = () => 
        {
            isUsingAbility = true;
            IsAttacking();

            if(IsLookingRight())
            {
                dashEndPosition = MyRayCast.RaycastRight(transform.position, attackLength);
                dashStartPosition = transform.position;
            }
            else
            {
                dashEndPosition = MyRayCast.RaycastLeft(transform.position, attackLength);
                dashStartPosition = transform.position;
            }
            rb.useGravity = false;
        };
        abilities[0].onAbilityUpdate = () => 
        {
            if (MyEpsilon.Epsilon(transform.position.x, dashEndPosition.x, 0.5f))
            {
                rb.useGravity = true;
                abilities[0].End();
                return;
            }
            travel += dashSpeed * Time.deltaTime;
            float curvePercent = curve.Evaluate(travel);
            this.transform.position = Vector3.LerpUnclamped(dashStartPosition, dashEndPosition, curvePercent);
        };
        abilities[0].onAbilityCancel = () => 
        {
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[0].onAbilityEnd = () => 
        {
            EndAttacking();
            isUsingAbility = false;
            travel = 0;
        };
        abilities[0].onAbilityReady = () => { };
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
        //for (int i = 0; i < abilities.Length; i++)
        //{
        //    abilities[i].Update();
        //}
        abilities[0].Update();
    }

    
    void LightAttack()
    {
        if (IsFalling() && !isUsingAbility)
        {
            abilities[0].Activate();
        }
    }
    void LightAttackRight()
    {
        if (IsFalling() && !isUsingAbility)
        {
            LookRight();
            abilities[0].Activate();
        }
    }
    void LightAttackLeft()
    {
        if (IsFalling() && !isUsingAbility)
        {
            LookLeft();
            abilities[0].Activate();
        }
    }
    void LightAttackUp()
    {
        if (!isUsingAbility && IsFalling())
            abilities[1].Activate();
    }
    void LightAttackDown()
    {
        if (IsFalling() && !isUsingAbility)
        {
            abilities[2].Activate();
        }
    }
}
