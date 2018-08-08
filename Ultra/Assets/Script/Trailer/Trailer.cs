using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailer : MonoBehaviour
{
    public Animator nav;

    public void PlayAnim()
    {
        nav.SetBool("outOfIdle", true);
    }
}