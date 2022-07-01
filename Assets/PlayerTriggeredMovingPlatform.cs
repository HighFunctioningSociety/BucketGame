using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggeredMovingPlatform : MonoBehaviour
{
    public Transform position1;
    public Transform position2;
    private Transform targetPosition;
    public GameObject platformObj;
    public float speed;

    private void Start()
    {
        platformObj.transform.position = position1.position;
        targetPosition = position1;
    }

    private void FixedUpdate()
    {
        MoveToAlternateSide();
    }

    private void MoveToAlternateSide()
    {
        if (!DestinationReached())
        {
            Vector3 nextPosVector = Vector3.MoveTowards(platformObj.transform.position, targetPosition.transform.position, speed * Time.deltaTime);
            platformObj.transform.position = nextPosVector;
        }
    }

    public void OnTriggerEnterEvent(GameObject _playerObj)
    {
        if (DestinationReached())
        {
            if (targetPosition == position1)
                targetPosition = position2;
            else
                targetPosition = position1;
        }
    }

    private bool DestinationReached()
    {
        bool xIsClose = Mathf.Abs(platformObj.transform.position.x - targetPosition.transform.position.x) < 0.1;
        bool yIsClose = Mathf.Abs(platformObj.transform.position.y - targetPosition.transform.position.y) < 0.1;
        return (xIsClose && yIsClose);
    }
}
