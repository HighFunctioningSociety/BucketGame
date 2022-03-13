using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileAttackTriggerable : PlayerAttackTriggerable
{
    [HideInInspector] public float speed;
    [HideInInspector] public float knockBack;
    [HideInInspector] public GameObject projectilePrefab;
    [HideInInspector] public float moveForceX, moveForceY;
    [HideInInspector] public float shakeLength;
    [HideInInspector] public float shakeAmplitude;
    [HideInInspector] public float rumbleDurration;
    [HideInInspector] public float rumbleLow;
    [HideInInspector] public float rumbleHigh;
    public GameObject firingEffectPrefab;
    public Transform effectSpawn;

    private void Awake()
    {
        player = GetComponentInParent<PlayerContainer>();
        animator = GetComponentInParent<Animator>();
        Initialize(ability, this.gameObject);
    }

    public override void Initialize(Ability selectedAbility, GameObject scriptObject)
    {
        selectedAbility.Initialize(scriptObject);
    }

    public override void Trigger ()
    {
        animator.SetTrigger(triggerName);
    }

    public void SpawnProjectileEvent()
    {
        player.rb.velocity = Vector2.zero;
        player.rb.AddForce(new Vector2(moveForceX* -Mathf.Sign(player.transform.localScale.x), moveForceY), ForceMode2D.Impulse);
        ActivateCameraShake(shakeLength, shakeAmplitude);
        ApplyControllerRumble(rumbleDurration, rumbleLow, rumbleHigh);
        InitializeProjectile();
    }

    public void InitializeProjectile()
    {
        float projectileDir = Mathf.Sign(player.transform.localScale.x);
        GameObject firingEffectClone = (GameObject)Instantiate(firingEffectPrefab, effectSpawn.position, effectSpawn.rotation); ;
        GameObject projectileClone = (GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation);
        firingEffectClone.transform.localScale = new Vector3(firingEffectClone.transform.localScale.x * projectileDir, firingEffectClone.transform.localScale.y, firingEffectClone.transform.localScale.z);
        projectileClone.transform.localScale = new Vector3(projectilePrefab.transform.localScale.x * projectileDir, projectilePrefab.transform.localScale.y, projectilePrefab.transform.localScale.z);
        Rigidbody2D rb = projectileClone.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(projectileDir * speed, 0);
    }
    private void ActivateCameraShake(float _shakeLength, float _shakeAmplitude)
    {
        if (_shakeLength != 0)
            SimpleCameraShake._CameraShake(_shakeLength, _shakeAmplitude);
    }

    private void ApplyControllerRumble(float _rumbleDurration, float _rumbleLow, float _rumbleHigh)
    {
        if (_rumbleDurration > 0)
        {
            Rumbler.RumbleConstant(_rumbleLow, _rumbleHigh, _rumbleDurration);
        }
    }
}
