using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Actions
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        Chase(stateMachine);
    }

    private void Chase(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.TargetObject == null)
            return;

        stateMachine.Enemy.Direction = Mathf.Sign(stateMachine.transform.position.x - stateMachine.Enemy.TargetObject.position.x);
        stateMachine.Enemy.Speed = Mathf.Abs(stateMachine.Enemy.Direction);
        if ((stateMachine.Enemy.GroundCheck.EdgeRight && stateMachine.Enemy.Direction < 0) || (stateMachine.Enemy.GroundCheck.EdgeLeft && stateMachine.Enemy.Direction > 0))
        {
            stateMachine.Enemy.Speed = 0;
            return;
        }


        Vector2 newTarget = new Vector2(stateMachine.Enemy.TargetObject.position.x, stateMachine.transform.position.y);

        if (Mathf.Abs(newTarget.x - stateMachine.transform.position.x) > 3)
        { 
            stateMachine.transform.position = Vector2.MoveTowards(stateMachine.transform.position, newTarget, stateMachine.Enemy.EnemyStats.speed * 1f * Time.fixedDeltaTime);
        }
        else
        {
            stateMachine.Enemy.Direction = 0;
            stateMachine.Enemy.Speed = 0;
            return;
        }
    }
}
