using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtractFromWallet : TriggerableEvent
{
    public int amountToRemove;

    public override void TriggerEvent()
    {
        SubtractAmount();
    }

    public void SubtractAmount()
    {
        FindObjectOfType<Inventory>().RemoveCurrency(amountToRemove);
    }
}
