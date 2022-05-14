using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAbility/EnemyMovement")]
public class EnemyForceAbility : EnemyAbility
{
    [HideInInspector] public EnemyMoveTriggerable movement;

    public float verticalForce;
    public float horizontalForce;
    public ForceMode2D forceMode;
    //public string AnimationName;

    public override void Initialize(GameObject obj)
    {
        movement = obj.GetComponent<EnemyMoveTriggerable>();

        movement.verticalForce = verticalForce;
        movement.horizontalForce = horizontalForce;
        movement.forceMode = forceMode;
        movement.AnimationName = aName;
    }

    public override void TriggerAbility()
    {
        movement.Trigger();
    }
}
