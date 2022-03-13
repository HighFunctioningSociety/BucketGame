using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterWall : MonoBehaviour
{
    public Transform target;
    public SpriteRenderer sr;
    private EnemyContainer enemy;
    private Collider2D colliderBox;
    private float timeToCheck;

    private void Start()
    {
        colliderBox = GetComponent<Collider2D>();
        colliderBox.enabled = false;
        enemy = target.GetComponent<EnemyContainer>();
        timeToCheck = Time.time;
    }

    void FixedUpdate()
    {
        bool checkForTarget = Time.time > timeToCheck;
        if (enemy.aiActive && target.gameObject.activeSelf)
        {
            colliderBox.enabled = true;
            sr.enabled = true;
        }
        else 
        {
            colliderBox.enabled = false;
            sr.enabled = false;
        }

        if (checkForTarget)
        {
            if (!target.gameObject.activeSelf)
            {
                colliderBox.enabled = false;
                sr.enabled = false;
            }
            timeToCheck = Time.time + 2;
        }
    }
}
