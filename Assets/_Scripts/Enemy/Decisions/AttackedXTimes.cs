using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackedXTimes")]
public class AttackedXTimes : Decision
{
    public int numberOfAttacks;

    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool doneAttacking = HasAttackedXTimes(stateMachine);
        return doneAttacking;
    }

    private bool HasAttackedXTimes(EnemyStateMachine stateMachine)
    {
        if (stateMachine.AttacksDoneInState < numberOfAttacks)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
