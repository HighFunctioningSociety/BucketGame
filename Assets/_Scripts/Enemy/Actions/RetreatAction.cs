using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Retreat")]
public class RetreatAction : Action
{
    public string AnimationName;
    public override void Act(EnemyStateMachine stateMachine)
    {
        Retreat(stateMachine);
    }

    private void Retreat(EnemyStateMachine stateMachine)
    {
        if (!stateMachine.Enemy.Triggers.retreated )
        {
            Debug.Log("Retreat");
            stateMachine.Enemy.AbilityManager.globalCooldownComplete = false;
            stateMachine.Enemy.AbilityManager.nextReadyTime = Time.time + 1.5f;
            stateMachine.Enemy.Triggers.retreated = true;
            stateMachine.Enemy.Animator.Play(AnimationName);
        }
    }
}
