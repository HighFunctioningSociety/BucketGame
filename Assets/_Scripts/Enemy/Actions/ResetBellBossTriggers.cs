using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/ResetBellBossTriggers")]
public class ResetBellBossTriggers : Action
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        ResetAnimationTriggers(stateMachine);
    }

    private void ResetAnimationTriggers(EnemyStateMachine stateMachine)
    {
        if (stateMachine.StateTimeElapsed < 0.05f)
        {
            stateMachine.Enemy.Animator.ResetTrigger("Blast");
            stateMachine.Enemy.Animator.ResetTrigger("Jump");
            stateMachine.Enemy.Animator.ResetTrigger("LowPunch");
            stateMachine.Enemy.Animator.ResetTrigger("HighPunch");
            stateMachine.Enemy.Animator.ResetTrigger("Hurt");
        }
    }
}
