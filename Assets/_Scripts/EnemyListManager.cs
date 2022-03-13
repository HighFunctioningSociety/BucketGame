using UnityEngine;

public class EnemyListManager : MonoBehaviour
{
    public GameObject enemyInfoPrefab;

    public void InstantiateEnemyUIData(EnemyContainer _enemy)
    {
        GameObject UIPrefab = Instantiate(enemyInfoPrefab);
        EnemyUIData enemyData = UIPrefab.GetComponent<EnemyUIData>();
        enemyData.enemyObject = _enemy;
        UIPrefab.transform.SetParent(this.transform);
    }

    public void DeleteEnemyList()
    {
        EnemyUIData[] EnemyList = this.GetComponentsInChildren<EnemyUIData>();
        foreach (EnemyUIData enemyData in EnemyList)
        {
            enemyData.DestroyEnemyData();
        }
    }
}
