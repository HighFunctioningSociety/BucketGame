using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/HelmSplitterSubHitAbility")]
public class HelmSplitterSubHitAbility : MeleeAbilities
{
    private HelmSplitterTriggerable attack;

    public float hitStop;

    public override void Initialize(GameObject obj)
    {
        attack = obj.GetComponent<HelmSplitterTriggerable>();

        attack.subHitDamage = damage;
        attack.subHitHitStop = hitStop;
    }

    public override void TriggerAbility()
    {
        attack.Trigger();
    }
}
