using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform objectToOrbit;
    public float xSpread;
    public float ySpread;
    public float zOffset;
    public float speed;
    public float angle;
    public float timerOffset;
    public bool rotateClockwise;
    public bool changeAngleObjectAngle;

    public float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * speed;
        if (objectToOrbit != null)
            Rotate();
    }

    private void Rotate()
    { 
        float x = (rotateClockwise ? -1f : 1f) * Mathf.Cos(timerOffset + timer) * xSpread;
        float y = Mathf.Sin(timerOffset + timer) * ySpread;
        Vector3 pos = new Vector3(x, y, zOffset);
        transform.position = pos + objectToOrbit.position;

        if (changeAngleObjectAngle)
        {
            Vector3 direction = Vector3.Normalize(this.transform.position - objectToOrbit.position);
            angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90;   
            transform.localEulerAngles = this.transform.forward * angle;
        }
    }
}
