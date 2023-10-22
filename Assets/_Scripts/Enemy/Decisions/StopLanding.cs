using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopLanding")]
public class StopLanding : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool grounded = CheckGroundAndIdle(stateMachine);
        return grounded;
    }

    private bool CheckGroundAndIdle(EnemyStateMachine stateMachine)
    {
        return stateMachine.Enemy.GroundCheck.Grounded && stateMachine.Enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }
}
