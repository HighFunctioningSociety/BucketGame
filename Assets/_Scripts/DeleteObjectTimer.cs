using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectTimer : MonoBehaviour
{
    public float lifeSpan;

    private void FixedUpdate()
    {
        lifeSpan -= Time.fixedDeltaTime;

        if (lifeSpan <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
