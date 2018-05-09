using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : AbilityEvent
{
    int damage;
    string name;
    string desc;

    public Ability(string name, string desc, int damage, float timeTillActive, float activeTime, float cooldownTime, bool startActive) : base(timeTillActive, activeTime, cooldownTime, startActive)
    {
        this.damage = damage;
        this.name = name;
        this.desc = desc;
    }

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return desc;
    }

    public int GetDamage()
    {
        return damage;
    }
}
