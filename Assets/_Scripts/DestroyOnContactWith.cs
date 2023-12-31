﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnContactWith : MonoBehaviour
{
    public LayerMask contact;
    public int Durability = 0;
    private int DurabilityLeft = 0;
    public GameObject objectToDestroy;
    public GameObject prefabEffect;

    public void Awake()
    {
        DurabilityLeft = Durability;
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        GameObject effect;
        if (DurabilityLeft <= 0)
        {
            if (((1 << _colInfo.gameObject.layer) & contact) != 0)
            {
                if (prefabEffect != null)
                {
                    effect = GameObject.Instantiate(prefabEffect, this.transform.position, this.transform.rotation);
                    effect.transform.localScale = new Vector3(prefabEffect.transform.localScale.x * Mathf.Sign(this.transform.localScale.x), 
                                                              prefabEffect.transform.localScale.y, 
                                                              prefabEffect.transform.localScale.z);
                }
                StartCoroutine(DestroyAtEndOfFrame());
            }
        }
       else
            DurabilityLeft--;
    }

    private IEnumerator DestroyAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        Destroy(objectToDestroy.gameObject);
    }
}
