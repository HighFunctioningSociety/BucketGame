using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyDefeatEvent : MonoBehaviour
{
    public EnemyContainer enemy;
    public UnityEvent OnEnemyDefeatEvent;

    private void Awake()
    {
        if (OnEnemyDefeatEvent == null)
            OnEnemyDefeatEvent = new UnityEvent();
    }

    private void Update()
    {
        if (enemy.gameObject.activeSelf == false)
            OnEnemyDefeatEvent.Invoke();
    }
}
