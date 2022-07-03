using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasRetreated : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        return RetreatFinished(enemy);
    }

    private bool RetreatFinished(EnemyContainer _enemy)
    {
        bool gcdComplete = _enemy.AbilityManager.globalCooldownComplete;
        bool hasRetreated = _enemy.Triggers.retreated;
        return gcdComplete && hasRetreated;
    }
}
