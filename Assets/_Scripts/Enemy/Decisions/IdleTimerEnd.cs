using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IdleTimerEnd")]
public class IdleTimerEnd : Decision
{
    public override bool Decide(EnemyStateMachine enemy)
    {
        bool TimeToRemainInStateReached = CheckTimer(enemy);
        return TimeToRemainInStateReached;
    }
    
    private bool CheckTimer(EnemyStateMachine enemy)
    {
        return enemy.StateTimeElapsed >= enemy.CurrentState.TimeToRemainInState;
    }
}
