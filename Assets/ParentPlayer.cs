using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayer : MonoBehaviour
{
    public GameObject objToParent;

    private void Start()
    {
        if (objToParent == null)
            Debug.Log("ParentPlayer: ObjToParent not set!");
    }

    public void ParentPlayerObject(GameObject playerObj)
    {
        playerObj.transform.parent = objToParent.transform;
    }

    public void UnparentPlayerObject(GameObject playerObj)
    {
        playerObj.transform.parent = null;
        DontDestroyOnLoad(playerObj);
    }
}
