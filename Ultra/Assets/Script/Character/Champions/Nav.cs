using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav : MyCharacter
{
    [Header("Light Attack Stuff")]
    public float xHitNormalStunTime;
    public float lightAttackDashLenght;
    public float lightAttackDashTime;

    [Header("Special Attack Stuff")]
    public float SpecialAttackDashRange;
    public float SpecialAttackDashTime;
    public float SpecialAttackKickHight;
    public float SpecialAttackTeleportRange;
    public float SpecialAttackBackTeleportRange;
    public float SpecialAttackBackDashForwardRange;
    public float SpecialAttackKickDown;

    bool usingSpecialUp = false;
    bool usingSpecialDown = false;
    bool usingSpecialSide = false;
    bool usingSpecialNormal = false;
    bool isCharging = false;
    bool doOnce = false;
    bool enemyKickingUp = false;
    bool isUsingAbility = false;
    float currentSpecialAttackDashTime;
    float currentLightAttackDashTime;
    float currentEnemyTravelTime;
    float SpecialAttackJourneyLenght;
    float enemyKickUpJourneyDistance;
    float lightAttackJourneyLenght;
    float havyAttackChargeCounter;
    Vector3 dashDestination;
    Vector3 enemyDestination;
    Vector3 TeleportDestination;
    Ability[] abilities = new Ability[12];

    private void Start()
    {
        DefineAbilities();
        XAttackNormalAction += LightAttack;
        XAttackRightAction += LightAttackRight;
        XAttackLeftAction += LightAttackLeft;
        XAttackUpAction += LightAttackUp;
        XAttackDownAction += LightAttackDown;
        SpecialNormalAction += SpecialAttackNormal;
        SpecialReleaseAction += AbordSpecial;
        SpecialRightAction += SpecialAttackRight;
        SpecialLeftAction += SpecialAttackLeft;
        SpecialDownAction += SpecialAttackDown;
        SpecialUpAction += SpecialAttackUp;
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
            0.5f,
            0.2f,
            false
            );

        // X Light attack in Left Direction
        abilities[2] = new Ability(
            "Light Attack Right",
            "A Light Attack that lefts u dash to the Right (On ground)",
            10,
            0.2f,
            0.5f,
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
           100,
           0.2f,
           0.5f,
           0.3f,
           false
           );

        // Y/B Havy Attack to the Sides on ground
        abilities[8] = new Ability(
           "Teleport Stuff",
           "TODO :",
           100,
           0.2f,
           0.5f,
           0.3f,
           false
           );

        // Y/B Havy Attack Down on ground
        abilities[9] = new Ability(
           "Teleport & Dash Stuff",
           "TODO :",
           100,
           0.2f,
           0.5f,
           0.3f,
           false
           );

        // Y/B Havy Attack Up in air
        abilities[10] = new Ability(
           "Heavy punsh UP",
           "TODO :",
           100,
           0.1f,
           0.7f,
           0.3f,
           false
           );

        // Y/B Havy Attack down in air
        abilities[11] = new Ability(
           "Heavy punsh down",
           "TODO :",
           100,
           0.1f,
           0.7f,
           0.3f,
           false
           );

        #endregion

        #region Ability's
        // Normal Attacks (X)
        #region Ability 0   // Normal Attack in view Direction
        abilities[0].onAbilityStart = () => 
        {
            isUsingAbility = true;
            Disable();

            //if (eventDelegate != null)
                //eventDelegate(EventState.LightHit);
        };
        abilities[0].onAbilityEnd = () => 
        {
            if (abilities[0].hitObject)
            {
                enemyCharacter.Damage(abilities[0].GetDamage());
                enemyCharacter.KickAway(this.transform.position, false);
            }
            EndDisable();
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
                enemyCharacter.Disable();
            }
        };
        abilities[0].onAbilityReady = () => { };
        #endregion  
        #region Ability 1   // Light Dash to the Right
        abilities[1].onAbilityStart = () =>
        {
            RaycastHit hit;

            isUsingAbility = true;
            currentLightAttackDashTime = lightAttackDashTime;
            Disable();

            //eventDelegate(EventState.LightHitSide);

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

            currentLightAttackDashTime -= Time.deltaTime;
            float travel = currentLightAttackDashTime / lightAttackJourneyLenght;
            this.transform.position = Vector3.Lerp(this.transform.position, dashDestination, travel);
        
            if(xNormalHitBox && !abilities[1].hitObject)
            {
                abilities[1].hitObject = true;
                enemyCharacter.Damage(abilities[1].GetDamage());
                enemyCharacter.KickAway(this.transform.position, false);
                abilities[1].End();
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
        #region Ability 2   // Light Dash to the Left
        abilities[2].onAbilityStart = () =>
        {
            RaycastHit hit;

            isUsingAbility = true;
            currentLightAttackDashTime = lightAttackDashTime;
            Disable();

            //if (eventDelegate != null)
            //    eventDelegate(EventState.LightHitSide);

            if (this.transform.position.x < 0)
            {
                if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, lightAttackDashLenght, 9, QueryTriggerInteraction.Ignore))
                {
                    dashDestination = hit.point;
                }
                else
                {
                    dashDestination = new Vector3(this.transform.position.x - lightAttackDashLenght, this.transform.position.y, 0);
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
                    dashDestination = new Vector3(this.transform.position.x - lightAttackDashLenght, this.transform.position.y, 0);
                }
            }
            lightAttackJourneyLenght = Vector3.Distance(this.transform.position, dashDestination);
        };
        abilities[2].onAbilityUpdate = () =>
        {

            currentLightAttackDashTime -= Time.deltaTime;
            float travel = currentLightAttackDashTime / lightAttackJourneyLenght;
            this.transform.position = Vector3.Lerp(this.transform.position, dashDestination, travel);
        
            if (xNormalHitBox && !abilities[2].hitObject)
            {
                abilities[2].hitObject = true;
                enemyCharacter.Damage(abilities[2].GetDamage());
                enemyCharacter.KickAway(this.transform.position, false);
                abilities[2].End();
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
        #region Ability 3   // Light UperCut
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
                enemyCharacter.KickUp(this.transform.position, false);
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
        #region Ability 4   // Light UperCut in Air
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
            else if(xUpHitBox && !abilities[4].hitObject)
            {
                abilities[4].hitObject = true;
                enemyCharacter.Damage(abilities[4].GetDamage());
                enemyCharacter.KickUp(this.transform.position, false);
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
        #region Ability 5   // Light Attack in view Direction in air
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
                enemyCharacter.KickAway(this.transform.position, false);
                enemyCharacter.EndStun();
            }
            isUsingAbility = false;
            EndDisable();
        };
        abilities[5].onAbilityReady = () => { };
        #endregion
        #region Ability 6   // Light Kickdown in Air
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
                enemyCharacter.KickDown(this.transform.position, false);
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
        #region Ability 7   // Heavy Dash with Jump
        abilities[7].onAbilityStart = () =>
        {
            RaycastHit hit;
            usingSpecialNormal = true;
            isUsingAbility = true;
            Disable();

            // Checking where the player is looking and if there are obsticles in his way for a Dash
            if (IsLookingRight()) 
            {
                if (this.transform.position.x < 0)
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, SpecialAttackDashRange, 9, QueryTriggerInteraction.Ignore))
                    {
                        dashDestination = hit.point;
                    }
                    else
                    {
                        dashDestination = new Vector3(this.transform.position.x + SpecialAttackDashRange, this.transform.position.y, 0);
                    }
                }
                else
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, SpecialAttackDashRange, 9, QueryTriggerInteraction.Ignore))
                    {
                        dashDestination = hit.point;
                    }
                    else
                    {
                        dashDestination = new Vector3(this.transform.position.x + SpecialAttackDashRange, this.transform.position.y, 0);
                    }
                }
            }
            else
            {
                if (this.transform.position.x < 0)
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, SpecialAttackDashRange, 9, QueryTriggerInteraction.Ignore))
                    {
                        dashDestination = hit.point;
                    }
                    else
                    {
                        dashDestination = new Vector3(this.transform.position.x - SpecialAttackDashRange, this.transform.position.y, 0);
                    }
                }
                else
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, SpecialAttackDashRange, 9, QueryTriggerInteraction.Ignore))
                    {
                        dashDestination = hit.point;
                    }
                    else
                    {
                        dashDestination = new Vector3(this.transform.position.x - SpecialAttackDashRange, this.transform.position.y, 0);
                    }
                }
            }
            // Safing Vars for later Operations
            SpecialAttackJourneyLenght = Vector3.Distance(this.transform.position, dashDestination);
            currentSpecialAttackDashTime = SpecialAttackDashTime;
        };
        abilities[7].onAbilityUpdate = () =>
        {
            if (currentSpecialAttackDashTime > 0 && !abilities[7].hitObject)            // Dash to the DashDestination while he doesnt hit an enemy
            {
                currentSpecialAttackDashTime -= Time.deltaTime;
                float travel = currentSpecialAttackDashTime / SpecialAttackJourneyLenght;
                this.transform.position = Vector3.Lerp(this.transform.position, dashDestination, travel);
            }
            if(!abilities[7].hitObject && xNormalHitBox)                            // when a enemy is hit check how high the enym character can fly in the air
            {
                RaycastHit hit;
                
                abilities[7].hitObject = true;
                if(Physics.Raycast(enemy.transform.position, new Vector3(0, this.transform.position.y, 0), out hit, SpecialAttackKickHight, 9, QueryTriggerInteraction.Ignore))
                {
                    // hit an Obsticle
                    enemyCharacter.Stun();
                    enemyDestination = hit.point;
                    enemyKickUpJourneyDistance = Vector3.Distance(enemy.transform.position, enemyDestination);
                    enemyKickingUp = true;
                    currentEnemyTravelTime = 0.5f;
                }
                else
                {
                    // hits no Obsticle
                    enemyCharacter.Stun();
                    enemyDestination = new Vector3(enemy.transform.position.x, enemy.transform.position.y + SpecialAttackKickHight, 0);
                    enemyKickUpJourneyDistance = Vector3.Distance(enemy.transform.position, enemyDestination);
                    enemyKickingUp = true;
                    currentEnemyTravelTime = 0.5f;
                }
            }
        };
        abilities[7].onAbilityCancel = () =>
        {
            enemyKickingUp = false;
            usingSpecialNormal = false;
            isUsingAbility = false;
            EndDisable();
        };
        abilities[7].onAbilityEnd = () => 
        {
            if(abilities[7].hitObject)
            {
                // if the Player hits the enemy Teleport to him and Kick him Away
                if(enemyCharacter.IsLookingRight())
                {
                    LookLeft();
                    //if (eventDelegate != null)
                    //    eventDelegate(EventState.Teleport);
                    transform.position = new Vector3(enemy.transform.position.x + 1, enemy.transform.position.y, 0);
                    //if (eventDelegate != null)
                    //    eventDelegate(EventState.Teleport);
                    enemyCharacter.EndStun();
                    enemyCharacter.Damage(abilities[7].GetDamage() * Mathf.RoundToInt(havyAttackChargeCounter));
                    enemyCharacter.KickAway(this.transform.position, true);
                }
                else
                {
                    LookRight();
                    //if (eventDelegate != null)
                    //    eventDelegate(EventState.Teleport);
                    transform.position = new Vector3(enemy.transform.position.x - 1, enemy.transform.position.y, 0);
                    //if (eventDelegate != null)
                    //    eventDelegate(EventState.Teleport);
                    enemyCharacter.EndStun();
                    enemyCharacter.Damage(abilities[7].GetDamage() * Mathf.RoundToInt(havyAttackChargeCounter));
                    enemyCharacter.KickAway(this.transform.position, true);
                }
            }
            else
            {
                //if (eventDelegate != null)
                //    eventDelegate(EventState.Teleport);
                transform.position = new Vector3(this.transform.position.x, this.transform.position.y + SpecialAttackKickHight, 0);
                //if (eventDelegate != null)
                //    eventDelegate(EventState.Teleport);
            }
            enemyKickingUp = false;
            isUsingAbility = false;
            EndDisable();
        };
        abilities[7].onAbilityReady = () => 
        {
            usingSpecialNormal = false;
        };

        #endregion
        #region Ability 8   // Heavy Attack Teleport and Smash
        abilities[8].onAbilityStart = () => 
        {

            isUsingAbility = true;
            Disable();
            doOnce = false;

        };
        abilities[8].onAbilityUpdate = () => 
        {
            if(!doOnce)
            {
                RaycastHit hit;
                doOnce = true;

                if (IsLookingRight())
                {
                    if (transform.position.x < 0)
                    {
                        if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, SpecialAttackTeleportRange, 9, QueryTriggerInteraction.Ignore))
                        {
                            TeleportDestination = hit.point;
                        }
                        else
                        {
                            TeleportDestination = new Vector3(this.transform.position.x + SpecialAttackTeleportRange, this.transform.position.y, 0);
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, SpecialAttackTeleportRange, 9, QueryTriggerInteraction.Ignore))
                        {
                            TeleportDestination = hit.point;
                        }
                        else
                        {
                            TeleportDestination = new Vector3(this.transform.position.x + SpecialAttackTeleportRange, this.transform.position.y, 0);
                        }
                    }
                }
                else
                {
                    if (transform.position.x < 0)
                    {
                        if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, SpecialAttackTeleportRange, 9, QueryTriggerInteraction.Ignore))
                        {
                            TeleportDestination = hit.point;
                        }
                        else
                        {
                            TeleportDestination = new Vector3(this.transform.position.x - SpecialAttackTeleportRange, this.transform.position.y, 0);
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, SpecialAttackTeleportRange, 9, QueryTriggerInteraction.Ignore))
                        {
                            TeleportDestination = hit.point;
                        }
                        else
                        {
                            TeleportDestination = new Vector3(this.transform.position.x - SpecialAttackTeleportRange, this.transform.position.y, 0);
                        }
                    }
                }
                //if (eventDelegate != null)
                //    eventDelegate(EventState.Teleport);
                this.transform.position = TeleportDestination;
                //if (eventDelegate != null)
                //    eventDelegate(EventState.Teleport);
            }

            if(xNormalHitBox && !abilities[8].hitObject)
            {
                abilities[8].hitObject = true;
                enemyCharacter.Damage(abilities[8].GetDamage() * Mathf.RoundToInt(havyAttackChargeCounter));
                enemyCharacter.KickAway(this.transform.position, true);
            }
        };
        abilities[8].onAbilityCancel = () =>
        {
            usingSpecialSide = false;
            isUsingAbility = false;
            EndDisable();
        };
        abilities[8].onAbilityEnd = () =>
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[8].onAbilityReady = () => 
        {
            usingSpecialSide = false;
        };

        #endregion
        #region Ability 9   // Heavy Attack back Teleport with dash in view direction
        abilities[9].onAbilityStart = () => 
        {
            RaycastHit hit;

            doOnce = false;

            // Set the Back Teleport Location
            if (IsLookingRight())
            {
                if (transform.position.x < 0)
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, SpecialAttackBackTeleportRange, 9, QueryTriggerInteraction.Ignore))
                    {
                        TeleportDestination = hit.point;
                    }
                    else
                    {
                        TeleportDestination = new Vector3(this.transform.position.x - SpecialAttackBackTeleportRange, this.transform.position.y, 0);
                    }
                }
                else
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, SpecialAttackBackTeleportRange, 9, QueryTriggerInteraction.Ignore))
                    {
                        TeleportDestination = hit.point;
                    }
                    else
                    {
                        TeleportDestination = new Vector3(this.transform.position.x - SpecialAttackBackTeleportRange, this.transform.position.y, 0);
                    }
                }
            }
            else
            {
                if (transform.position.x < 0)
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, SpecialAttackBackTeleportRange, 9, QueryTriggerInteraction.Ignore))
                    {
                        TeleportDestination = hit.point;
                    }
                    else
                    {
                        TeleportDestination = new Vector3(this.transform.position.x + SpecialAttackBackTeleportRange, this.transform.position.y, 0);
                    }
                }
                else
                {
                    if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, SpecialAttackBackTeleportRange, 9, QueryTriggerInteraction.Ignore))
                    {
                        TeleportDestination = hit.point;
                    }
                    else
                    {
                        TeleportDestination = new Vector3(this.transform.position.x + SpecialAttackBackTeleportRange, this.transform.position.y, 0);
                    }
                }
            }

            // Set the Player to the Teleport Position
            //if (eventDelegate != null)
            //    eventDelegate(EventState.Teleport);
            this.transform.position = TeleportDestination;
            //if (eventDelegate != null)
            //    eventDelegate(EventState.Teleport);

        };
        abilities[9].onAbilityUpdate = () => 
        {
            if (!doOnce)
            {
                RaycastHit hit;

                doOnce = true;
               
                // Set Dash Forward Location
                if (IsLookingRight())
                {
                    if (transform.position.x < 0)
                    {
                        if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, SpecialAttackBackDashForwardRange, 9, QueryTriggerInteraction.Ignore))
                        {
                            dashDestination = hit.point;
                        }
                        else
                        {
                            dashDestination = new Vector3(this.transform.position.x + SpecialAttackBackDashForwardRange, this.transform.position.y, 0);
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, SpecialAttackBackDashForwardRange, 9, QueryTriggerInteraction.Ignore))
                        {
                            dashDestination = hit.point;
                        }
                        else
                        {
                            dashDestination = new Vector3(this.transform.position.x + SpecialAttackBackDashForwardRange, this.transform.position.y, 0);
                        }
                    }
                }
                else
                {
                    if (this.transform.position.x < 0)
                    {
                        if (Physics.Raycast(this.transform.position, new Vector3(this.transform.position.x, 0, 0), out hit, SpecialAttackBackDashForwardRange, 9, QueryTriggerInteraction.Ignore))
                        {
                            dashDestination = hit.point;
                        }
                        else
                        {
                            dashDestination = new Vector3(this.transform.position.x - SpecialAttackBackDashForwardRange, this.transform.position.y, 0);
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(this.transform.position, new Vector3(-this.transform.position.x, 0, 0), out hit, SpecialAttackBackDashForwardRange, 9, QueryTriggerInteraction.Ignore))
                        {
                            dashDestination = hit.point;
                        }
                        else
                        {
                            dashDestination = new Vector3(this.transform.position.x - SpecialAttackBackDashForwardRange, this.transform.position.y, 0);
                        }
                    }
                }

                // Set the Distince between Character Location and the Dashdestination
                SpecialAttackJourneyLenght = Vector3.Distance(this.transform.position, dashDestination);

                // Set the dashTime back to default value
                currentSpecialAttackDashTime = SpecialAttackDashTime;
            }
            // Check if hit the enemy while Dashing
            if(!abilities[9].hitObject && xNormalHitBox)
            {
                abilities[9].hitObject = true;
                enemyCharacter.Damage(abilities[9].GetDamage() * Mathf.RoundToInt(havyAttackChargeCounter));
                enemyCharacter.KickAway(this.transform.position, true);
                abilities[9].End();
            }
            // Lerp the Player Position to the DashDestination
            else if (currentSpecialAttackDashTime > 0 && !abilities[9].hitObject)
            {
                currentSpecialAttackDashTime -= Time.deltaTime;
                float travel = currentSpecialAttackDashTime / SpecialAttackJourneyLenght;
                this.transform.position = Vector3.Lerp(this.transform.position, dashDestination, travel);
            }
        };
        abilities[9].onAbilityCancel = () => 
        {
            isUsingAbility = false;
            usingSpecialDown = false;
            EndDisable();
        };
        abilities[9].onAbilityEnd = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[9].onAbilityReady = () => 
        {
            usingSpecialDown = false;
        };
        #endregion
        #region Ability 10  // Heavy Upercut in Air
        abilities[10].onAbilityStart = () => 
        {
            SpecialJump();
        };
        abilities[10].onAbilityUpdate = () => 
        {
            if(sUpHitBox && !abilities[10].hitObject)
            {
                abilities[10].hitObject = true;
                enemyCharacter.Damage(abilities[10].GetDamage() * Mathf.RoundToInt(havyAttackChargeCounter));
                enemyCharacter.KickUp(this.transform.position, true);
            }
        };
        abilities[10].onAbilityCancel = () =>
        {
            isUsingAbility = false;
            usingSpecialUp = false;
        };
        abilities[10].onAbilityEnd = () => 
        {
            isUsingAbility = false;
        };
        abilities[10].onAbilityReady = () => 
        {
            usingSpecialUp = false;
        };
        #endregion
        #region Ability 11  // Heavy Downkick in Air
        abilities[11].onAbilityStart = () => 
        {
            RaycastHit hit;
            if(this.transform.position.y < 0)
            {
                if (Physics.Raycast(transform.position, new Vector3(0, this.transform.position.y, 0), out hit, SpecialAttackKickDown, 9, QueryTriggerInteraction.Ignore))
                {
                    dashDestination = hit.point;
                }
                else
                {
                    dashDestination = new Vector3(this.transform.position.x, this.transform.position.y - SpecialAttackKickDown, 0);
                }
            }
            else
            {
                if (Physics.Raycast(transform.position, new Vector3(0, -this.transform.position.y, 0), out hit, SpecialAttackKickDown, 9, QueryTriggerInteraction.Ignore))
                {
                    dashDestination = hit.point;
                }
                else
                {
                    dashDestination = new Vector3(this.transform.position.x, this.transform.position.y - SpecialAttackKickDown, 0);
                }
            }
            currentSpecialAttackDashTime = SpecialAttackDashTime;
            SpecialAttackJourneyLenght = Vector3.Distance(this.transform.position, dashDestination);
        };
        abilities[11].onAbilityUpdate = () => 
        {
            if (!abilities[11].hitObject && sDownHitBox)
            {
                abilities[11].hitObject = true;
                enemyCharacter.Damage(abilities[11].GetDamage());
                enemyCharacter.KickDown(this.transform.position, true);
            }
            else if (currentSpecialAttackDashTime > 0)
            {
                currentSpecialAttackDashTime -= Time.deltaTime;
                float travel = currentSpecialAttackDashTime / SpecialAttackJourneyLenght;
                this.transform.position = Vector3.Lerp(this.transform.position, dashDestination, travel);
            }
        };
        abilities[11].onAbilityCancel = () => 
        {
            usingSpecialDown = false;
            isUsingAbility = false;
            EndDisable();
        };
        abilities[11].onAbilityEnd = () => 
        {
            isUsingAbility = false;
            EndDisable();
        };
        abilities[11].onAbilityReady = () => 
        {
            usingSpecialDown = false;
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

    void Update()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].Update();
        }

        if (isCharging)
            havyAttackChargeCounter += 0.5f * Time.deltaTime;
        
        if(enemyKickingUp)
        {
            if(MoveEnemyTo(enemyDestination) && usingSpecialNormal)
            {
                abilities[7].End();
            }
        }
    }

    /// <summary>
    /// Move The Enemy to a destination
    /// </summary>
    /// <param name="destination"></param>
    /// <returns></returns>
    bool MoveEnemyTo(Vector3 destination)
    {
        if(enemy.transform.position == destination)
        {
            return true;
        }
        else
        {
            currentEnemyTravelTime -= Time.deltaTime;
            float travel = currentEnemyTravelTime / enemyKickUpJourneyDistance;
            enemy.transform.position = Vector3.Lerp(enemy.transform.position, destination, travel);
            return false;
        }
        
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

    void AbordSpecial()
    {
        if (!isUsingAbility || !isCharging)
            return;

        if (usingSpecialNormal)
        {
            isCharging = false;
            abilities[7].Activate();
        }
        else if(usingSpecialSide)
        {
            isCharging = false;
            abilities[8].Activate();
        }
        else if(usingSpecialDown)
        {
            isCharging = false;
            abilities[9].Activate();
        }
    }
    void SpecialAttackNormal()
    {
        if (!isUsingAbility && !IsFalling() && !usingSpecialNormal)
        {
            usingSpecialNormal = true;
            isUsingAbility = true;
            isCharging = true;
            havyAttackChargeCounter = 1;
            Disable();
        }
        else if (IsFalling() && !isUsingAbility && !usingSpecialUp)
        {
            usingSpecialUp = true;
            isUsingAbility = true;
            abilities[10].Activate();
        }
    }
    void SpecialAttackRight()
    {
        if (!isUsingAbility && !IsFalling() && !usingSpecialSide)
        {
            usingSpecialSide = true;
            isUsingAbility = true;
            isCharging = true;
            havyAttackChargeCounter = 1;
            LookRight();
            Disable();
        }
        else if (IsFalling() && !isUsingAbility && !usingSpecialUp)
        {
            usingSpecialUp = true;
            isUsingAbility = true;
            abilities[10].Activate();
        }
    }
    void SpecialAttackLeft()
    {
        if (!isUsingAbility && !IsFalling() && !usingSpecialSide)
        {
            usingSpecialSide = true;
            isUsingAbility = true;
            isCharging = true;
            havyAttackChargeCounter = 1;
            LookLeft();
            Disable();
        }
        else if (IsFalling() && !isUsingAbility && !usingSpecialUp)
        {
            usingSpecialUp = true;
            isUsingAbility = true;
            abilities[10].Activate();
        }
    }
    void SpecialAttackDown()
    {
        if (!isUsingAbility && !IsFalling() && !usingSpecialDown)
        {
            usingSpecialDown = true;
            isUsingAbility = true;
            isCharging = true;
            havyAttackChargeCounter = 1;
            Disable();
        }
        else if(IsFalling() && !isUsingAbility && !usingSpecialDown)
        {
            usingSpecialDown = true;
            isUsingAbility = true;
            abilities[11].Activate();
        }
    }
    void SpecialAttackUp()
    {
        if (!isUsingAbility && !IsFalling() && !usingSpecialNormal)
        {
            usingSpecialNormal = true;
            isUsingAbility = true;
            isCharging = true;
            havyAttackChargeCounter = 1;
            Disable();  
        }
        else if (IsFalling() && !isUsingAbility && !usingSpecialUp)
        {
            usingSpecialUp = true;
            isUsingAbility = true;
            abilities[10].Activate();
        }
    }
}
