using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackedXTimes")]
public class AttackedXTimes : Decision
{
    public int numberOfAttacks;

    public override bool Decide(EnemyContainer enemy)
    {
        bool doneAttacking = HasAttackedXTimes(enemy);
        return doneAttacking;
    }

    private bool HasAttackedXTimes(EnemyContainer _enemy)
    {
        if (_enemy.attacksDoneInState < numberOfAttacks)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
