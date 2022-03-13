using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    private Collider2D _cameraBounds;
    private BoxCollider2D _playerBounds;
    private BoundsManager _boundsManager;
    public float LockOffsetZ = 0;
    public bool LockRight = true;
    public bool LockLeft = true;
    public bool LockUp = true;
    public bool LockDown = true;
    public bool FreezeLock = false;



    private void Start()
    {
        _boundsManager = GetComponentInParent<BoundsManager>();
        _cameraBounds = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null)
        {
            _playerBounds = _colInfo.GetComponent<BoxCollider2D>();
            _boundsManager.mainCamera.ActiveBoundary = _cameraBounds;
            _boundsManager.mainCamera.LockRight = LockRight;
            _boundsManager.mainCamera.LockLeft = LockLeft;
            _boundsManager.mainCamera.LockUp = LockUp;
            _boundsManager.mainCamera.LockDown = LockDown;
            _boundsManager.mainCamera.LockOffsetZ = LockOffsetZ;
            _boundsManager.mainCamera.FreezeLock = FreezeLock;
        }
    }

    private void OnTriggerExit2D(Collider2D _colInfo)
    {
        if (_colInfo == _playerBounds && _boundsManager.mainCamera.ActiveBoundary == _cameraBounds)
        {
            _boundsManager.mainCamera.ActiveBoundary = null;
            _boundsManager.mainCamera.LockRight = false;
            _boundsManager.mainCamera.LockLeft = false;
            _boundsManager.mainCamera.LockUp = false;
            _boundsManager.mainCamera.LockDown = false;
            _boundsManager.mainCamera.LockOffsetZ = 0;
            _boundsManager.mainCamera.FreezeLock = false;
        }
    }

    private void OnTriggerStay2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null && _boundsManager.mainCamera.ActiveBoundary == null)
        {
            _playerBounds = _colInfo.GetComponent<BoxCollider2D>();
            _boundsManager.mainCamera.ActiveBoundary = _cameraBounds;
            _boundsManager.mainCamera.LockRight = LockRight;
            _boundsManager.mainCamera.LockLeft = LockLeft;
            _boundsManager.mainCamera.LockUp = LockUp;
            _boundsManager.mainCamera.LockDown = LockDown;
            _boundsManager.mainCamera.LockOffsetZ = LockOffsetZ;
            _boundsManager.mainCamera.FreezeLock = FreezeLock;
        }
    }
}
