using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlankerAnimator : MonoBehaviour
{
    public static BlankerAnimator blanker;
    public Animator animator;
    public bool fadeIn = true;
    public bool transitionInProgress = false;

    public void Awake()
    {
        if (blanker != null)
        {
            Destroy(gameObject);
        }
        else
        {
            blanker = this;
        }

        animator.SetBool("FadeIn", fadeIn);
    }

    public void FadeOut()
    {
        transitionInProgress = true;
        fadeIn = false;
        animator.SetBool("FadeIn", fadeIn);
    }

    public void FadeIn()
    {
        transitionInProgress = true;
        fadeIn = true;
        animator.SetBool("FadeIn", fadeIn);
    }

    public void FinishedTransition()
    {
        transitionInProgress = false;
    }

    public void ResetBlanker()
    {
        animator.SetTrigger("ResetBlanker");
        FadeIn();
    }

    public void TestGuy()
    {
        Debug.Log("Loaded!");
    }
}
