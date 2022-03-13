using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Cameras : MonoBehaviour
{
    public static _Cameras cameras;
    public CameraBrain currentCamera;
    public FindTarget findTarget;

    public void Awake()
    {
        if (cameras == null)
        {
            cameras = this;
            DontDestroyOnLoad(gameObject);
        }else if (cameras != this)
        {
            Destroy(gameObject);
        }
    }

    public void FixedUpdate()
    {
        if (currentCamera.Target == null)
        {
            SetTarget();
        }
    }

    private void SetTarget()
    {
        currentCamera.Target = findTarget.target;
    }
}
