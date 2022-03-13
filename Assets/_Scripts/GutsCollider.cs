using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GutsCollider : MonoBehaviour
{
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null)
        {
            audioSource.Play();
        }
    }
}
