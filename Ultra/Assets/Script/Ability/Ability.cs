using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : AbilityEvent
{
    string name;
    string desc;

    public Ability(string name, string desc, float timeTillActive, float activeTime, float cooldownTime, bool startActive) : base(timeTillActive, activeTime, cooldownTime, startActive)
    {
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
}
