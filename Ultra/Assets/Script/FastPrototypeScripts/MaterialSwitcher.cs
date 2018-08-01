using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material AButton;
    public Material XButton;
    public Renderer rend;
    public Movement mov;

    bool isXButton = false;
    void Update()
    {
        if (mov.fallComp == null)
            return;

        if (mov.fallComp.isFalling && !isXButton)
        {
            isXButton = true;
            rend.material = XButton;
        }
        else if(!mov.fallComp.isFalling && isXButton)
        {
            isXButton = false;
            rend.material = AButton;
        }
    }
}