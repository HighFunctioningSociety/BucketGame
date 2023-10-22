using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "PluggableAI/Decisions/InIdleAnimation")]
public class InIdleAnimation : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool isIdlePlaying = IsIdlePlaying(stateMachine);
        return isIdlePlaying;
    }
    private bool IsIdlePlaying(EnemyStateMachine stateMachine)
    {
        return stateMachine.Enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }
}
