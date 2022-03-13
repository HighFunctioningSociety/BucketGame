using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTriggers : MonoBehaviour
{
    public bool trigger1;
    public bool trigger2;
    public bool trigger3;
    public bool trigger4;
    public bool retreated;

    private void Start()
    {
        trigger1 = false;
        trigger2 = false;
        trigger3 = false;
        trigger4 = false;
        retreated = false;
    }

    public void FlipTrigger(int index)
    {
        switch (index)
        {
            case 1:
                trigger1 = !trigger1;
                break;
            case 2:
                trigger2 = !trigger2;
                break;
            case 3:
                trigger3 = !trigger3;
                break;
            case 4:
                trigger4 = !trigger4;
                break;
        }
    }

    public bool GetTriggerValue(int index)
    {
        switch (index)
        {
            case 1:
                return trigger1;
            case 2:
                return trigger2;
            case 3:
                return trigger3;
            case 4:
                return trigger4;
            default:
                return false;
        }
    }

    public void EndRetreat()
    {
        retreated = false;
    }

    public void ResetTriggers()
    {
        trigger1 = false;
        trigger2 = false;
        trigger3 = false;
        trigger4 = false;
        retreated = false;
    }
}
