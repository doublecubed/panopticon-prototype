using System;
using UnityEngine;

public class CameraPreviewController : MonoBehaviour
{
    [SerializeField] private GameObject _renderTextureObject;
    [SerializeField] private RenderTexture _renderTexture;

    private Camera _currentPreviewCamera;

    private void Start()
    {
        _renderTextureObject.SetActive(false);
    }

    public void UpdateCameraPreview(Camera camera)
    {
        _currentPreviewCamera = camera;
        _currentPreviewCamera.targetTexture = _renderTexture;   
        _renderTextureObject.SetActive(true);
    }

    public void ResetCameraPreview()
    {
        _renderTextureObject.SetActive(false);
        _currentPreviewCamera.targetTexture = null;
        _currentPreviewCamera = null;
    }
}
