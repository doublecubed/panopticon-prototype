using System;
using UnityEngine;

public class PortableCamera : MonoBehaviour
{
    #region REFERENCES
    
    [SerializeField] private Transform _cameraCradle;

    private Camera _camera;
    private Rigidbody _rigidbody;
    private Collider _collider;

    #endregion
    
    #region MONOBEHAVIOUR
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        _camera = GetComponentInChildren<Camera>();
    }
    
    #endregion
    
    
}
