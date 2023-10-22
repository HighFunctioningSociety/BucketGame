using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/AnimationHasChanged")]
public class AnimationChangeDecision : Decision
{
    public string animationName;
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool hasAnimationChanged = CheckAnimationName(stateMachine);
        return hasAnimationChanged;
    }

    private bool CheckAnimationName(EnemyStateMachine stateMachine)
    {
        return (stateMachine.Enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) == false) && stateMachine.StateTimeElapsed > 0.5f;
    }
}
