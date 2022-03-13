using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Idle")]
public class IdleAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Idle(enemy);
    }

    private void Idle(EnemyContainer _enemy)
    {
        _enemy.dir = 0;
        _enemy.speed = 0;
    }
}
