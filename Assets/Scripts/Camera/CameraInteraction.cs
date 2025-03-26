using System;
using UnityEngine;
using InteractionSystem;
using UnityEngine.Serialization;

public class CameraInteraction : InteractableDouble
{
    private CameraAngleAdjuster _angleAdjuster;

    public bool AdjustmentActive { get; private set; }
    
    private void Awake()
    {
        _angleAdjuster = GetComponent<CameraAngleAdjuster>();
    }

    public override void Interact(InteractionContext context)
    {
        if (!AdjustmentActive) {_angleAdjuster.Activate();}
        else _angleAdjuster.Deactivate();
        
        AdjustmentActive = !AdjustmentActive;
    }
}
