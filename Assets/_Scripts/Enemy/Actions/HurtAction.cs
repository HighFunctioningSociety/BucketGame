using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Hurt")]
public class HurtAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Hurt(enemy);
    }

    private void Hurt(EnemyContainer _enemy)
    {
        _enemy.dir = 0;
        _enemy.speed = 0;
    }
}
