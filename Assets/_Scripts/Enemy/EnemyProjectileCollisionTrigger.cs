using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileCollisionTrigger : MonoBehaviour
{
    public Collider2D projectileCollider;
    public int damage;

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        PlayerContainer _player = _colInfo.GetComponentInParent<PlayerContainer>();
        if (_player != null)
        {
            _player._KnockBack(300, 100, projectileCollider.bounds.center);
            _player._DamagePlayer(damage, false);
        }
    }
}
