using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTiggerEvents: MonoBehaviour
{
    public UnityEvent<GameObject> TriggerEnterEvent;
    public UnityEvent<GameObject>TriggerExitEvent;

    private void Start()
    {
        if (TriggerEnterEvent == null)
            TriggerEnterEvent = new UnityEvent<GameObject>();

        if (TriggerExitEvent == null)
            TriggerExitEvent = new UnityEvent<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null)
            TriggerEnterEvent.Invoke(_colInfo.gameObject);
    }

    private void OnTriggerExit2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null)
            TriggerExitEvent.Invoke(_colInfo.gameObject);
    }
}
