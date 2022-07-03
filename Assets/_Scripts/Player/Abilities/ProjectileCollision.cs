using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public Collider2D projectileCollider;
    public int damage;
    public GameObject hitEffect;

    public float knockBackX;
    public float knockBackY;
    public bool shouldHurtEnemy = true;
    public bool createHitEffect = false;

    [Header ("Hitstop and Rumble")]
    public float hitStop;
    public float shakeLength;
    public float shakeAmplitude;
    public float rumbleDurration;
    public float rumbleLow;
    public float rumbleHigh;

    

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        EnemyContainer _enemy = _colInfo.GetComponentInParent<EnemyContainer>();

        if (_enemy != null)
        {
            if (shouldHurtEnemy && _enemy.Hurt != null)
            {
                _enemy.Hurt.EnterHurtState();
            }

            if (createHitEffect)
                DrawHitEffect(_colInfo);
            _enemy.KnockBack(knockBackX, knockBackY, projectileCollider.bounds.center); ;
            _enemy.TakeDamage(damage, false);
            ApplyHitstop(hitStop, shakeLength, shakeAmplitude);
        }
    }

    private void ApplyHitstop(float _hitstop, float _shakeLength, float _shakeAmplitude)
    {
        if (hitStop > 0)
        {
            SimpleCameraShake._CameraShake(_shakeLength, _shakeAmplitude);
            HitStop._SimpleHitStop(_hitstop);
        }
    }

    private void ApplyControllerRumble()
    {
        if (rumbleDurration > 0)
        {
            Rumbler.RumbleConstant(rumbleLow, rumbleHigh, rumbleDurration);
        }
    }

    protected void DrawHitEffect(Collider2D collider)
    {
        Vector2 hitPoint = collider.ClosestPoint(new Vector2(projectileCollider.bounds.max.x, projectileCollider.bounds.center.y));
        Quaternion hitRotation = new Quaternion(hitEffect.transform.rotation.x, hitEffect.transform.rotation.y, hitEffect.transform.rotation.z * Random.Range(-45, 45), hitEffect.transform.rotation.w);
        GameObject hitSpawn = Instantiate(hitEffect, hitPoint, hitRotation);
    }
}
