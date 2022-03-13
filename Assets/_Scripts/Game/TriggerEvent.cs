using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public ScriptableBool scriptableBool;
    public UnityEvent OnSwitchChange;
    public bool currentState;

    public void Start()
    {
        if (OnSwitchChange == null)
        {
            OnSwitchChange = new UnityEvent();
        }
        currentState = false;
    }

    public void Update()
    {
        if (scriptableBool.state != currentState)
        {
            OnSwitchChange.Invoke();
            currentState = scriptableBool.state;
        }
    }
}
