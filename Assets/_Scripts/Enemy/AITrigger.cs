using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrigger : MonoBehaviour
{
    public EnemyContainer enemy;

    public void ActivateEnemy() { 
        enemy.aiActive = true;
    }
}
