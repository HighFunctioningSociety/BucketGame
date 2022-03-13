using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScan : MonoBehaviour
{
    public float scanRangeX, scanRangeY;
    public float scanRangeX_Special, scanRangeY_Special;
    public LayerMask enemyLayer;

    public bool EnemyCheckStingerSpecial()
    {
        Vector2 scanRange = new Vector2(scanRangeX_Special, scanRangeY_Special);
        Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(transform.position, scanRange, 0, enemyLayer);

        if (hitEnemy.Length != 0)
        {
            return true;
        }
        else 
            return false;
    }

    public bool EnemyCheckStinger()
    {
        Vector2 scanRange = new Vector2(scanRangeX, scanRangeY);
        Collider2D[] hitEnemy = Physics2D.OverlapBoxAll(transform.position, scanRange, 0, enemyLayer);

        if (hitEnemy.Length != 0)
        {
            return true;
        }
        else
            return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(scanRangeX, scanRangeY, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(scanRangeX_Special, scanRangeY_Special, 0));
    }
}
