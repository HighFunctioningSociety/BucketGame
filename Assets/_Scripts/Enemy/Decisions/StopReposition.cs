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
        if (_enemy.groundCheck.EdgeRight && _enemy.Direction < 0 || _enemy.groundCheck.EdgeLeft && _enemy.Direction > 0 || Mathf.Abs(_enemy.TargetPosition.x - _enemy.transform.position.x) < 1)
        {
            _enemy.Direction = Mathf.Sign(_enemy.transform.position.x - _enemy.TargetObject.position.x);
            _enemy.Speed = 0;
            return true;
        }
        else
        {
            return false; 
        }
    }
}
