using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIData: MonoBehaviour
{
    public EnemyContainer enemyObject;
    public Text NameText;
    public Text HealthText;
    public Text CoordinateTextX;
    public Text CoordinateTextY;
    public Toggle ActiveAIToggle;

    private void Start()
    {
        transform.localScale = new Vector3 (1, 1, 1);
    }

    private void FixedUpdate()
    {
        if (enemyObject != null)
        {
            SetName();
            SetHealth();
            SetCoordiantes();
            SetActice();
        }
    }

    private void SetName()
    {
        NameText.text = enemyObject.name;
    }

    private void SetCoordiantes()
    {
        CoordinateTextX.text = string.Format("({0}," , Mathf.Round(enemyObject.transform.position.x * 100)*0.01);
        CoordinateTextY.text = string.Format("{0})", Mathf.Round(enemyObject.transform.position.y * 100) * 0.01);
    }

    private void SetHealth()
    {
        HealthText.text =  string.Format("{0} / {1}", enemyObject.curHealth, enemyObject.enemyStats.maxHealth);
    }
    private void SetActice()
    {
        ActiveAIToggle.isOn = enemyObject.gameObject.activeSelf;
    }

    public void DestroyEnemyData()
    {
        Destroy(gameObject);
    }

}
