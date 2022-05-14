using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAbility/EnemyMelee")]
public class EnemyMeleeAbility : EnemyProximityAbility
{
    [HideInInspector] public EnemyMeleeTriggerable attack;

    public int damage;
    public float knockBackX;
    public float knockBackY;
    public float moveForceX;
    public float moveForceY;


    public override void Initialize(GameObject obj)
    {
        attack = obj.GetComponent<EnemyMeleeTriggerable>();

        // overwrite melee attack values with the values for this attack
        attack.Damage = damage;
        attack.rangeX = rangeX;
        attack.rangeY = rangeY;
        attack.MoveForceX = moveForceX;
        attack.moveForceY = moveForceY;
        attack.KnockBackX = knockBackX;
        attack.KnockBackY = knockBackY;
        attack.AnimationName = aName;
    }

    public override void TriggerAbility()
    {
        attack.Trigger();
    }
}
