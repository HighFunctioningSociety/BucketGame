using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class HurtController : MonoBehaviour
{
    public EnemyStateMachine StateMachine;
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
            if (condition.Decide(StateMachine) == false)
                meetsConditions = false;
        }

        return meetsConditions;
    }

    public void EnterHurtState()
    {
        if (CheckHurtConditions() && !StateMachine.CurrentState.uninterruptable)
        {
            StateMachine.CurrentState = HurtState;
            Enemy.StopMomentum();
            Enemy.ResetAnimationTriggers();
            Enemy.UnfreezeConstraints();

            TriggerHurtAnimation();

            if (Enemy.LOS != null)
            {
                Enemy.LOS.LOSHitCount = 0;
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
