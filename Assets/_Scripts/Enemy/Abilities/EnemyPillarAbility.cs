using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAbility/EnemyPillar")]
public class EnemyPillarAbility : EnemyAbility
{
    private EnemyPillarTriggerable attack;

    public int damage;
    public GameObject pillarPrefab;

    public override void Initialize(GameObject obj)
    {
        attack = obj.GetComponent<EnemyPillarTriggerable>();
        attack.pillarPrefab = pillarPrefab;
        attack.damage = damage;
    }
    public override void TriggerAbility()
    {
        attack.Trigger();
    }
}
