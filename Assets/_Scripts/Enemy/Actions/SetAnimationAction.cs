using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/SetAnimation")]
public class SetAnimationAction : Actions
{
    public string animationName;
    public override void Act(EnemyStateMachine stateMachine)
    {
        SetAnimation(stateMachine);
    }

    private void SetAnimation(EnemyStateMachine stateMachine)
    {
        stateMachine.Enemy.Animator.Play(animationName);
    }
}
