using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Reposition")]
public class Reposition : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        MoveToTargetPosition(enemy);
    }

    private void MoveToTargetPosition(EnemyContainer _enemy)
    {
        if (_enemy.targetObject == null)
            return;

        _enemy.dir = Mathf.Sign(_enemy.transform.position.x - _enemy.targetPosition.x);
        _enemy.speed = Mathf.Abs(_enemy.dir);
        _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position, _enemy.targetPosition, _enemy.enemyStats.speed * 1f * Time.fixedDeltaTime);
    }
}
