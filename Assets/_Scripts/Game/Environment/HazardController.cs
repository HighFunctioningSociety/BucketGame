using System.Collections;
using UnityEngine;

public class HazardController : MonoBehaviour
{
    public Collider2D activeHazardCollider;
    public Vector2 activeHazardCoordinates;

    public void SetActiveHazardZone(HazardRespawnTrigger _hazardZone)
    {
        activeHazardCoordinates = CalculateHazardRespawn(_hazardZone);
        activeHazardCollider = _hazardZone.GetComponent<Collider2D>();
    }

    public Vector2 CalculateHazardRespawn(HazardRespawnTrigger _hazardRespawn)
    {
        Collider2D hazardCollider = _hazardRespawn.GetComponent<Collider2D>();
        float hazardSpawnX = hazardCollider.bounds.center.x;
        float hazardSpawnY = hazardCollider.bounds.min.y;
        Vector2 hazardSpawn = new Vector2(hazardSpawnX, hazardSpawnY);

        return hazardSpawn;
    }

    public void _RespawnAfterHitStop(PlayerContainer _player)
    {
        StartCoroutine(RespawnAfterHitStop(_player));
    }

    private IEnumerator RespawnAfterHitStop(PlayerContainer _player)
    {
        while (HitStop.hitStopActive == true)
        {
            yield return null;
        }
        _player.transform.position = new Vector2(activeHazardCoordinates.x, activeHazardCoordinates.y);
    }
}
