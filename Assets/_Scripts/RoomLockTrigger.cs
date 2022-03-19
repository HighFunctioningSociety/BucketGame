using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLockTrigger : MonoBehaviour
{
    public BoxCollider2D boxColliderL;
    public BoxCollider2D boxColliderR;

    public void Start()
    {
        boxColliderL.enabled = false;
        boxColliderR.enabled = false;
    }

    public void ActivateColliders()
    {
        boxColliderL.enabled = true;
        boxColliderR.enabled = true;
    }
}
