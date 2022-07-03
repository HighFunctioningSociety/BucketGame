using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/JuggleGravity")]
public class JuggleGravity : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        ChangeGravity(enemy);
    }

    private void ChangeGravity(EnemyContainer enemy)
    {   if (enemy.RigidBody.velocity.y < 0)
        {
            float newGravity = Mathf.Clamp(enemy.enemyStats.defaultGravity - 7 + enemy.StateTimeElapsed, 1, enemy.enemyStats.defaultGravity);
            enemy.RigidBody.gravityScale = newGravity;
        }
        else
        {
            enemy.RigidBody.gravityScale = enemy.enemyStats.defaultGravity;
        }
    }
}
