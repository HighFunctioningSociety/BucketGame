using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentTriggerable : MonoBehaviour
{
    public Equipment equipment;
    [HideInInspector] public string equipmentName;
    [HideInInspector] public Sprite imageIcon;
    [HideInInspector] public string triggerName;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    public abstract void Initialize(Equipment selectedAbility, GameObject scriptObject);

    public abstract void TriggerEquipmentAbility();

    public abstract void TriggerSecondaryFunction();
}
