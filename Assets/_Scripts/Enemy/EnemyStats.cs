using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Stats/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Space]
    [Header("Enemy Stats")] 
    public int maxHealth = 300;
    public int damage = 1;
    public float armor = 0;
    public float speed = 1;
    public float defaultGravity = 10;
    public bool isHeavy = false;

    [Space]
    [Header ("Enemy State Variables")]
    public float idleTime = 0;
    public float fullscreenTimingMin = 0, fullscreenTimingMax = 0;
    public float repositionMinDistance = 0, repositionMaxDistance = 0;
    public float repositionCooldown = 0;
    public float aggroRangeX = 0, aggroRangeY = 0;
    public float aggroTime = 10;
    public float hurtStunTime = 0.1f;

    [Space]
    [Header ("Misc.")]
    public float stateSphereRadius = 3;

    public int currencyDroppedMin = 0;
    public int currencyDroppedMax = 0;

}
