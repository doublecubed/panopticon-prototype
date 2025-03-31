using System;
using Cinemachine;
using UnityEngine;
using InteractionSystem;
using UnityEngine.Serialization;

public class CameraInteraction : MonoBehaviour, IInteractablePrimary, IInteractableSecondary
{
    [SerializeField] private InteractionObject _interactionObject;
    
    private CameraAngleAdjuster _angleAdjuster;
    private CameraModuleCanvas _moduleCanvas;
    
    public bool AdjustmentActive { get; private set; }
    public bool CanvasActive { get; private set; }

    private InteractableInfo _primaryInfo;
    private InteractableInfo _secondaryInfo;
    
    private void Awake()
    {
        _angleAdjuster = GetComponent<CameraAngleAdjuster>();
        _moduleCanvas = GetComponent<CameraModuleCanvas>();
    }


    public InteractableInfo GetInfoPrimary(InteractionContext context)
    {
        throw new NotImplementedException();
    }

    public bool CanInteractPrimary(InteractionContext context)
    {
        throw new NotImplementedException();
    }

    public void InteractPrimary(InteractionContext context)
    {
        throw new NotImplementedException();
    }

    public InteractableInfo GetInfoSecondary(InteractionContext context)
    {
        throw new NotImplementedException();
    }

    public bool CanInteractSecondary(InteractionContext context)
    {
        throw new NotImplementedException();
    }

    public void InteractSecondary(InteractionContext context)
    {
        throw new NotImplementedException();
    }
}
