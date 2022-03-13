using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDanceEquipment : EquipmentTriggerable
{
    public GameObject swordPrefab;
    public GameObject swordProjectilePrefab;
    public int amountToSpawn;
    public int amountActive;
    public float firingSpeed;
    public float timeToDestroy;
    public float timer = 0;
    private float timeOffset;
    private PlayerContainer _player;
    public GameObject[] activeSwords;

    public void Start()
    {
        Initialize(equipment, this.gameObject);
        timeOffset = (2 * Mathf.PI) / amountToSpawn;
        _player = GetComponentInParent<PlayerContainer>();
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToDestroy)
            DestroySwords();
    }

    public override void Initialize(Equipment selectedAbility, GameObject scriptObject)
    {
        selectedAbility.Initialize(scriptObject);
    }

    public override void TriggerEquipmentAbility()
    {
        SpawnSwords();
    }

    public override void TriggerSecondaryFunction()
    {
        if (amountActive > 0)
            ShootSwords();
    }

    private void SpawnSwords()
    {
        Debug.Log("ImTriggered");
        timer = 0;
        activeSwords = new GameObject[amountToSpawn];
        amountActive = amountToSpawn;
        Orbit swordClone;
        for (int i = 0; i < amountToSpawn; i++)
        {
            swordClone = Instantiate(swordPrefab, transform.position, transform.rotation).GetComponent<Orbit>();
            swordClone.objectToOrbit = this.transform;
            swordClone.timerOffset = i * timeOffset;
            activeSwords[i] = swordClone.gameObject;
        }
    }

    private void ShootSwords()
    {
        Debug.Log("Shoot!");
        Destroy(activeSwords[amountActive - 1]);
        amountActive--;
        InitializeProjectile();
    }

    public void InitializeProjectile()
    {
        float projectileDir = Mathf.Sign(_player.transform.localScale.x);
        GameObject projectileClone = (GameObject)Instantiate(swordProjectilePrefab, transform.position, transform.rotation);
        projectileClone.transform.localScale = new Vector3(swordProjectilePrefab.transform.localScale.x * projectileDir, swordProjectilePrefab.transform.localScale.y, swordProjectilePrefab.transform.localScale.z);
        Rigidbody2D rb = projectileClone.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(projectileDir * firingSpeed, 0);
    }

    private void DestroySwords()
    {
        foreach(GameObject sword in activeSwords)
            Destroy(sword.gameObject);
        activeSwords = new GameObject[0];
        amountActive = 0;
    }
}
