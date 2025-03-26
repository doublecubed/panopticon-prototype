using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using InteractionSystem;

public class InputController : MonoBehaviour, IInteractionInputControl
{
    public static InputController Instance;
    
    [SerializeField] private InputActionAsset _playerMovement;
    [SerializeField] private InputActionAsset _monitorControl;
    [SerializeField] private InputActionAsset _interactionControl;
    [SerializeField] private InputActionAsset _cameraMovementControl;
    
    
    public bool playerControl;
    
    private InputActionMap _interactionMap;
    private InputActionMap _monitorMap;
    
    private void Awake()
    {
        Instance = this;
        _interactionMap = _interactionControl.FindActionMap("Interaction");
        _monitorMap = _monitorControl.FindActionMap("CameraSwitch");
    }

    private void Update()
    {
        if (playerControl) _playerMovement.Enable();
        else _playerMovement.Disable();
    }

    public List<InputAction> GetInteractionActions()
    {
        List<InputAction> actions = new List<InputAction>();
        actions.Add(_interactionMap.FindAction("PrimaryInteract"));
        actions.Add(_interactionControl.FindAction("SecondaryInteract"));
        return actions;
    }

    public void EnableInteractionControl()
    {
        _interactionMap.Enable();
    }

    public void DisableInteractionControl()
    {
        _interactionMap.Disable();
    }

    public void EnableMonitorControl()
    {
        _monitorMap.Enable();
    }

    public void DisableMonitorControl()
    {
        _monitorMap.Disable();
    }
}
