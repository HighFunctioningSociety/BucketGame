using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StartReposition")]
public class RepositionDecision : Decision
{
    [HideInInspector] public EnemyContainer enemy;

    public override bool Decide(EnemyContainer enemy)
    {
        bool shouldReposition = ShouldTriggerReposition(enemy);
        return shouldReposition;
    }

    private bool ShouldTriggerReposition(EnemyContainer _enemy)
    {
        if (_enemy.abilityManager.repositionCooldownComplete)
        {
            _enemy.InitiateReposition();
            Debug.Log("reposition");
            return true;
        }
        else
        {
            return false;
        }
    }
}
