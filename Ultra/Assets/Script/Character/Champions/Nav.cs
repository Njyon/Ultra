using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nav : MyCharacter
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
        Instantiate(lol, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z), gameObject.transform.rotation);
        Debug.Log(gameObject.name + " : Xhit");
        if (XNormalHitBox)
        {
            Stun(XHitNormalStunTime);
            enemy.GetComponent<MyCharacter>().Damage(50);
            Invoke("Kick", XHitNormalStunTime);
        }
    }
}
