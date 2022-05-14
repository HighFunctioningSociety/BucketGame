using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAbility/EnemySwoop")]
public class EnemySwoopAbility : EnemyProximityAbility
{
    [HideInInspector] public EnemySwoopTriggerable ability;
    public override void Initialize(GameObject obj)
    {
        ability = obj.GetComponent<EnemySwoopTriggerable>();

        ability.rangeX = rangeX;
        ability.rangeY = rangeY;
        ability.AnimationName = name;
    }

    public override void TriggerAbility()
    {
        ability.Trigger();
    }
}
