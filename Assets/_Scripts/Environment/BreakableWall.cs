using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : BreakableObject
{
    public int health;
    public ParticleSystem particles;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        objectCollider = GetComponent<Collider2D>();
    }

    public override void DamageObject(Transform _playerTransform)
    {
        health--;
        particles.Play();
        if (health <= 0)
            DestroyObject();
    }

    private void DestroyObject()
    {
        objectCollider.enabled = false;
        sr.enabled = false;
    }
}
