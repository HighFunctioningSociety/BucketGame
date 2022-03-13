using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObjectController : MonoBehaviour
{
    public int durability;
    public float respawnCooldown;
    public GameObject backLight;
    public GameObject[] firePrefabs;
    private float nextReadyTime;
    private int durabilityLeft;
    private Collider2D boxCollider;
    private SpriteRenderer sr;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        durabilityLeft = durability;
        if (durability != firePrefabs.Length)
            Debug.LogError("durability doesn't match length of firePrefabArray");
    }

    private void Update()
    {
        if (ReadyToRespawn())
            Respawn();
    }

    public void DecrementDurability()
    {
        firePrefabs[durabilityLeft-1].SetActive(false);
        nextReadyTime = Time.time + respawnCooldown;
        durabilityLeft--;

        if (durabilityLeft <= 0)
        {
            sr.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            boxCollider.enabled = false;
            backLight.SetActive(false);
        }
    }

    private void Respawn()
    {
        boxCollider.enabled = true;
        backLight.SetActive(true);
        durabilityLeft = durability;
        sr.color = new Color(1f, 1f, 1f, 1f);
        foreach (GameObject firePrefab in firePrefabs)
        {
            firePrefab.SetActive(true);
        }
    }

    private bool ReadyToRespawn()
    {
        return (nextReadyTime <= Time.time) && (durabilityLeft < durability);
    }
}
