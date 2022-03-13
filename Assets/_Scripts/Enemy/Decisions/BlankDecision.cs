using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Blank")]
public class BlankDecision : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        return true;
    }
}
