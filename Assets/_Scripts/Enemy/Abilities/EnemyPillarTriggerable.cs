using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyPillarTriggerable : EnemyTriggerable
{
    [HideInInspector] public int damage;
    [HideInInspector] public Transform[] spawnList;
    [HideInInspector] public EnemyContainer enemy;
    [HideInInspector] public GameObject pillarPrefab;

    public Transform[] pattern1;
    public Transform[] pattern2;
    public Transform[] pattern3;
    public Transform[] pattern4;
    public Transform[] pattern5;
    public Transform[] pattern6;
    public int numberOfPatterns;
    private int previousIndex;
    private List<Transform[]> patterns;
    void Start()
    {
        patterns = new List<Transform[]>();
        patterns.Add(pattern1);
        patterns.Add(pattern2);
        patterns.Add(pattern3);
        patterns.Add(pattern4);
        patterns.Add(pattern5);
        patterns.Add(pattern6);

        enemyAbilityClone = Object.Instantiate(scriptableAbility);
        Initialize(enemyAbilityClone, this.gameObject);
    }

    public void Update()
    {
        CheckCooldown();
    }

    public void Activate(Transform spawn)
    {
        int randInt = Random.Range(0, 2);
        Transform _pillar = Instantiate(pillarPrefab.transform, spawn.position, spawn.rotation);

        if (randInt == 0)
        {
            _pillar.localScale = new Vector3(_pillar.localScale.x, _pillar.localScale.y, _pillar.localScale.z);
        }
        else
        {
            _pillar.localScale = new Vector3(_pillar.localScale.x* -1, _pillar.localScale.y, _pillar.localScale.z);
        }
    }

    public override void Initialize(EnemyAbility selectedAbility, GameObject abilityObject)
    {
        selectedAbility.Initialize(abilityObject);
    }

    private void GetPattern()
    {
        int index = Random.Range(0, numberOfPatterns);
        if (index == previousIndex)
        {
            if (index == numberOfPatterns - 1)
            {
                index--;
            }
            else
            {
                index++;
            }
        }
        previousIndex = index;

        foreach (Transform spawn in patterns[index])
        {
            Activate(spawn);
        }
    }

    public override void Trigger()
    {
        if (abilityCooldown) 
        {    
            GetPattern();
        }
    }
}
