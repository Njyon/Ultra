using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keeram : MyCharacter
{
    public float XHitNormalStunTime;
    public GameObject lol;

    private void Start()
    {
        XAttackNormalAction += LightAttack;
    }

    private void OnDisable()
    {
        XAttackNormalAction -= LightAttack;
    }

    void LightAttack()
    {

    }
}
