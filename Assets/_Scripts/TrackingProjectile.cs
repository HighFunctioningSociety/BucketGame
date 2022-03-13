using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    public Transform target;
    public float projectileDir;
    public Vector2 velocity;

    [Space]
    [Header ("Projectile Attributes")]
    public float speed;
    public float turnRate;
    public float lifeTime;
    public float dampen;

    [Space]
    [Header("Components")]
    public LayerMask contact;
    public GameObject projectileParent;
    public GameObject collisionEffectPrefab;


    private float timeOfInstantiation;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeOfInstantiation = Time.time;
    }

    void Update()
    {
        if (target != null)
        {
            TrackTarget();
        }
    }

    private void TrackTarget()
    {
        Vector2 newTarget = new Vector2(target.position.x, target.position.y + 5);
        Vector2 rotateDir = newTarget - rb.position;
        rotateDir.Normalize();
        float rotateAmount = Vector3.Cross(rotateDir, transform.right * projectileDir).z;
        rb.angularVelocity = -rotateAmount * turnRate;

        if (!PauseMenu.GamePaused)
        {
            rb.velocity = transform.right * speed * projectileDir;
            rb.velocity = rb.velocity * (1 - (Time.time - timeOfInstantiation) * dampen);
            if (Time.time - timeOfInstantiation >= lifeTime)
            {
                InstantiatePrefabAndDestroyProjectile();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (((1 << _colInfo.gameObject.layer) & contact) != 0)
        {
            InstantiatePrefabAndDestroyProjectile();
        }
    }

    private void InstantiatePrefabAndDestroyProjectile()
    {
        if (collisionEffectPrefab != null)
            GameObject.Instantiate(collisionEffectPrefab, this.transform.position, this.transform.rotation);
        Destroy(projectileParent.gameObject);
    }
}
