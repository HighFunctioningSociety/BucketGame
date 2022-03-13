using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCheck : MonoBehaviour
{
    public MovingPlatform movingPlatform;
    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (IsCollidingWithEntity(_colInfo))
        {
            ResolveCollision();
        }
    }

    private bool IsCollidingWithEntity(Collider2D _colInfo)
    {
        bool isTouchingPlayer = _colInfo.GetComponent<PlayerContainer>() != null;
        bool isTouchingEnemy = _colInfo.GetComponent<EnemyContainer>() != null;
        return (isTouchingPlayer || isTouchingEnemy);
    }

    private void ResolveCollision()
    {
        if (movingPlatform.IsMovingDown())
        {
            movingPlatform.currentDestinationIndex--;
            movingPlatform.ResolveNegativeIndex();
            movingPlatform.currentDestination = movingPlatform.points[movingPlatform.currentDestinationIndex];
            Debug.Log(movingPlatform.currentDestinationIndex);
        }
    }
}
