using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopReposition")]
public class StopReposition : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool shouldStopRepostion = CheckPathToTarget(stateMachine);
        return shouldStopRepostion;
    }
    private bool CheckPathToTarget(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.GroundCheck.EdgeRight && stateMachine.Enemy.Direction < 0 || stateMachine.Enemy.GroundCheck.EdgeLeft && stateMachine.Enemy.Direction > 0 || Mathf.Abs(stateMachine.Enemy.TargetPosition.x - stateMachine.transform.position.x) < 1)
        {
            stateMachine.Enemy.Direction = Mathf.Sign(stateMachine.transform.position.x - stateMachine.Enemy.TargetObject.position.x);
            stateMachine.Enemy.Speed = 0;
            return true;
        }
        else
        {
            return false; 
        }
    }
}
