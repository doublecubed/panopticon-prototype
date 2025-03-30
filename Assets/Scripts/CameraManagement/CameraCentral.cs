using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraCentral : MonoBehaviour
{
    #region REFERENCES

    private CameraPreviewController _cameraPreviewController;
    [SerializeField] private List<PortableCamera> _portableCameras;
    [SerializeField] private List<Monitor> _monitors;
    
    #endregion
    
    #region MONOBEHAVIOUR

    private void Awake()
    {
        _cameraPreviewController = GetComponent<CameraPreviewController>();
        _portableCameras = new List<PortableCamera>();
        _monitors = new List<Monitor>();
    }
    
    #endregion

    #region METHODS

    public void AddMonitor(Monitor monitor)
    {
        _monitors.Add(monitor);
    }
    
    public void AddCamera(PortableCamera portableCamera)
    {
        portableCamera.SetCameraCentral(this);
        _portableCameras.Add(portableCamera);
    }

    public void RemoveCamera(PortableCamera portableCamera)
    {
        if (_portableCameras.Contains(portableCamera)) _portableCameras.Remove(portableCamera);
    }

    public void SwitchToNextCamera(Monitor monitor)
    {
        int cameraIndex = monitor.CurrentCameraIndex;
        bool flag = false;
        while (!flag)
        {
            cameraIndex = (cameraIndex + 1) % _portableCameras.Count;
            flag = _portableCameras[cameraIndex].IsOn();
        }
        SetCameraToMonitor(_portableCameras[cameraIndex], monitor);
    }

    public void SwitchToPreviousCamera(Monitor monitor)
    {
        int cameraIndex = monitor.CurrentCameraIndex;
        bool flag = false;
        while (!flag)
        {
            cameraIndex = (cameraIndex - 1);
            if (cameraIndex < 0) cameraIndex += _portableCameras.Count;
            flag = _portableCameras[cameraIndex].IsOn();
        }
        SetCameraToMonitor(_portableCameras[cameraIndex], monitor);
    }

    private void SetCameraPriorities(int primaryIndex)
    {
        for (int i = 0; i < _portableCameras.Count; i++)
        {
            if (i == primaryIndex) _portableCameras[i].SetCameraPriority(10);
            else _portableCameras[i].SetCameraPriority(1);
        }
    }

    public void ToggleCameraPreview(Camera camera, bool enable)
    {
        if (enable) _cameraPreviewController.UpdateCameraPreview(camera);
        else _cameraPreviewController.ResetCameraPreview();
    }

    private void BlitCameraToMonitor(Camera camera, Monitor monitor)
    {
        RenderTexture cameraTexture = camera.targetTexture;
        if (cameraTexture != null && monitor.MonitorTexture != null)
        {
            Graphics.Blit(cameraTexture, monitor.MonitorTexture);
        }
    }

    private void SetCameraToMonitor(PortableCamera cam, Monitor monitor)
    {
        BlitCameraToMonitor(cam.Camera, monitor);
        monitor.SetCameraIndex(_portableCameras.IndexOf(cam));
    }

    private void SetCameraToMonitor(PortableCamera cam, List<Monitor> monitors)
    {
        foreach (Monitor monitor in monitors)
        {
            SetCameraToMonitor(cam, monitor);
        }
    }
    
    #endregion
}
