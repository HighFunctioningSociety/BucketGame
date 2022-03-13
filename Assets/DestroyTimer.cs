using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float timeToDestroy;
    public float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeToDestroy)
            Destroy(this.gameObject);
    }
}
