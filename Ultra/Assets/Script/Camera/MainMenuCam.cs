using System.Collections.Generic;
using UnityEngine;

public class MainMenuCam : MonoBehaviour
{
    public Animator animator;

    public void Play()
    {
        animator.SetBool("Play", true);
    }
    public void ReversePlaye()
    {
        animator.SetBool("ReversePlay", true);
    }

    public void SetBoolsBackToDefault()
    {
        animator.SetBool("Play", false);
        animator.SetBool("ReversePlay", false);
    }
}
