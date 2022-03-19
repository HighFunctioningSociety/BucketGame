using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEquipmentTriggerable : EquipmentTriggerable
{
    public void Start()
    {
        Initialize(equipment, this.gameObject);
    }

    public override void Initialize(Equipment selectedAbility, GameObject scriptObject)
    {
        selectedAbility.Initialize(scriptObject);
    }

    public override void TriggerEquipmentAbility()
    {

    }

    public override void TriggerSecondaryFunction()
    {

    }
}
