using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/FlyingChase")]
public class FlyingChaseAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Chase(enemy);
    }

    private void Chase(EnemyContainer _enemy)
    {
        if (_enemy.TargetObject == null)
            return;
        _enemy.Direction = Mathf.Sign(_enemy.transform.position.x - _enemy.TargetObject.position.x);
        _enemy.Speed = Mathf.Abs(_enemy.Direction);
        Vector2 newTarget = new Vector2(_enemy.TargetObject.position.x, _enemy.TargetObject.position.y + 5);
        Vector2 direction = new Vector2((newTarget.x - _enemy.transform.position.x), (newTarget.y - _enemy.transform.position.y));
        float directionMagnitude = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
        Vector2 directionUnit = direction / directionMagnitude;
        _enemy.RigidBody.AddForce(directionUnit * _enemy.enemyStats.speed, ForceMode2D.Impulse);
    }
}
