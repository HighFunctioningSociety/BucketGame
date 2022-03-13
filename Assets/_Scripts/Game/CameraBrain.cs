using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBrain : MonoBehaviour
{
    public Transform Target;
    public Vector3 MenuPosition;
    public Vector3 Offset;
    private Vector3 _velocity = Vector2.zero;
    public float Smoothing= 0.125f;
    public float LockOffsetZ;

    public BoxCollider2D CameraBox;
    public Collider2D ActiveBoundary;
    public bool LockRight;
    public bool LockLeft;
    public bool LockUp;
    public bool LockDown;
    public bool FreezeLock;

    [SerializeField] private Vector3 _desiredPosition;
    [SerializeField] private Vector3 _smoothedPosition;

    private float _xMin, _xMax;
    private float _yMin, _yMax;
    private float _zValue;
    private Vector2 _cameraBoxOriginalValue;

    public Vector3 LastTargetPosition;

    public void Start()
    {
        MenuPosition = this.transform.position;
        _cameraBoxOriginalValue = CameraBox.size;

        if (Target != null)
        {
            transform.position = Target.position + Offset;
        }
    }

    public void Update()
    {
        if (BlankerAnimator.blanker.transitionInProgress && Target != null)
        {
            transform.position = _desiredPosition;
        }
    }

    private void LateUpdate()
    {
        if (Target == null)
            return;

        FollowPlayer();
    }

    private void SetCameraBounds()
    {
        if (LockDown) _yMin = ActiveBoundary.bounds.min.y + CameraBox.size.y / 2;
        else _yMin =Target.position.y + Offset.y;

        if (LockUp) _yMax = ActiveBoundary.bounds.max.y - CameraBox.size.y / 2;
        else _yMax = Target.position.y + Offset.y;

        if (LockLeft) _xMin = ActiveBoundary.bounds.min.x + CameraBox.size.x / 2;
        else _xMin = Target.position.x + Offset.x;

        if (LockRight) _xMax = ActiveBoundary.bounds.max.x - CameraBox.size.x / 2;
        else _xMax = Target.position.x + Offset.x;

        _zValue = Target.position.z + Offset.z + LockOffsetZ;
    }

    private void FollowPlayer()
    {
        if (ActiveBoundary != null)
        {
            SetCameraBounds();
            Vector3 newCameraBounds = new Vector3(Mathf.Clamp(Target.position.x, _xMin, _xMax),
                                                  Mathf.Clamp(Target.position.y, _yMin, _yMax),
                                                  _zValue);

            _desiredPosition = FreezeLock ? LastTargetPosition : newCameraBounds;
            AdjustCameraBounds();
        }
        else
        {
            _desiredPosition = Target.position + Offset;
            CameraBox.size = _cameraBoxOriginalValue;
        }
        LastTargetPosition = _desiredPosition;
        _smoothedPosition = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _velocity, Smoothing);
        transform.position = _smoothedPosition;
    }

    private void AdjustCameraBounds()
    {
        float xRatio = _cameraBoxOriginalValue.x / Mathf.Abs(Offset.z);
        float yRatio = _cameraBoxOriginalValue.y / Mathf.Abs(Offset.z);
        float newOffset = Mathf.Abs(Offset.z + LockOffsetZ);
        float newXBounds = newOffset * xRatio;
        float newYBounds = newOffset * yRatio;

        CameraBox.size = new Vector2(newXBounds, newYBounds);
    }

    private void LockSmoothing()
    {

    }

    public void MainMenuPosition()
    {
        transform.position = MenuPosition;
    }

    private IEnumerator SmoothTransition()
    {
        yield return new WaitForEndOfFrame();
    }
}
