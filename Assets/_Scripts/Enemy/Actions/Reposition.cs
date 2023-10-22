using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Reposition")]
public class Reposition : Actions
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        MoveToTargetPosition(stateMachine);
    }

    private void MoveToTargetPosition(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.TargetObject == null)
            return;

        stateMachine.Enemy.Direction = Mathf.Sign(stateMachine.transform.position.x - stateMachine.Enemy.TargetPosition.x);
        stateMachine.Enemy.Speed = Mathf.Abs(stateMachine.Enemy.Direction);
        stateMachine.transform.position = Vector2.MoveTowards(stateMachine.transform.position, stateMachine.Enemy.TargetPosition, stateMachine.Enemy.EnemyStats.speed * 1f * Time.fixedDeltaTime);
    }
}
