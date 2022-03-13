using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{
    //float nextTimeToSearch = 0;
    public Transform target;

    void Update()
    {
        if (target == null)
        {
            FindPlayer();
            return;
        }   
    }

    void FindPlayer()
    {
        GameObject searchResult = _GameManager.GivePlayer();
        if (searchResult != null)
        {
            target = searchResult.transform;
        }
    }
}
