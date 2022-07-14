using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformWiggle : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayPlatformWiggleAnimation()
    {
        animator.SetTrigger("Wiggle");
    }
}
