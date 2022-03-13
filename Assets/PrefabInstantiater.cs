using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInstantiater : MonoBehaviour
{
    public GameObject prefabToInstantiate;

    public void InstantiatePrefab()
    {
        Instantiate(prefabToInstantiate, this.transform.position, this.transform.rotation);
    }
}
