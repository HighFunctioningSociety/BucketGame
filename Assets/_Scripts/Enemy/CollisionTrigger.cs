using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    private PlayerContainer player;
    private EnemyContainer enemy;
    private int damage = 1;

    private void Start()
    {
        player = GetComponentInParent<PlayerContainer>();
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if ((enemy = _colInfo.GetComponent<EnemyContainer>()) || (enemy = _colInfo.GetComponentInParent<EnemyContainer>()))
        {
            if (enemy.currentState.hurtboxDisabled)
            {
                return;
            }
            else
            {
                DefaultCollision();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D _colInfo)
    {
        if ((enemy = _colInfo.GetComponent<EnemyContainer>()) || (enemy = _colInfo.GetComponentInParent<EnemyContainer>()))
        {
            if (enemy.currentState.hurtboxDisabled)
            {
                return;
            }
            else
            {
                DefaultCollision();
            }
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
