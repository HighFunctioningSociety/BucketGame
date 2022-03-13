using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityLight : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float transitionSpeed = 0.01f;
    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (IsCollidingWithPlayer(_colInfo))
        {
            //spriteRenderer.color = new Color(1, 1, 1);
            StartCoroutine(BrightenColor());
        }
    }

    private void OnTriggerExit2D(Collider2D _colInfo)
    {
        if (IsCollidingWithPlayer(_colInfo))
        {
            //spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
            StartCoroutine(DarkenColor());
        }
    }


    private bool IsCollidingWithPlayer(Collider2D _colInfo)
    {
        return _colInfo.GetComponent<PlayerContainer>() != null;
    }

    IEnumerator BrightenColor()
    {
        float brightness = 0.5f;
        while (spriteRenderer.color.r < 1)
        {
            spriteRenderer.color = new Color(brightness, brightness, brightness);
            brightness += transitionSpeed;
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.color = new Color(1, 1, 1);
    }

    IEnumerator DarkenColor()
    {
        float brightness = 1f;
        while (spriteRenderer.color.r > 0.5f)
        {
            spriteRenderer.color = new Color(brightness, brightness, brightness);
            brightness -= transitionSpeed;
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
    }
}
