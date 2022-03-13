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
        bool gcdComplete = _enemy.abilityManager.globalCooldownComplete;
        bool hasRetreated = _enemy.triggers.retreated;
        return gcdComplete && hasRetreated;
    }
}
