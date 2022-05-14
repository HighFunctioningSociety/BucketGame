using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateEnemy : MonoBehaviour   
{
    private Animator Animator;
    private EnemyContainer Enemy;
    private bool _facingRight = true;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        Enemy = GetComponent<EnemyContainer>();
    }

    void Update()
    {
        if (Enemy.Direction > 0 && !_facingRight)
        {
            Flip();
        }
        else if (Enemy.Direction < 0 && _facingRight)
        {
            Flip();
        }

        Animator.SetFloat("Speed", Enemy.Speed);
    }

    public void AddForceY(float force)
    {
        Enemy.RigidBody.AddForce(Vector2.up * force * Enemy.RigidBody.mass * Enemy.RigidBody.gravityScale, ForceMode2D.Impulse);
    }

    public void FreezeConstraints()
    {
        Enemy.RigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Flip()
    {
        // Switch the way the enemy is labelled as facing.
        _facingRight = !_facingRight;

        // Multiply the enemy's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
