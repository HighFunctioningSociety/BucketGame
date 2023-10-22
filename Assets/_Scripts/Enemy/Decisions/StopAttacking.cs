using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopAttacking")]
public class StopAttacking : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool stopAttacking = EnteredIdleAnimation(stateMachine);
        return stopAttacking;
    }

    private bool EnteredIdleAnimation(EnemyStateMachine stateMachine)
    {
        return stateMachine.Enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && stateMachine.Enemy.InIdle == false && stateMachine.StateTimeElapsed > 0.1f;
    }
}

