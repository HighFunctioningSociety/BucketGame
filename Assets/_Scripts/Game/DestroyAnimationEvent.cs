using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAnimationEvent : MonoBehaviour
{
    public void DestroyEvent()
    {
        Destroy(this.gameObject);
    }
}
