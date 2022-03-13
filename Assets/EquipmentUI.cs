using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public Image equipmentIcon;
    public Image equipmentCoolDown;
    public float imageFillMin;
    public float imageFillMax;
    public float startTime;
    public Color IconCoolDownColor;
    public Color IconReadyColor;
    public float cooldownPercent;

    void Start()
    {
        if (equipmentIcon == null || equipmentCoolDown == null)
        {
            Debug.LogError("Equipment UI not properly assembled");
        }

        equipmentCoolDown.fillAmount = imageFillMin;
    }

    public void SetEquipmentCooldown(float _nextReadyTime)
    {
        equipmentCoolDown.fillAmount = Mathf.Clamp(imageFillMin + ((Time.time - startTime) / (_nextReadyTime - startTime)/2), imageFillMin, imageFillMax);
    }

    public void SetEquipmentIconColor(bool _coolDownComplete)
    {
        if (_coolDownComplete)
        {
            equipmentIcon.color = IconReadyColor;
            equipmentCoolDown.color = IconReadyColor;
        }
        else
        {
            equipmentIcon.color = IconCoolDownColor;
            equipmentCoolDown.color = IconCoolDownColor;
        }
    }

    public void _ChangeEquipmentIcon(Sprite _iconToUse)
    {
        equipmentIcon.sprite = _iconToUse;
    }
}
