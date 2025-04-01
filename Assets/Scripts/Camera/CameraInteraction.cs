using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraInteraction : MonoBehaviour
{
    
    private CameraAngleAdjuster _angleAdjuster;
    private CameraModuleCanvas _moduleCanvas;
    
    public bool AdjustmentActive { get; private set; }
    public bool CanvasActive { get; private set; }

    private void Awake()
    {
        _angleAdjuster = GetComponent<CameraAngleAdjuster>();
        _moduleCanvas = GetComponent<CameraModuleCanvas>();
    }



}
