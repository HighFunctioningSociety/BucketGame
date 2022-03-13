using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableEnvironment : BreakableObject
{
    private Sprite initialSprite;
    public Sprite brokenSprite;
    private SpriteRenderer sr;
    public AudioSource audioSource;
    public ParticleSystem particles;
    
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        objectCollider = GetComponent<BoxCollider2D>();
        initialSprite = sr.sprite;
    }

    public override void DamageObject(Transform _playerTransform)
    {
        if (brokenSprite != null)
            sr.sprite = brokenSprite;
        if (audioSource.clip != null)
            audioSource.Play();
        if (particles != null)
            CreateDebris(_playerTransform.position.x);
        objectCollider.enabled = false;
    }

    public void _ResetObject()
    {
        sr.sprite = initialSprite;
        objectCollider.enabled = true;
    }

    public void CreateDebris(float _playerXCoordinate)
    {
        float direction = Mathf.Sign(transform.position.x - _playerXCoordinate);
        particles.transform.localScale = new Vector3 (Mathf.Abs(particles.transform.localScale.x) * direction, particles.transform.localScale.y, particles.transform.localScale.z);
        particles.Play();
    }
}
