using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFOV : MonoBehaviour
{
    public Camera mainCamera;
    public Collider2D activeRegion;
    private float distance;
    public float frustumHeight;
    public float frustumWidth;
    

    // Start is called before the first frame update
    void Start()
    {
        distance = Mathf.Abs(mainCamera.transform.position.z);
        frustumHeight = 2.0f * distance * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * mainCamera.aspect;
    } 
}
