using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer: MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float fadingSpeed = 0.0005f;
    

    [Range(0f,1f)]
    private float fadeAmount = 0.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator Fade()
    {
        for (float j = 3; j > 0; j--) { 
            // Fade to desired color
            for (float i = 1f; i >= fadeAmount; i -= 0.05f)
            {
                // Getting access to Color options
                Color spriteColor = spriteRenderer.color;

                // Setting values for Green and Blue channels
                spriteColor.g = i;
                spriteColor.b = i;
                spriteColor.r = i;

                // Set color to Sprite Renderer
                spriteRenderer.color = spriteColor;

                // Pause to make color be changed slowly
                yield return new WaitForSeconds(fadingSpeed);
            }

            // Return to original color
            for (float i = 0f; i <= 1; i += 0.05f)
            {
                // Getting access to Color options
                Color spriteColor = spriteRenderer.color;

                // Setting values for Green and Blue channels
                spriteColor.g = i;
                spriteColor.b = i;
                spriteColor.r = i;

                // Set color to Sprite Renderer
                spriteRenderer.color = spriteColor;

                // Pause to make color be changed slowly
                yield return new WaitForSeconds(fadingSpeed);
            }
        }
    }

    public void _flickerEffect()
    {
        StartCoroutine(Fade());
        StopCoroutine(Fade());
    }
}
