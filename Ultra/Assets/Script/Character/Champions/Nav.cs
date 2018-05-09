using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav : MyCharacter
{
    public float xHitNormalStunTime;
    public float lightAttackDashLenght;
    public float lightAttackDashTime;

    bool isUsingAbility = false;
    float currentLightAttackDashTime;
    float lightAttackJourneyLenght;
    Vector3 dashDestination;
    Ability[] abilities = new Ability[10];

    private void Start()
    {
        DefineAbilities();
        XAttackNormalAction += LightAttack;
        XAttackRightAction += LightAttackRight;
        XAttackLeftAction += LightAttackLeft;
    }

    private void OnDisable()
    {
        XAttackNormalAction -= LightAttack;
    }

    void DefineAbilities()
    {
        #region Ability Definiton
        // X Normal direction = Look direction
        abilities[0] = new Ability(
            "Light Attack",
            "A Light Attack in view Direktion, its fast and does some Damage",
            10,
            0.5f,
            0.3f,
            0.2f,
            false
            );

        // X Light attack in Right Direction
        abilities[1] = new Ability(
            "Light Attack Right",
            "A Light Attack that lefts u dash to the Right (On ground)",
            10,
            0.2f,
            1f,
            0.2f,
            false
            );

        // X Light attack in Left Direction
        abilities[2] = new Ability(
            "Light Attack Right",
            "A Light Attack that lefts u dash to the Right (On ground)",
            10,
            0.2f,
            1f,
            0.2f,
            false
            );

        #endregion

        #region Ability's
        #region Ability 0
        abilities[0].onAbilityStart = () => 
        {
            isUsingAbility = true;
            Disable(this);
        };
        abilities[0].onAbilityEnd = () => 
        {
            if (abilities[0].hitObject)
            {
                enemyCharacter.Damage(abilities[0].GetDamage());
                enemyCharacter.KickAway(enemyCharacter, this.transform.position, false);
            }
            EndDisable(this);
            isUsingAbility = false;
        };
        abilities[0].onAbilityCancel = () => 
        {
            EndDisable();
            isUsingAbility = false;
        };
        abilities[0].onAbilityUpdate = () =>
        {
            if (xNormalHitBox && !abilities[0].hitObject)
            {
                abilities[0].hitObject = true;
                Disable(enemyCharacter);
            }
            else if (hasSlider && !abilities[0].hitObject)
            {
                abilities[0].hitObject = true;
                slider.GetComponent<Slider>().SliderChange(this.transform);
            }
        };
        abilities[0].onAbilityReady = () => { };
        #endregion
        #region Ability 1
        abilities[1].onAbilityStart = () =>
        {
            RaycastHit hit;

            isUsingAbility = true;
            currentLightAttackDashTime = lightAttackDashTime;
            Disable();

            if (this.transform.position.x < 0)
            {
                if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, lightAttackDashLenght, 9, QueryTriggerInteraction.Ignore))
                {
                    dashDestination = hit.point;
                }
                else
                {
                    dashDestination = new Vector3(this.transform.position.x + lightAttackDashLenght, this.transform.position.y, 0);
                }
            }
            else
            {
                if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, lightAttackDashLenght, 9, QueryTriggerInteraction.Ignore))
                {
                    dashDestination = hit.point;
                }
                else
                {
                    dashDestination = new Vector3(this.transform.position.x + lightAttackDashLenght, this.transform.position.y, 0);
                }
            }
            lightAttackJourneyLenght = Vector3.Distance(this.transform.position, dashDestination);
        };
        abilities[1].onAbilityUpdate = () => 
        {
            if (!IsFalling())
            {
                currentLightAttackDashTime -= Time.deltaTime;
                float travel = currentLightAttackDashTime / lightAttackJourneyLenght;
                this.transform.position = Vector3.Lerp(this.transform.position, dashDestination, travel);
            }
            else
            {
                abilities[1].Cancel();
            }
            if(xNormalHitBox && !abilities[1].hitObject)
            {
                abilities[1].hitObject = true;
                enemyCharacter.Damage(abilities[1].GetDamage());
                enemyCharacter.KickAway(enemyCharacter, this.transform.position, false);
                enemyCharacter.Disable(1f);
            }
        };
        abilities[1].onAbilityCancel = () => 
        {
            EndDisable();
            isUsingAbility = false;
        };
        abilities[1].onAbilityEnd = () =>
        {
            EndDisable();
            isUsingAbility = false;
        };
        abilities[1].onAbilityReady = () => { };

        #endregion
        #region Ability 2
        abilities[2].onAbilityStart = () =>
        {
            RaycastHit hit;

            isUsingAbility = true;
            currentLightAttackDashTime = lightAttackDashTime;
            Disable();

            if (this.transform.position.x < 0)
            {
                if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, lightAttackDashLenght, 9, QueryTriggerInteraction.Ignore))
                {
                    dashDestination = hit.point;
                }
                else
                {
                    dashDestination = new Vector3(this.transform.position.x + lightAttackDashLenght, this.transform.position.y, 0);
                }
            }
            else
            {
                if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, lightAttackDashLenght, 9, QueryTriggerInteraction.Ignore))
                {
                    dashDestination = hit.point;
                }
                else
                {
                    dashDestination = new Vector3(this.transform.position.x + lightAttackDashLenght, this.transform.position.y, 0);
                }
            }
            Debug.Log("HUAN!");
            lightAttackJourneyLenght = Vector3.Distance(this.transform.position, dashDestination);
        };
        abilities[2].onAbilityUpdate = () =>
        {
            if (!IsFalling())
            {
                currentLightAttackDashTime -= Time.deltaTime;
                float travel = currentLightAttackDashTime / lightAttackJourneyLenght;
                this.transform.position = Vector3.Lerp(this.transform.position, dashDestination, travel);
            }
            else
            {
                abilities[2].Cancel();
            }
            if (xNormalHitBox && !abilities[2].hitObject)
            {
                abilities[2].hitObject = true;
                enemyCharacter.Damage(abilities[2].GetDamage());
                enemyCharacter.KickAway(enemyCharacter, this.transform.position, false);
                enemyCharacter.Disable(1f);
            }
        };
        abilities[2].onAbilityCancel = () =>
        {
            EndDisable();
            isUsingAbility = false;
        };
        abilities[2].onAbilityEnd = () =>
        {
            EndDisable();
            isUsingAbility = false;
        };
        abilities[2].onAbilityReady = () => { };

        #endregion


        //abilities[2].onAbilityStart = () => { };
        //abilities[2].onAbilityUpdate = () => { };
        //abilities[2].onAbilityEnd = () => { };
        //abilities[2].onAbilityCancel = () => { };
        //abilities[2].onAbilityEnd = () => { };
        //abilities[2].onAbilityReady = () => { };
        #endregion
    }

    void Update()
    {
        abilities[0].Update();
        abilities[1].Update();
        abilities[2].Update();
    }

    void LightAttack()
    {
        if(!isUsingAbility)
            abilities[0].Activate();
    }

    void LightAttackRight()
    {
        if (!isUsingAbility && !IsFalling())
            abilities[1].Activate();
    }

    void LightAttackLeft()
    {
        if (!isUsingAbility && !IsFalling())
            abilities[2].Activate();
    }
}
