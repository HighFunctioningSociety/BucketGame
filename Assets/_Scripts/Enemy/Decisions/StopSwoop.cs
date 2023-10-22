using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopTackle")]
public class StopSwoop : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool shouldStopTackling = CheckTackleTarget(stateMachine);
        return shouldStopTackling;
    }

    private bool CheckTackleTarget(EnemyStateMachine stateMachine)
    {
        if (Vector3.Distance(stateMachine.Enemy.TargetPosition, stateMachine.transform.position) < 0.1f || stateMachine.Enemy.LOS.CheckRaycastObstacleCollision())
        {
            stateMachine.Enemy.StopMomentum();
            stateMachine.Enemy.AbilityManager.SetCooldown(1f);
            stateMachine.Enemy.Animator.SetTrigger("DiveEnd");
            return true;
        }
        return false;
    }
}
