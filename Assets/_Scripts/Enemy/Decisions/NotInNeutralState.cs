using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/NotInNeutralState")]
public class NotInNeutralState : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool isStateNeutral = CheckIfStateIsNeutral(enemy);
        return isStateNeutral;
    }

    private bool CheckIfStateIsNeutral(EnemyContainer _enemy)
    {
        return _enemy.inIdle == false;
    }
}
