using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public float speed;
    public float waitTime = 1;
    public float waitTimeLeft = 0;
    private Transform startingPosition;
    public int currentDestinationIndex;
    [HideInInspector] public Transform currentDestination;

    // Start is called before the first frame update
    void Awake()
    {
        startingPosition = points[0];
        currentDestination = points[1];
        currentDestinationIndex = 1;
        transform.position = startingPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        bool doneWaiting = waitTimeLeft <= 0;

        // Destination reached, change destination
        if (doneWaiting && DestinationReached())
        {
            currentDestinationIndex = (currentDestinationIndex + 1) % points.Length;
            currentDestination = points[currentDestinationIndex];
            waitTimeLeft = waitTime;
        }

        //Continue moving towards destination
        else if (doneWaiting && !DestinationReached())
        {
            transform.position = Vector2.MoveTowards(transform.position, currentDestination.position, speed * Time.deltaTime);
        }

        else
        {
            waitTimeLeft -= Time.deltaTime;
        }
    }

    private bool DestinationReached()
    {
        bool xIsClose = Mathf.Abs(transform.position.x - currentDestination.position.x) < 0.1;
        bool yIsClose = Mathf.Abs(transform.position.y - currentDestination.position.y) < 0.1;
        return (xIsClose && yIsClose);
    }

    public bool IsMovingDown()
    {
        return (Mathf.Sign(transform.position.y - currentDestination.position.y) > 0);
    }

    public void ResolveNegativeIndex()
    {
        if (currentDestinationIndex < 0)
        {
            currentDestinationIndex = points.Length - 1;
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawLine(points[i].position, points[i++ % points.Length].position);
        }
    }
}
