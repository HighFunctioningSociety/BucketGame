using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public int lifeTime = 6;
    public float spikeRangeX = 4f;
    public float spikeRangeY = 10f;
    public float spikeActiveX = 3f;
    public float spikeActiveY = 18f;

    public Transform spikeActive;
    public Transform spikePoint;
    public Animator animator;
    public Rigidbody2D rb;
    public ParticleSystem fog;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;

    private int hurtBoxFlag = 0;

    
    void Start()
    {   
        StartCoroutine(LifeSpan());
    }

    void Update()
    {
        checkCollision();
        if (hurtBoxFlag == 1)
        {
            Activate();
        }
    }

    private void checkCollision()
    {
        Vector2 spikeRange = new Vector2(spikeRangeX, spikeRangeY);

        Collider2D hitPlayer = Physics2D.OverlapBox(spikePoint.position, spikeRange, 0, playerLayer);
        Collider2D hitObstacle = Physics2D.OverlapBox(spikePoint.position, spikeRange, 0, obstacleLayer);

        if (hitPlayer != null || hitObstacle != null)
        {
            StopCoroutine(LifeSpan());
            animator.SetTrigger("Activate");
            rb.velocity = new Vector3 (0, 0, 0);
            //fog.emissionRate = 0;
        }

    }

    private void Activate()
    {
        Vector2 spikeRangeActive = new Vector2(spikeActiveX, spikeActiveY);

        Collider2D hitPlayer = Physics2D.OverlapBox(spikeActive.position, spikeRangeActive, 0, playerLayer);

        if (hitPlayer != null)
        {
            PlayerContainer _player = hitPlayer.GetComponent<PlayerContainer>();
            _player._KnockBack(40, 30, spikeActive.position);
            _player._DamagePlayer(1, false);
        }
    }
    
    public void FlagOn()
    {
        if (hurtBoxFlag == 0) hurtBoxFlag = 1;
    }

    public void FlagOff()
    {
        if (hurtBoxFlag == 1) hurtBoxFlag = 0;
    }

    public void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (spikePoint == null)
            return;

        Vector2 spikeRange = new Vector2(spikeRangeX, spikeRangeY);
        Vector2 spikeRangeActive = new Vector2(spikeActiveX, spikeActiveY);

        Gizmos.DrawWireCube(spikePoint.position, spikeRange);
        Gizmos.DrawWireCube(spikeActive.position, spikeRangeActive);
    }

}
