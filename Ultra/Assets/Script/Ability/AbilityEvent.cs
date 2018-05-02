using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityEvent
{
    public Action onAbilityStart;
    public Action<float> onAbilityTick;
    public Action onAbilityEnd;
    public Action onAbilityReady;


    AbilityState state = AbilityState.EventReady;
    float remainingTime;
}
