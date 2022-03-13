using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Equipment")]
public class Equipment : ScriptableObject
{
    private EquipmentTriggerable equipment;

    public string equipmentName;
    public float cooldown;
    public float secondaryCooldown;
    public Sprite imageIcon;
    public string triggerName;

    public void Initialize(GameObject obj)
    {
        equipment = obj.GetComponent<EquipmentTriggerable>();
        equipment.equipmentName = equipmentName;
        equipment.imageIcon = imageIcon;
        equipment.triggerName = triggerName;
    }

    public void TriggerEquipment()
    {
        equipment.TriggerEquipmentAbility();
    }

    public void TriggerSecondaryEquipmentFunction()
    {
        equipment.TriggerSecondaryFunction();
    }
}
