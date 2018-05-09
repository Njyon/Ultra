using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEvent
{
    public Action onAbilityStart;
    public Action onAbilityCancel;
    public Action onAbilityEnd;
    public Action onAbilityReady;
    public Action onAbilityUpdate;
    public bool hitObject = false;

    AbilityState state = AbilityState.EventReady;
    float startTimeStamp;
    float timetillActiveHelper;
    float activeTimeHelper;
    float cooldownTimeHelper;
    bool timerActive = false;
   
    float timeTillActive;
    float activeTime;
    float cooldownTime;

    public AbilityEvent(float timeTillActive = 1, float activeTime = 1, float cooldownTime = 2, bool startActive = true)
    {
        this.timeTillActive = timeTillActive;
        this.activeTime = activeTime;
        this.cooldownTime = cooldownTime;
        this.state = (startActive == true ? AbilityState.EventActive : AbilityState.EventReady);
    }

    public bool IsActive()
    {
        return (this.state == AbilityState.EventActive);
    }

    public bool IsCooling()
    {
        return (this.state == AbilityState.EventCoolingdown);
    }

    public void Activate()
    {
        if(!IsActive() && !IsCooling())
        {
            state = AbilityState.EventActive;
            timetillActiveHelper = timeTillActive;
            activeTimeHelper = activeTime;
            timerActive = true;
            onAbilityStart();
        }
    }

    public void Cancel()
    {
        if(IsActive())
        {
            state = AbilityState.EventCoolingdown;
            onAbilityCancel();
        }
    }

    public void Update()
    {
        if(IsActive() && timerActive)
        {
            timetillActiveHelper -= Time.deltaTime;
            if(timetillActiveHelper <= 0)
            {
                timerActive = false;
            }
        }
        else if(IsActive())
        {
            onAbilityUpdate();
            activeTimeHelper -= Time.deltaTime;
            if(activeTimeHelper <= 0)
            {
                state = AbilityState.EventCoolingdown;
                cooldownTimeHelper = cooldownTime;
                onAbilityEnd();
            }
        }
        else if(IsCooling())
        {
            cooldownTimeHelper -= Time.deltaTime;
            if(cooldownTimeHelper <= 0)
            {
                state = AbilityState.EventReady;
                hitObject = false;
            }
        }
    }
}
