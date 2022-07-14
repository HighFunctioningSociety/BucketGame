using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeAttackTriggerable : PlayerAttackTriggerable
{
    [HideInInspector] public float rumbleDurration;
    [HideInInspector] public float rumbleLow;
    [HideInInspector] public float rumbleHigh;

    public AudioSource impactAudioScource;
    public Collider2D attackCollider;
    public GameObject hitEffect;
    public ContactFilter2D enemyLayer;
    public ContactFilter2D obstacleLayer;
    public ContactFilter2D blockerLayer;
    public List<Collider2D> hitEnemies;
    public List<Collider2D> hitObstacles;
    public List<Collider2D> hitBlockers;
    public int enNum = 0;
    public int obNum = 0;
    public int blockNum = 0;

    public abstract void DrawHurtBox();

    public void EnableCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableCollider()
    {
        attackCollider.enabled = false;
    }

    protected void DrawHitEffect(Collider2D collider)
    {
        Vector2 hitPoint = collider.ClosestPoint(new Vector2(attackCollider.bounds.max.x, attackCollider.bounds.center.y));
        Quaternion hitRotation = new Quaternion(hitEffect.transform.rotation.x, hitEffect.transform.rotation.y, hitEffect.transform.rotation.z * Random.Range(-45, 45), hitEffect.transform.rotation.w);
        GameObject hitSpawn = Instantiate(hitEffect, hitPoint, hitRotation);
        hitSpawn.transform.localScale = new Vector3(hitSpawn.transform.localScale.x * player.controller.DirectionFaced, hitSpawn.transform.localScale.y, hitSpawn.transform.localScale.z);
    }
}

