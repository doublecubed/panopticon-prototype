using System;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using DG.Tweening;
using TMPro;

public class CameraModuleCanvas : MonoBehaviour
{
    private CameraPower _cameraPower;
    [SerializeField] private CinemachineVirtualCamera _canvasCam;

    [SerializeField] private Transform _canvasTransform;
    [SerializeField] private float _canvasOpenDuration;

    [SerializeField] private Button _powerButton;
    private Image _powerButtonImage;
    private TextMeshProUGUI _powerButtonText;
    
    private void Awake()
    {
        _cameraPower = GetComponent<CameraPower>();
        _powerButtonImage = _powerButton.GetComponent<Image>();
        _powerButtonText = _powerButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _canvasTransform.localScale = Vector3.zero;
    }

    public void OpenCanvas()
    {
        DressPowerButton();
        _canvasTransform.DOScale(Vector3.one, _canvasOpenDuration);
        _canvasCam.Priority = 12;
        InputController.Instance.EnableCameraCanvasControl();
    }

    public void CloseCanvas()
    {
        DressPowerButton();
        _canvasTransform.DOScale(Vector3.zero, _canvasOpenDuration);
        _canvasCam.Priority = 0;
        InputController.Instance.DisableCameraCanvasControl();
    }

    public void DressPowerButton()
    {
        _powerButtonImage.color = _cameraPower.IsTurnedOn ? Color.green : Color.red;
        _powerButtonText.text = "POWER: " + (_cameraPower.IsTurnedOn ? "ON" : "OFF");
    }
}
