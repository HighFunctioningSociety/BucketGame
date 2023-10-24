using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Idle")]
public class IdleAction : Action
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        Idle(stateMachine);
    }

    private void Idle(EnemyStateMachine stateMachine)
    {
        stateMachine.Enemy.Direction = 0;
        stateMachine.Enemy.Speed = 0;
    }
}
