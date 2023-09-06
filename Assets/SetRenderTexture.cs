using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRenderTexture : MonoBehaviour
{
    public RenderTexture renderTexture;

    void OnPreRender()
    {
        GetComponent<Camera>().targetTexture = renderTexture;
    }

    void OnPostRender()
    {
        GetComponent<Camera>().targetTexture = null;
    }
}


