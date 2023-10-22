using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/TimeInStateGreaterThan")]
public class TimeInStateGreaterThan : Decision
{
    public float time;
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool isStateTimeGreaterThan = CheckStateTime(stateMachine);
        return isStateTimeGreaterThan;
    }

    private bool CheckStateTime(EnemyStateMachine stateMachine)
    {
        return (stateMachine.StateTimeElapsed > time);
    }
}
