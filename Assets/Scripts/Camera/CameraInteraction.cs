using System;
using Cinemachine;
using UnityEngine;
using InteractionSystem;
using UnityEngine.Serialization;

public class CameraInteraction : InteractableDouble
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

    public override void Interact(InteractionContext context)
    {
        if (!AdjustmentActive) _angleAdjuster.Activate();
        else _angleAdjuster.Deactivate();
        
        AdjustmentActive = !AdjustmentActive;
    }

    public override void InteractSecondary(InteractionContext context)
    {
        if (!CanvasActive) _moduleCanvas.OpenCanvas();
        else _moduleCanvas.CloseCanvas();
        
        CanvasActive = !CanvasActive;
    }
}
