using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Chase(enemy);
    }

    private void Chase(EnemyContainer _enemy)
    {
        if (_enemy.targetObject == null)
            return;

        _enemy.dir = Mathf.Sign(_enemy.transform.position.x - _enemy.targetObject.position.x);
        _enemy.speed = Mathf.Abs(_enemy.dir);
        if ((_enemy.groundCheck.edgeRight && _enemy.dir < 0) || (_enemy.groundCheck.edgeLeft && _enemy.dir > 0))
        {
            _enemy.speed = 0;
            return;
        }


        Vector2 newTarget = new Vector2(_enemy.targetObject.position.x, _enemy.transform.position.y);

        if (Mathf.Abs(newTarget.x - _enemy.transform.position.x) > 3)
        { 
            _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position, newTarget, _enemy.enemyStats.speed * 1f * Time.fixedDeltaTime);
        }
        else
        {
            _enemy.dir = 0;
            _enemy.speed = 0;
            return;
        }
    }
}
