using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    private PlayerContainer player;
    private EnemyStateMachine enemy;
    private int damage = 1;

    private void Start()
    {
        player = GetComponentInParent<PlayerContainer>();
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if ((enemy = _colInfo.GetComponent<EnemyStateMachine>()) || (enemy = _colInfo.GetComponentInParent<EnemyStateMachine>()))
        {
            if (enemy.CurrentState.hurtboxDisabled)
                return;
            
            DefaultCollision();
        }
    }

    private void OnTriggerStay2D(Collider2D _colInfo)
    {
        if ((enemy = _colInfo.GetComponent<EnemyStateMachine>()) || (enemy = _colInfo.GetComponentInParent<EnemyStateMachine>()))
        {
            if (enemy.CurrentState.hurtboxDisabled)
                return;

            DefaultCollision();
        }
    }

    public void DisableCollider()
    {

    }

    public void EnableCollider()
    {

    }

    private void DefaultCollision()
    {
        player._KnockBack(300, 150, enemy.transform.position);
        player._DamagePlayer(damage, false);
    }
}
