using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLightning : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] LightningPattern1;
    public Transform[] LightningPattern2;
    public Transform[] LightningPattern3;
    public Transform[] LightningPattern4;
    public Transform[] LightningPattern5;
    public Transform[] LightningPattern6;
    public int numberOfPatterns;

    private int previousIndex;
    private List<Transform[]> LightningPatterns;

    private void Start()
    {
        LightningPatterns = new List<Transform[]>();
        LightningPatterns.Add(LightningPattern1);
        LightningPatterns.Add(LightningPattern2);
        LightningPatterns.Add(LightningPattern3);
        LightningPatterns.Add(LightningPattern4);
        LightningPatterns.Add(LightningPattern5);
        LightningPatterns.Add(LightningPattern6);
    }

    public Transform lightning;

    public void InstantiateLightning(Transform spawn) 
    {
        int randInt = Random.Range(0, 2);
        Transform _lightning = Instantiate(lightning, spawn.position, spawn.rotation);

        if (randInt == 0)
        {
            _lightning.localScale = new Vector3(_lightning.localScale.x, _lightning.localScale.y, _lightning.localScale.z);
        }
        else
        {
            _lightning.localScale = new Vector3(_lightning.localScale.x * -1, _lightning.localScale.y, _lightning.localScale.z);
        }
    }

    public void GenerateRandomLightning()
    {
        int index = Random.Range(0, numberOfPatterns);
        Debug.Log(index);
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

        foreach(Transform spawn in LightningPatterns[index])
        {
            InstantiateLightning(spawn);
        }
    }
}
