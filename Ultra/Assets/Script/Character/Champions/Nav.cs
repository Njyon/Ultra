﻿using System.Collections;
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
        XAttackUpAction += LightAttackUp;
        XAttackDownAction += LightAttackDown;
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

        // X Light Attack Up
        abilities[3] = new Ability(
           "Light Attack Upercut",
           "A Light Attack thats launches the enmy in the air (On ground)",
           10,
           0.2f,
           1f,
           0.2f,
           false
           );

        // X Light Attack in Air Up (in Look direction)
        abilities[4] = new Ability(
           "Light Attack in Air Up",
           "A Light Attack thats launches the enmy in the air (in Air)",
           10,
           0.1f,
           1f,
           0.01f,
           false
           );

        // X Light Attack in Air Up (in Look direction)
        abilities[5] = new Ability(
           "Light Attack in Air in Look Direction",
           "A Light Attack thats that grabs the enemy and at the end kick him away (in Air)",
           10,
           0.1f,
           1f,
           0.01f,
           false
           );

        // X Light Attack in Air Up (in Look direction)
        abilities[6] = new Ability(
           "Light Attack Kicking down",
           "A Light Attack thats launches the enmy to the ground (in Air)",
           10,
           0.1f,
           1f,
           0.01f,
           false
           );

        // Y/B Havy Attack on ground
        abilities[7] = new Ability(
           "Teleport Stuff",
           "TODO :",
           20,
           0.2f,
           2f,
           0.3f,
           false
           );

        #endregion

        #region Ability's
        // Normal Attacks (X)
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
        #region Ability 3
        abilities[3].onAbilityStart = () => 
        {
            isUsingAbility = true;
            Disable();
        };
        abilities[3].onAbilityUpdate = () => 
        {
            if(xNormalHitBox && !abilities[3].hitObject)
            {
                abilities[3].hitObject = true;
                enemyCharacter.Disable();
                enemyCharacter.Damage(abilities[3].GetDamage());
                enemyCharacter.KickUp(enemyCharacter, this.transform.position, false);
            }
        };
        abilities[3].onAbilityCancel = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[3].onAbilityEnd = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[3].onAbilityReady = () => { };
        #endregion
        #region Ability 4
        abilities[4].onAbilityStart = () => 
        {
            isUsingAbility = true;
            Disable();
        };
        abilities[4].onAbilityUpdate = () => 
        {
            if(!IsFalling())
            {
                abilities[4].Cancel();
            }
            else if(!abilities[4].hitObject)
            {
                abilities[4].hitObject = true;
                enemyCharacter.Damage(abilities[4].GetDamage());
                enemyCharacter.KickUp(enemyCharacter, this.transform.position, false);
            }
        };
        abilities[4].onAbilityCancel = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[4].onAbilityEnd = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[4].onAbilityReady = () => { };
        #endregion
        #region Ability 5
        abilities[5].onAbilityStart = () => 
        {
            isUsingAbility = true;
            Disable();
        };
        abilities[5].onAbilityUpdate = () => 
        {
            if(xNormalHitBox)
            {
                abilities[5].hitObject = true;
                enemyCharacter.Stun();
                if(IsLookingRight())
                {
                    float distance = enemy.transform.position.x - this.transform.position.x;
                    enemy.transform.position = new Vector3(this.transform.position.x + distance, enemy.transform.position.y, 0);
                }
                else
                {
                    float distance = enemy.transform.position.x - this.transform.position.x;
                    enemy.transform.position = new Vector3(this.transform.position.x - distance, enemy.transform.position.y, 0);
                }
            }
        };
        abilities[5].onAbilityCancel = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[5].onAbilityEnd = () => 
        {
            if(abilities[5].hitObject)
            {
                enemyCharacter.Damage(abilities[5].GetDamage());
                enemyCharacter.KickAway(enemyCharacter, this.transform.position, false);
                enemyCharacter.EndStun();
            }
            isUsingAbility = false;
            EndDisable();
        };
        abilities[5].onAbilityReady = () => { };
        #endregion
        #region Ability 6
        abilities[6].onAbilityStart = () => 
        {
            isUsingAbility = true;
            Disable();
        };
        abilities[6].onAbilityUpdate = () => 
        {
            if (xDownHitBox && !abilities[6].hitObject)
            {
                abilities[6].hitObject = true;
                enemyCharacter.Damage(abilities[6].GetDamage());
                enemyCharacter.KickDown(enemyCharacter, this.transform.position, false);
            }
        };
        abilities[6].onAbilityCancel = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[6].onAbilityEnd = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[6].onAbilityReady = () => { };

        #endregion

        // Havy Attacks (Y/B)
        #region Ability 7
        abilities[7].onAbilityStart = () => 
        {
            isUsingAbility = true;
            Disable();
        };
        abilities[7].onAbilityUpdate = () => 
        {

        };
        abilities[7].onAbilityCancel = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[7].onAbilityEnd = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[7].onAbilityReady = () => { };

        #endregion

        //abilities[2].onAbilityStart = () => { };
        //abilities[2].onAbilityUpdate = () => { };
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
        abilities[3].Update();
        abilities[4].Update();
        abilities[5].Update();
        abilities[6].Update();
    }

    void LightAttack()
    {
        if(IsFalling() && !isUsingAbility)
        {
            abilities[4].Activate();
        }
        else if(!isUsingAbility)
            abilities[0].Activate();
    }
    void LightAttackRight()
    {
        if (IsFalling() && !isUsingAbility)
        {
            abilities[5].Activate();
        }
        else if (!isUsingAbility && !IsFalling())
            abilities[1].Activate();
    }
    void LightAttackLeft()
    {
        if (IsFalling() && !isUsingAbility)
        {
            abilities[5].Activate();
        }
        else if (!isUsingAbility && !IsFalling())
            abilities[2].Activate();
    }
    void LightAttackUp()
    {
        if (!isUsingAbility && !IsFalling())
            abilities[3].Activate();
    }
    void LightAttackDown()
    {
        if (IsFalling() && !isUsingAbility)
        {
            abilities[6].Activate();
        }
    }
}
