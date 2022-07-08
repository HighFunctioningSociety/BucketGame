using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public EquipmentTriggerable[] EquipmentList;
    [Space]

    public AbilityController abilityManager;
    public float equipmentNextReadyTime;
    public float secondaryNextReadyTime;
    public bool equipmentCoolDownComplete = false;
    public bool secondaryCoolDownComplete = false;

    private void Start()
    {
        UpdateIconColor();
    }

    private void Update()
    {
        CheckCooldown();
        UpdateEquipmentCoolDownUI();
    }

    public void SetNextReadyTime(float _equipmentNextReadyTime)
    {
        equipmentNextReadyTime = Time.time + _equipmentNextReadyTime;
        SetUIStartTime();
    }

    public void SetSecondaryNextReadyTime(float _secondaryNextReadyTime)
    {
        secondaryNextReadyTime = Time.time + _secondaryNextReadyTime;
    }

    private void SetUIStartTime()
    {
        _UIManager.UIManager._SetEquipmentUIStartTime(Time.time);
    }

    public void CheckCooldown()
    {
        bool tempVal = equipmentCoolDownComplete;
        equipmentCoolDownComplete = (equipmentNextReadyTime <= Time.time);
        secondaryCoolDownComplete = (secondaryNextReadyTime <= Time.time);
        if (tempVal != equipmentCoolDownComplete)
            UpdateIconColor();
    }

    public void UpdateIconColor()
    {
        _UIManager.UIManager._SetEquipmentIconColor(equipmentCoolDownComplete);
    }

    public void UpdateEquipmentCoolDownUI()
    {
        _UIManager.UIManager._SetEquipmentCoolDown(equipmentNextReadyTime);
    }

    public void ChangeEquipmentIcon(Sprite iconToUse)
    {
        _UIManager.UIManager._ChangeEquipmentIcon(iconToUse);
    }

    public void SelectEquipment(string equipmentName)
    {
        foreach (EquipmentTriggerable equipment in EquipmentList)
        {
            if (equipmentName == equipment.equipmentName)
            {
                Debug.Log("eck!");
                abilityManager.currentEquipmentAbility = equipment;
                ChangeEquipmentIcon(equipment.imageIcon);
            }
        }
    }
}
