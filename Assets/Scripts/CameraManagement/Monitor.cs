using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Monitor : MonoBehaviour
{
    #region REFERENCES
    
    [SerializeField] private CameraCentral _cameraCentral;
    [SerializeField] private InputActionAsset _inputAsset;
    
    
    private InputAction _nextCamera;
    private InputAction _previousCamera;
    
    #endregion
    
    #region MONOBEHAVIOUR

    private void Start()
    {
        _cameraCentral = FindFirstObjectByType<CameraCentral>();
        SubscribeToInputs();
    }

    private void OnTriggerEnter(Collider other)
    {
        InputController.Instance.EnableMonitorControl();
        //_nextCamera.Enable();
        //_previousCamera.Enable();
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.Instance.DisableMonitorControl();
        //_nextCamera.Disable();
        //_previousCamera.Disable();
    }
    
    #endregion
    
    #region METHODS

    private void SubscribeToInputs()
    {
        _nextCamera = _inputAsset.actionMaps[0].FindAction("NextCamera");
        _previousCamera = _inputAsset.actionMaps[0].FindAction("PreviousCamera");

        _nextCamera.performed += NextCamera;
        _previousCamera.performed += PreviousCamera;
    }

    private void NextCamera(InputAction.CallbackContext context)
    {
        _cameraCentral.SwitchToNextCamera();
    }

    private void PreviousCamera(InputAction.CallbackContext context)
    {
        _cameraCentral.SwitchToPreviousCamera();
    }
    
    #endregion
}
