using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/ResetBellBossTriggers")]
public class ResetBellBossTriggers : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        ResetAnimationTriggers(enemy);
    }

    private void ResetAnimationTriggers(EnemyContainer _enemy)
    {
        if (_enemy.StateTimeElapsed < 0.05f)
        {
            _enemy.Animator.ResetTrigger("Blast");
            _enemy.Animator.ResetTrigger("Jump");
            _enemy.Animator.ResetTrigger("LowPunch");
            _enemy.Animator.ResetTrigger("HighPunch");
            _enemy.Animator.ResetTrigger("Hurt");
        }
    }
}
