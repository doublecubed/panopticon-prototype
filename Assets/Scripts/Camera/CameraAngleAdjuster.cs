using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAngleAdjuster : MonoBehaviour
{
    private PortableCamera _portableCamera;
    private CameraInteraction _interaction;
    [SerializeField] private Transform _cameraCradle;
    private InputController _inputController;
    private InputAction _cameraMoveAction;

    [SerializeField] private float _movementSpeed;
    
    private Vector2 _adjustmentInputValue;

    private void Awake()
    {
        _portableCamera = GetComponent<PortableCamera>();
        _interaction = GetComponent<CameraInteraction>();
    }

    private void Start()
    {
        _inputController = InputController.Instance;
        _cameraMoveAction = _inputController.GetCameraMovementAction();
    }

    private void Update()
    {
        if (_cameraMoveAction.enabled && _interaction.AdjustmentActive)
        {
            _adjustmentInputValue = _cameraMoveAction.ReadValue<Vector2>();
            _cameraCradle.RotateAround(_cameraCradle.position, _cameraCradle.right,
                _adjustmentInputValue.y * _movementSpeed * Time.deltaTime);
            _cameraCradle.RotateAround(_cameraCradle.position, Vector3.up,
                _adjustmentInputValue.x * _movementSpeed * Time.deltaTime);
        }
    }

    public void Activate()
    {
        _inputController.EnableCameraAngleControl();
        _portableCamera.EnableCameraPreview();
    }

    public void Deactivate()
    {
        _inputController.DisableCameraAngleControl();
        _portableCamera.DisableCameraPreview();
    }
}
