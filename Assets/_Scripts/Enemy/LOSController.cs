using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LOSController : MonoBehaviour
{

    public float LOSLength;
    public float LOSHitCount;

    private Bounds _enemyBounds;
    private EnemyContainer _enemyContainer;

    private void Start()
    {
        _enemyContainer = GetComponent<EnemyContainer>();
        _enemyBounds = _enemyContainer.EnemyCollider.bounds;
    }

    private void Update()
    {
        //DrawLineOfSight();
    }

    public RaycastHit2D GenerateLOSRaycast()
    {
        Vector3 xBoundsExtents = (_enemyContainer.Direction == 1 ? -new Vector3(_enemyBounds.extents.x, 0) : new Vector3(_enemyBounds.extents.x, 0));
        Vector2 directionalVector = (_enemyContainer.Direction == 1 ? Vector2.left : Vector2.right);
        return Physics2D.Raycast(_enemyBounds.center + xBoundsExtents, directionalVector, 1f, Constants.Layers.Obstacles);
    }

    public bool CheckLineOfSight()
    {
        RaycastHit2D raycastLOS = GenerateLOSRaycast();
        if (raycastLOS.collider != null)
        {
            LOSHitCount++;
            return true;
        }
        else
        {
            LOSHitCount = 0;
            return false;
        }
    }

    public void DrawLineOfSight()
    {
        if (LOSLength != 0)
        {
            Color raycastColor = Color.cyan;
            Vector3 xBoundsExtents = (_enemyContainer.Direction == 1 ? -new Vector3(_enemyBounds.extents.x, 0) : new Vector3(_enemyBounds.extents.x, 0));
            Vector2 directionalVector = (_enemyContainer.Direction == 1 ? Vector2.left : Vector2.right);
            Debug.DrawRay(_enemyBounds.center + xBoundsExtents, directionalVector * LOSLength, raycastColor);
        }
    }

    public bool CheckRaycastObstacleCollision()
    {
        RaycastHit2D raycastLOS = GenerateLOSRaycast();
        return raycastLOS.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (LOSLength != 0)
        {
            Color raycastColor = Color.cyan;
            Debug.DrawRay(_enemyBounds.center - new Vector3(_enemyBounds.extents.x, 0), Vector2.left * LOSLength, raycastColor);
        }
    }
}
