using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EncounterEventTrigger : MonoBehaviour
{
    public bool triggeredThisScene = false;
    public UnityEvent OnTriggerEnterEvent;

    private void Awake()
    {
        if (OnTriggerEnterEvent == null)
            OnTriggerEnterEvent = new UnityEvent();
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (triggeredThisScene == true)
            return;

        PlayerContainer _player = _colInfo.GetComponent<PlayerContainer>();

        if (_player != null)
        {
            OnTriggerEnterEvent.Invoke();
            triggeredThisScene = true;
        }
    }
}
