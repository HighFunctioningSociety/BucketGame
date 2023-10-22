using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionController : MonoBehaviour
{
    public EnemyContainer Enemy;

    public void InitiateReposition()
    {
        Enemy.AbilityManager.nextRepositionTime = Enemy.EnemyStats.repositionCooldown + Time.time;
        Enemy.TargetPosition = GenerateTargetPosition();
    }

    public Vector2 GenerateTargetPosition()
    {
        float randRangeX = Random.Range(Enemy.EnemyStats.repositionMinDistance, Enemy.EnemyStats.repositionMaxDistance);
        int randSign = RandomSignGenerator();
        Debug.Log(randSign + "reposition sign");
        randRangeX *= randSign;
        randRangeX += transform.position.x;
        Vector2 newTargetPosition = new Vector2(randRangeX, transform.position.y);
        return newTargetPosition;
    }

    private int RandomSignGenerator()
    {
        int randNum = Random.Range(0, 2);
        return randNum == 0 ? -1 : 0;
    }
}
