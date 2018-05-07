using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav : MyCharacter
{
    public float XHitNormalStunTime;
    Ability[] abilities = new Ability[10];

    private void Start()
    {
        DefineAbilities();
        XAttackNormalAction += LightAttack;
        XAttackRightAction += LightAttackRight;
    }

    private void OnDisable()
    {
        XAttackNormalAction -= LightAttack;
    }

    void DefineAbilities()
    {
        abilities[0] = new Ability(
            "Light Attack",
            "A Light Attack in view Direktion, its fast and does some Damage",
            0.5f,
            0.2f,
            0.3f,
            false
            );

        abilities[0].onAbilityStart = () => 
        {
            Debug.Log("Start");
        };
        abilities[0].onAbilityEnd = () => 
        {
            Debug.Log("End");
        };
        abilities[0].onAbilityCancel = () => 
        {
            Debug.Log("Canel");
        };
        abilities[0].onAbilityUpdate = () => 
        {
            Debug.Log("TriggerAttack");
        };
        abilities[0].onAbilityReady = () => 
        {
            Debug.Log("Ready");
        };
    }

    void Update()
    {
        abilities[0].Update();
    }

    void LightAttack()
    {
        Debug.Log("X");
        abilities[0].Activate();
    }

    void LightAttackRight()
    {
        abilities[0].Cancel();
    }
}
