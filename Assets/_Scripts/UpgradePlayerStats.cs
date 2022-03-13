using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlayerStats : MonoBehaviour
{
    public ScriptableBool alreadyCollected;
    void Start()
    {
        gameObject.SetActive(!alreadyCollected.state);
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null)
        {
            PlayerContainer _player = _colInfo.GetComponent<PlayerContainer>();
            _player.playerStats.healthUpgrades++;
            _player.playerStats.curHealth++;
            alreadyCollected.state = true;
            gameObject.SetActive(false);
        }
    }
}
