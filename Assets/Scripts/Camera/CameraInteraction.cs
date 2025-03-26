using System;
using UnityEngine;
using InteractionSystem;

public class CameraInteraction : InteractableDouble
{
    private CameraAngleAdjuster _angleAdjuster;

    private void Awake()
    {
        _angleAdjuster = GetComponent<CameraAngleAdjuster>();
    }

    public override void Interact(InteractionContext context)
    {
        
    }
}
