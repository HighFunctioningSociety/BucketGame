using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopTackle")]
public class StopSwoop : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool shouldStopTackling = CheckTackleTarget(enemy);
        return shouldStopTackling;
    }

    private bool CheckTackleTarget(EnemyContainer _enemy)
    {
        if (_enemy.stateTimeElapsed > 3f || (_enemy.transform.position.y >= _enemy.startingPosition.y && _enemy.stateTimeElapsed > 0.1f) || _enemy.CheckRaycastObstacleCollision())
        {
            _enemy.StopMomentum();
            _enemy.abilityManager.SetCooldown(1f);
            _enemy.animator.SetTrigger("DiveEnd");
            return true;
        }
        return false;
    }
}
