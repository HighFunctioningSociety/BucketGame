using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BreakableObject : MonoBehaviour
{
    protected Collider2D objectCollider;

    public abstract void DamageObject(Transform _playerTransform);
}
