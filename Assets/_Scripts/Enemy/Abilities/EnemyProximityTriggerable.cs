using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyProximityTriggerable : EnemyTriggerable
{
    [HideInInspector] public string triggerName;
    [HideInInspector] public float rangeX;
    [HideInInspector] public float rangeY;
}
