using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateEnemy : MonoBehaviour   
{
    private Animator animator;
    private EnemyContainer enemy;
    private bool facingRight = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyContainer>();
    }

    void Update()
    {
        if (enemy.dir > 0 && !facingRight)
        {
            Flip();
        }
        else if (enemy.dir < 0 && facingRight)
        {
            Flip();
        }

        animator.SetFloat("Speed", enemy.speed);
    }

    public void AddForceY(float force)
    {
        enemy.rb.AddForce(Vector2.up * force * enemy.rb.mass * enemy.rb.gravityScale, ForceMode2D.Impulse);
    }

    public void FreezeConstraints()
    {
        enemy.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
