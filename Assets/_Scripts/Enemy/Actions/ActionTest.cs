using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Test")]
public class ActionTest : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        DoThing(enemy);
    }

    private void DoThing(EnemyContainer enemy)
    {
        enemy.transform.position += new Vector3 (100,0);
    }
}
