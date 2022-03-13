using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/GuardBroken")]
public class GuardBroken : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool guardBroken = false;//IsGuardBroken(enemy);
        return guardBroken;
    }

    //private bool IsGuardBroken(EnemyContainer enemy)
    //{
    //    return enemy.guardMeter.guardBroken;
    //}
}
