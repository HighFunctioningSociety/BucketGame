using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public ScriptableBool scriptableBool;
    public bool multiUse = false;
    private Animator animator;
    private AudioSource audioSource;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        SetState();
    }

    public void FlipSwitch()
    {
        if (scriptableBool.state != true || multiUse)
        {
            scriptableBool.state = !scriptableBool.state;
            audioSource.Play();
            SetState();
        }
    }

    public void SetState()
    {
        animator.SetBool("State", scriptableBool.state);
    }
}
