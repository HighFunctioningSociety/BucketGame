using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Abilities/Projectile"))]
public class ProjectileAbilities : Ability
{
    private ProjectileAttackTriggerable attack;

    public int damage;
    public float speed;
    public float knockBack;
    public float moveForceX, moveForceY;
    public string triggerName;
    public float shakeLength = 0;
    public float shakeAmplitude = 0;
    public float rumbleDurration;
    public float rumbleLow;
    public float rumbleHigh;
    public GameObject projectilePrefab;

    public override void Initialize(GameObject obj)
    {
        attack = obj.GetComponent<ProjectileAttackTriggerable>();

        //overwrite projectile values with the values in the scriptable object
        attack.damage = damage;
        attack.speed = speed;
        attack.knockBack = knockBack;
        attack.moveForceX = moveForceX;
        attack.moveForceY = moveForceY;
        attack.triggerName = triggerName;
        attack.shakeLength = shakeLength;
        attack.shakeAmplitude = shakeAmplitude;
        attack.rumbleDurration = rumbleDurration;
        attack.rumbleLow = rumbleLow;
        attack.rumbleHigh = rumbleHigh;
        attack.projectilePrefab = projectilePrefab;
    }

    public override void TriggerAbility()
    {
        attack.Trigger();
    }
}
