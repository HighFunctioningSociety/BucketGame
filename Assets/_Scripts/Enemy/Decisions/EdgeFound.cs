using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/EdgeFound")]
public class EdgeFound : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool edgeFound = CheckForEdge(enemy);
        return edgeFound;
    }

    private bool CheckForEdge(EnemyContainer _enemy)
    {
        if (_enemy.groundCheck.EdgeLeft  && _enemy.patrolDirection == -1)
        {
            _enemy.groundCheck.LeftEdgeAlreadyFound = true;
            _enemy.patrolDirection *= -1;
            return true;
        }
        else if (_enemy.groundCheck.EdgeRight && _enemy.patrolDirection == 1)
        {
            _enemy.groundCheck.RightEdgeAlreadyFound = true;
            _enemy.patrolDirection *= -1;
            return true;
        }
        return false;
    }
}
