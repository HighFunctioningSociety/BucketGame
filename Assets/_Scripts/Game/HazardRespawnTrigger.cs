using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardRespawnTrigger : MonoBehaviour
{
    private HazardController hazardController;

    private void Awake()
    {
        hazardController = GameObject.FindGameObjectWithTag("HazardController").GetComponent<HazardController>();
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        bool playerContact = _colInfo.GetComponent<PlayerContainer>();
        if (playerContact)
        {
            hazardController.SetActiveHazardZone(this);
        }
    }
}
