using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraCentral : MonoBehaviour
{
    #region REFERENCES

    [SerializeField] private List<PortableCamera> _portableCameras;
        
    #endregion
    
    #region VARIABLES

    private int _currentCameraIndex;
    
    #endregion
    
    #region MONOBEHAVIOUR

    private void Start()
    {
        _portableCameras = new List<PortableCamera>();
    }
    
    #endregion

    #region METHODS

    public void AddCamera(PortableCamera portableCamera)
    {
        _portableCameras.Add(portableCamera);

        if (_portableCameras.Count == 1)
        {
            portableCamera.SetCameraPriority(10);
            _currentCameraIndex = 0;
        }
        else portableCamera.SetCameraPriority(1);
    }

    public void RemoveCamera(PortableCamera portableCamera)
    {
        if (_portableCameras.Contains(portableCamera)) _portableCameras.Remove(portableCamera);
    }

    public void SwitchToNextCamera()
    {
        _currentCameraIndex = (_currentCameraIndex + 1) % _portableCameras.Count;
        SetCameraPriorities(_currentCameraIndex);
    }

    public void SwitchToPreviousCamera()
    {
        _currentCameraIndex = (_currentCameraIndex - 1);
        if (_currentCameraIndex < 0) _currentCameraIndex += _portableCameras.Count;
        SetCameraPriorities(_currentCameraIndex);
    }

    private void SetCameraPriorities(int primaryIndex)
    {
        for (int i = 0; i < _portableCameras.Count; i++)
        {
            if (i == primaryIndex) _portableCameras[i].SetCameraPriority(10);
            else _portableCameras[i].SetCameraPriority(1);
        }
    }
    
    #endregion
}
