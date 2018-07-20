using System.Collections.Generic;
using UnityEngine;

public class MainMenuCam : MonoBehaviour
{
    public Animator animator;
    public bool on;

    public void Play()
    {
        if(on)
            animator.SetBool("Play", true);
    }
    public void ReversePlaye()
    {
        if (on)
            animator.SetBool("ReversePlay", true);
    }

    public void SetBoolsBackToDefault()
    {
        if (on)
        {
            animator.SetBool("Play", false);
            animator.SetBool("ReversePlay", false);

        }
    }
}
