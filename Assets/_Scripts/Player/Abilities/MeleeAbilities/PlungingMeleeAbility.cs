using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Plunge")]
public class PlungingMeleeAbility : MeleeAbilities
{
    private PlungingMeleeTriggerable attack;

    public float meterProgression;
    public float knockBackY;
    public float moveForceY;
    public float bounce;
    public float hitStop;

    public override void Initialize(GameObject obj)
    {
        attack = obj.GetComponent<PlungingMeleeTriggerable>();

        attack.damage = damage;
        attack.meterProgression = meterProgression;
        attack.knockBackY = knockBackY;
        attack.bounce = bounce;
        attack.moveForceY = moveForceY;
        attack.hitStop = hitStop;
        attack.rumbleDurration = rumbleDurration;
        attack.rumbleLow = rumbleLow;
        attack.rumbleHigh = rumbleHigh;
        attack.triggerName = triggerName;
    }

    public override void TriggerAbility()
    {
        attack.Trigger();
    }
}
