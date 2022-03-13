using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopReposition")]
public class StopReposition : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool shouldStopRepostion = CheckPathToTarget(enemy);
        return shouldStopRepostion;
    }
    private bool CheckPathToTarget(EnemyContainer _enemy)
    {
        if (_enemy.groundCheck.edgeRight && _enemy.dir < 0 || _enemy.groundCheck.edgeLeft && _enemy.dir > 0 || Mathf.Abs(_enemy.targetPosition.x - _enemy.transform.position.x) < 1)
        {
            _enemy.dir = Mathf.Sign(_enemy.transform.position.x - _enemy.targetObject.position.x);
            _enemy.speed = 0;
            return true;
        }
        else
        {
            return false; 
        }
    }
}
