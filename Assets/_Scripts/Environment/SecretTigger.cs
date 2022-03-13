using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretTigger : MonoBehaviour
{
    private SecretObscurer obscurer;

    private void Start()
    {
        obscurer = GetComponentInParent<SecretObscurer>();
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (IsCollidingWithPlayer(_colInfo))
        {
            obscurer.FadeOutObscurers();
        }
    }

    private void OnTriggerExit2D(Collider2D _colInfo)
    {
        if (IsCollidingWithPlayer(_colInfo))
        {
            obscurer.FadeInObscurers();
        }
    }

    private bool IsCollidingWithPlayer(Collider2D _colInfo)
    {
        return _colInfo.GetComponent<PlayerContainer>() != null;
    }
}
