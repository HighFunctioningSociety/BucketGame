using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotator : MonoBehaviour
{
    public float speed;
    public float angleOffset;
    private float angle;
    public bool rotateClockwise;


    public float timer = 0;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime * speed;
        Rotate();
    }

    private void Rotate()
    {
        float sign = (rotateClockwise ? -1f : 1f);
        angle = sign * timer;
        transform.localEulerAngles = this.transform.forward * (angle + sign * angleOffset);
    }
}
