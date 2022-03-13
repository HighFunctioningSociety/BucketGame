using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitReactor : MonoBehaviour
{
    public UnityEvent OnHitEvent;

    private void Awake()
    {
        if (OnHitEvent == null)
            OnHitEvent = new UnityEvent();
    }

    public void ReactToHit()
    {
        OnHitEvent.Invoke();
    }
}
