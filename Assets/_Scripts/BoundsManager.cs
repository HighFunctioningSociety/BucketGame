using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsManager : MonoBehaviour
{
    [HideInInspector] public GameObject cameraObject;
    [HideInInspector] public CameraBrain mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera = cameraObject.GetComponent<CameraBrain>();
    }
}
