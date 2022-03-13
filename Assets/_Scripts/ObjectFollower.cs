using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    public GameObject objectToFollow;

    private void Update()
    {
        if(objectToFollow != null)
            this.transform.position = objectToFollow.transform.position;
    }
}
