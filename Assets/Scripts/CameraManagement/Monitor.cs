using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class Monitor : MonoBehaviour
{
    #region REFERENCES
    
    [SerializeField] private CameraCentral _cameraCentral;
    [SerializeField] private InputActionAsset _inputAsset;
    [SerializeField] private MeshRenderer _screenRenderer;

    public RenderTexture MonitorTexture { get; private set; }
    
    [field: SerializeField] public int CurrentCameraIndex { get; private set; }
    private bool _activeMonitor;
    
    
    private InputAction _nextCamera;
    private InputAction _previousCamera;
    
    #endregion
    
    #region MONOBEHAVIOUR

    private void Start()
    {
        CreateAndAssignRenderTexture();
        FindCameraCentral();
        SubscribeToInputs();
    }

    private void OnTriggerEnter(Collider other)
    {
        InputController.Instance.EnableMonitorControl();
        _activeMonitor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        InputController.Instance.DisableMonitorControl();
        _activeMonitor = false;
    }
    
    #endregion
    
    #region METHODS

    private void FindCameraCentral()
    {
        _cameraCentral = FindFirstObjectByType<CameraCentral>();
        _cameraCentral.AddMonitor(this);
    }
    
    private void CreateAndAssignRenderTexture()
    {
        MonitorTexture = CameraUtility.CreateRenderTexture();
        _screenRenderer.material.mainTexture = MonitorTexture;
    }

    public void SetCameraIndex(int index)
    {
        CurrentCameraIndex = index;
    }
    
    private void SubscribeToInputs()
    {
        _nextCamera = _inputAsset.actionMaps[0].FindAction("NextCamera");
        _previousCamera = _inputAsset.actionMaps[0].FindAction("PreviousCamera");

        _nextCamera.performed += NextCamera;
        _previousCamera.performed += PreviousCamera;
    }

    private void NextCamera(InputAction.CallbackContext context)
    {
        if (_activeMonitor) _cameraCentral.SwitchToNextCamera(this);
    }

    private void PreviousCamera(InputAction.CallbackContext context)
    {
        if (_activeMonitor) _cameraCentral.SwitchToPreviousCamera(this);
    }


    
    #endregion
}
