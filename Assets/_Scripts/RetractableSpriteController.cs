using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractableSpriteController : MonoBehaviour
{
    public float retractionTiming;
    public bool isRetracted;
    private float nextRetractionTime;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (CheckTimer())
            TriggerSpikes();
    }

    private void TriggerSpikes()
    {
        isRetracted = !isRetracted;
        animator.SetBool("IsRetracted", isRetracted);
        nextRetractionTime = retractionTiming + Time.time;
    }

    private bool CheckTimer()
    {
        return (nextRetractionTime <= Time.time);
    }
}
