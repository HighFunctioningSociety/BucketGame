using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerRaycast : MonoBehaviour
{
    public Collider2D enemyCollider;
    public EnemyContainer enemy;
    public float extraLength;
    public LayerMask obstacleMask;

    private void FixedUpdate()
    {
        DetectObstacle();
    }

    private void DetectObstacle()
    {
        RaycastHit2D rayUp = Physics2D.Raycast(enemyCollider.bounds.center, Vector2.down, ((enemyCollider.bounds.size.y / 2) + extraLength), obstacleMask);
        RaycastHit2D rayDown = Physics2D.Raycast(enemyCollider.bounds.center, Vector2.up, ((enemyCollider.bounds.size.y / 2) + extraLength), obstacleMask);
        RaycastHit2D rayLeft = Physics2D.Raycast(enemyCollider.bounds.center, Vector2.left, ((enemyCollider.bounds.size.x / 2) + extraLength), obstacleMask);
        RaycastHit2D rayRight = Physics2D.Raycast(enemyCollider.bounds.center, Vector2.right, ((enemyCollider.bounds.size.x / 2) + extraLength), obstacleMask);

        Color rayColor = Color.green;

        if (rayUp.collider != null || rayDown.collider != null)
        {
            //enemy.rb.velocity = new Vector2(enemy.rb.velocity.x, enemy.rb.velocity.y * -1);
            rayColor = Color.red;
        }

        if (rayLeft.collider != null || rayRight.collider != null)
        {
            //enemy.rb.velocity = new Vector2(enemy.rb.velocity.x * -1, enemy.rb.velocity.y);
            rayColor = Color.red;
        }

        Debug.DrawRay(enemyCollider.bounds.center, Vector2.down * ((enemyCollider.bounds.size.y / 2) + extraLength), rayColor);
        Debug.DrawRay(enemyCollider.bounds.center, Vector2.up * ((enemyCollider.bounds.size.y / 2) + extraLength), rayColor);
        Debug.DrawRay(enemyCollider.bounds.center, Vector2.left * ((enemyCollider.bounds.size.x / 2) + extraLength), rayColor);
        Debug.DrawRay(enemyCollider.bounds.center, Vector2.right * ((enemyCollider.bounds.size.x / 2) + extraLength), rayColor);
    }
}
