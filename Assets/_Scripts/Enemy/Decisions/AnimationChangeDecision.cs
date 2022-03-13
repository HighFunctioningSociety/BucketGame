using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/AnimationHasChanged")]
public class AnimationChangeDecision : Decision
{
    public string animationName;
    public override bool Decide(EnemyContainer enemy)
    {
        bool hasAnimationChanged = CheckAnimationName(enemy);
        return hasAnimationChanged;
    }

    private bool CheckAnimationName(EnemyContainer _enemy)
    {
        return (_enemy.animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) == false) && _enemy.stateTimeElapsed > 0.5f;
    }
}
