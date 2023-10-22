using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/HurtTimeOver")]
public class HurtTimerOver : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool hurtOver = CheckHurtTime(stateMachine);
        return hurtOver;
    }

    private bool CheckHurtTime(EnemyStateMachine stateMachine)
    {
        return (stateMachine.StateTimeElapsed >= stateMachine.CurrentState.TimeToRemainInState);
    }
}
