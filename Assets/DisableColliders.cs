using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColliders : MonoBehaviour
{
    public Collider2D[] collidersToDisable;

    public void DisableAllColliders()
    {
        foreach (Collider2D collider in collidersToDisable)
        {
            collider.enabled = false;
        }
    }
}
