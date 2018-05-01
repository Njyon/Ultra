using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keeram : MyCharacter
{
    public float XHitNormalStunTime;

    private void Start()
    {
        XAttackNormalAction += LightAttack;
    }

    void LightAttack()
    {
        Debug.Log("LOL");
        if (XNormalHitBox)
        {
            Stun(XHitNormalStunTime);
            enemy.GetComponent<MyCharacter>().Damage(50);
            Invoke("Kick", XHitNormalStunTime);
        }
    }

    void Kick()
    {
        KickAway(enemy.transform.position);
    }
}
