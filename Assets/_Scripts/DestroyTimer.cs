using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float TimeToDestroy;
    public float Timer = 0;

    public Transform PrefabEffect;

    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= TimeToDestroy)
            Destroy(this.gameObject);
    }
}
