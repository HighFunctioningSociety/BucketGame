using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretObscurer : MonoBehaviour
{
    public SpriteRenderer[] sprites;
    public float fadeAmount = 0.025f;

    public void FadeOutObscurers(){
        foreach (SpriteRenderer sprite in sprites)
        {
            StartCoroutine(FadeOut(sprite));
        }
    }
    public void FadeInObscurers()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            StartCoroutine(FadeIn(sprite));
        }
    }

    IEnumerator FadeOut(SpriteRenderer sprite)
    {
        float alpha = 1;
        while (sprite.color.a >= 0)
        {
            sprite.color = new Color(1, 1, 1, alpha);
            alpha -= fadeAmount;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator FadeIn(SpriteRenderer sprite)
    {
        float alpha = 0;
        while (sprite.color.a < 1)
        {
            sprite.color = new Color(1, 1, 1, alpha);
            alpha += fadeAmount;
            yield return new WaitForEndOfFrame();
        }
    }
}
