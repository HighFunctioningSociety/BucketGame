using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _SceneManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public Transform[] entrancePoints;
    public EnemyContainer[] enemyList;
    public DestructableEnvironment[] destructableEnvironment;
    public GameObject[] fightTriggers;
    public bool enemyUISet = false;
    private AreaLoader spawnEntrance;
    private PlayerContainer playerContainer;

    private void OnEnable()
    {
        
        if (_GameManager.gm != null)
            _GameManager.gm.SceneLoadEvent.Invoke();
    }

    private void Start()
    {
        _GameManager.GetSceneManager(this);
        BlankerAnimator.blanker.FadeIn();
        if (!_GameManager.respawningPlayer && !_GameManager.firstUpdate && !_GameManager.debugSceneChange)
        {
            FindCorrectEntrance();
            MoveToEntrance();
        }

        if (_GameManager.debugSceneChange)
        {
            playerContainer = _GameManager.GivePlayer().GetComponent<PlayerContainer>();
            playerContainer.transform.position = spawnPoints[0].transform.position;
            _GameManager.debugSceneChange = false;
        }
    }


    private void FindCorrectEntrance()
    {
        foreach(Transform entranceTransform in entrancePoints)
        {
            AreaLoader _entrance = entranceTransform.GetComponent<AreaLoader>();
            int doorId = _entrance.DoorID;
            if (doorId == _GameManager.DoorID)
            {
                spawnEntrance = _entrance;
            }
        }
    }

    public void MoveToEntrance()
    {
        _GameManager.MovePlayerToEntrance();
    }

    public void _MoveToEntrance(Transform _player)
    {
        ResetPlayerTransitions();

        playerContainer = _player.GetComponent<PlayerContainer>();
        Collider2D collider = spawnEntrance.GetComponent<Collider2D>();
        Vector2 spawnPosition = GetSpawnOffset(collider);
        playerContainer.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, _player.position.z);

        _GameManager.AcceptPlayerInput();
    }

    public void ResetPlayerTransitions()
    {
        PlayerContainer._playerContainer.transitioningLeft = false;
        PlayerContainer._playerContainer.transitioningRight = false;
    }

    private Vector2 GetSpawnOffset(Collider2D _collider)
    {
        float _xCoordinate = _collider.bounds.min.y;
        float _yCoordinate;
        if (spawnEntrance.doorDirection == AreaLoader.Direction.LEFT)
        {
            _yCoordinate = _collider.bounds.max.x + 10;
        }
        else if (spawnEntrance.doorDirection == AreaLoader.Direction.RIGHT)
        {
            _yCoordinate = _collider.bounds.min.x - 10;
        }
        else if (spawnEntrance.doorDirection == AreaLoader.Direction.UP)
        {
            _yCoordinate = _collider.bounds.center.x;
            _xCoordinate = _collider.bounds.min.y - 10;
        }
        else if (spawnEntrance.doorDirection == AreaLoader.Direction.DOWN)
        {
            //_colliderXOffset = _collider.bounds.center.x;
            //_colliderYOffset = _collider.bounds.max.y;
            _yCoordinate = spawnEntrance.specificSpawn.position.x;
            _xCoordinate = spawnEntrance.specificSpawn.position.y;
        }
        else if (spawnEntrance.doorDirection == AreaLoader.Direction.CENTER)
        {
            _yCoordinate = _collider.bounds.center.x;
            _xCoordinate = _collider.bounds.min.y;
        }
        else
        {
            _yCoordinate = 0;
        }        
        return new Vector2(_yCoordinate, _xCoordinate);
    }


    public void _ResetFightTriggers()
    {
        foreach (GameObject trigger in fightTriggers)
        {
            trigger.SetActive(true);
        }
    }

    public void _ResetAllEnemies()
    {
        foreach(EnemyContainer enemy in enemyList)
        {
            enemy.RespawnEnemy();
        }
    }

    public void _KillAllEnemies()
    {
        foreach(EnemyContainer enemy in enemyList)
        {
            if (enemy.gameObject.activeSelf)
            {
                _GameManager.KillEnemy(enemy);
            }
        }
    }

    public void _ResetAllObjects()
    {
        foreach(DestructableEnvironment destructableObject in destructableEnvironment)
        {
            destructableObject._ResetObject();
        }
    }
}
