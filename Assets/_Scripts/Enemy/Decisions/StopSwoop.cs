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
        if (_enemy.StateTimeElapsed > 3f || (_enemy.transform.position.y >= _enemy.StartingPosition.y && _enemy.StateTimeElapsed > 0.1f) || _enemy.LOSCaster.CheckRaycastObstacleCollision())
        {
            _enemy.StopMomentum();
            _enemy.AbilityManager.SetCooldown(1f);
            _enemy.Animator.SetTrigger("DiveEnd");
            return true;
        }
        return false;
    }
}
