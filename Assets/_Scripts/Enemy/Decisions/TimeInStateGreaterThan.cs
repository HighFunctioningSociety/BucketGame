using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/TimeInStateGreaterThan")]
public class TimeInStateGreaterThan : Decision
{
    public float time;
    public override bool Decide(EnemyContainer enemy)
    {
        bool isStateTimeGreaterThan = CheckStateTime(enemy);
        return isStateTimeGreaterThan;
    }

    private bool CheckStateTime(EnemyContainer _enemy)
    {
        return (_enemy.StateTimeElapsed > time);
    }
}
