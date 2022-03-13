using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    public GameObject prefabToSpawn;

    private void OnDestroy()
    {
        GameObject.Instantiate(prefabToSpawn);
    }
}
