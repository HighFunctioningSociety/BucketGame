using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentAngleCounterBalance : MonoBehaviour
{
    public Transform ParentObject;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = this.transform.forward * -ParentObject.transform.rotation.z;
    }
}
