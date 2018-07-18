﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugChar : MyCharacter
{
    public void InitDebug()
    {
        rb = GetComponent<Rigidbody>();
        ui = GameObject.Find("PlayerTwoPannel").GetComponent<InGameUI>();
        Posses();
    }

    void Update()
    {
        Bounce();
    }
}