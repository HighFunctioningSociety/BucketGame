using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RumbleOnGrounded : MonoBehaviour
{
    public float rumbleDurration;
    public float rumbleLow;
    public float rumbleHigh;
    public void RumbleEvent()
    {
        Rumbler.RumbleConstant(rumbleLow, rumbleHigh, rumbleDurration);
    }
}
