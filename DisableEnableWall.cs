using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEnableWall : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public SpriteRenderer sr;

    public void DisableEnableEvent()
    {
        boxCollider.enabled = !boxCollider.enabled;
        sr.enabled = !sr.enabled;
    }
}
