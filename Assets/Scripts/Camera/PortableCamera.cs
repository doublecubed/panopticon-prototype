using System;
using UnityEngine;

public class PortableCamera : MonoBehaviour
{
    #region REFERENCES
    
    [SerializeField] private Transform _cameraCradle;

    private Camera _camera;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private CameraPower _cameraPower;
    
    
    #endregion
    
   
    #region MONOBEHAVIOUR
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _camera = GetComponentInChildren<Camera>();
        _cameraPower = GetComponent<CameraPower>();
    }

    private void Start()
    {
        TurnOn();
    }

    #endregion
    
    #region METHODS

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
    
    #endregion
    
}
