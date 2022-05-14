using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtComponent : MonoBehaviour
{
    public EnemyContainer Enemy;
    public State HurtState;
    public Decision[] HurtDecisions;

    private bool CheckHurtConditions()
    {
        if (HurtDecisions.Length == 0)
            return true;

        bool meetsConditions = true;

        foreach (Decision condition in HurtDecisions)
        {
            if (condition.Decide(Enemy) == false)
                meetsConditions = false;
        }

        return meetsConditions;
    }

    public void EnterHurtState()
    {
        if (CheckHurtConditions() && !Enemy.currentState.uninterruptable)
        {
            Enemy.currentState = HurtState;
            Enemy.StopMomentum();
            Enemy.ResetAnimationTriggers();
            Enemy.UnfreezeConstraints();

            TriggerHurtAnimation();

            if (Enemy.LOSCaster != null)
            {
                Enemy.LOSCaster.LOSCount = 0;
            }
        }
    }

    private void TriggerHurtAnimation()
    {
        if (!Enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            Enemy.Animator.SetTrigger("Hurt");
        }
    }

}
