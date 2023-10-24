using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Hurt")]
public class HurtAction : Action
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        Hurt(stateMachine);
    }

    private void Hurt(EnemyStateMachine stateMachine)
    {
        stateMachine.Enemy.Direction = 0;
        stateMachine.Enemy.Speed = 0;
    }
}
