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
        if (_enemy.TargetObject == null)
            return;

        _enemy.Direction = Mathf.Sign(_enemy.transform.position.x - _enemy.TargetObject.position.x);
        _enemy.Speed = Mathf.Abs(_enemy.Direction);
        if ((_enemy.groundCheck.EdgeRight && _enemy.Direction < 0) || (_enemy.groundCheck.EdgeLeft && _enemy.Direction > 0))
        {
            _enemy.Speed = 0;
            return;
        }


        Vector2 newTarget = new Vector2(_enemy.TargetObject.position.x, _enemy.transform.position.y);

        if (Mathf.Abs(newTarget.x - _enemy.transform.position.x) > 3)
        { 
            _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position, newTarget, _enemy.enemyStats.speed * 1f * Time.fixedDeltaTime);
        }
        else
        {
            _enemy.Direction = 0;
            _enemy.Speed = 0;
            return;
        }
    }
}
