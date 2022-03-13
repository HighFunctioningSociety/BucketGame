using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardZone : MonoBehaviour
{
    private HazardController hazardController;

    private void Awake()
    {
        hazardController = GameObject.FindGameObjectWithTag("HazardController").GetComponent<HazardController>();
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {

        if (_colInfo.GetComponent<PlayerContainer>() != null)
        {
            PlayerContainer player = _colInfo.GetComponent<PlayerContainer>();
            player._DamagePlayer(1, true);
            player.abilityManager.CancelAbilities();
            player.rb.velocity = Vector2.zero;
            hazardController._RespawnAfterHitStop(player);

            return;
        }

        else if (_colInfo.GetComponent<EnemyContainer>() != null)
        {
            EnemyContainer _enemy = _colInfo.GetComponent<EnemyContainer>();
            _GameManager.KillEnemy(_enemy);

            return;
        }
    }
}
