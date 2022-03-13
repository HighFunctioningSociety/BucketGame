using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAbility/EnemyProjectile")]
public class EnemyProjectileAbilities : EnemyAbility
{
    private EnemyProjectileTriggerable attack;

    public int damage;
    public float speed;
    public float knockBack;
    public string triggerName;
    public bool isTracking;
    public GameObject projectilePrefab;

    public override void Initialize(GameObject obj)
    {
        attack = obj.GetComponent<EnemyProjectileTriggerable>();

        //overwrite projectile values with the values in the scriptable object
        attack.damage = damage;
        attack.speed = speed;
        attack.knockBack = knockBack;
        attack.triggerName = triggerName;
        attack.projectilePrefab = projectilePrefab;
        attack.isTracking = isTracking;
    }

    public override void TriggerAbility()
    {
        attack.Trigger();
    }
}
