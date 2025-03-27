using System;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraModuleCanvas : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _canvasCam;

    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private float _canvasOpenDuration;

    private void Start()
    {
        _canvasTransform.localScale = Vector3.zero;
    }

    public void OpenCanvas()
    {
        _canvasTransform.DOScale(Vector3.one, _canvasOpenDuration);
        _canvasCam.Priority = 12;
        InputController.Instance.EnableCameraCanvasControl();
    }

    public void CloseCanvas()
    {
        _canvasTransform.DOScale(Vector3.zero, _canvasOpenDuration);
        _canvasCam.Priority = 0;
        InputController.Instance.DisableCameraCanvasControl();
    }
}
