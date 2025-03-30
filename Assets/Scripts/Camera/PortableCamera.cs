using System;
using UnityEngine;

public class PortableCamera : MonoBehaviour
{
    #region REFERENCES

    private CameraCentral _cameraCentral;
    [SerializeField] private Transform _cameraCradle;

    [SerializeField] private Camera _camera;
    [SerializeField] private Camera _previewCamera;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private CameraPower _cameraPower;
    
    
    #endregion
    
   
    #region MONOBEHAVIOUR
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _cameraPower = GetComponent<CameraPower>();
    }

    private void Start()
    {
        TurnOn();
    }

    #endregion
    
    #region METHODS

    public void SetCameraCentral(CameraCentral cameraCentral)
    {
        _cameraCentral = cameraCentral;
    }
    
    public bool IsOn()
    {
        return _cameraPower.IsTurnedOn;
    }
    
    public void SetCameraPriority(int value)
    {
        _camera.depth = value;
    }

    public void TurnOn()
    {
        _cameraPower.SwitchOn();
    }

    public void TurnOff()
    {
        _cameraPower.SwitchOff();
    }

    public void EnableCameraPreview()
    {
        _previewCamera.enabled = true;
        _cameraCentral.ToggleCameraPreview(_previewCamera, true);
    }

    public void DisableCameraPreview()
    {
        _previewCamera.enabled = false;
        _cameraCentral.ToggleCameraPreview(_previewCamera, false);
    }
    
    #endregion
    
}
