using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using InteractionSystem;

public class InputController : MonoBehaviour, IInteractionInputControl
{
    public static InputController Instance;

    [SerializeField] private InputActionAsset _globalAsset;
    [SerializeField] private InputActionAsset _playerMovement;
    [SerializeField] private InputActionAsset _monitorControl;
    [SerializeField] private InputActionAsset _interactionControl;
    [SerializeField] private InputActionAsset _cameraMovementControl;
    
    
    public bool playerControl;
    
    private InputActionMap _interactionMap;
    private InputActionMap _monitorMap;
    private InputActionMap _uiMap;
    
    private InputActionMap _cameraMovementMap;
    
    private void Awake()
    {
        Instance = this;
        _interactionMap = _interactionControl.FindActionMap("Interaction");
        _monitorMap = _monitorControl.FindActionMap("CameraSwitch");
        _cameraMovementMap = _cameraMovementControl.FindActionMap("CameraAngleMovement");
        _uiMap = InputSystem.actions.FindActionMap("UI");
        
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

    public InputAction GetCameraMovementAction()
    {
        return _cameraMovementMap.FindAction("MoveCamera");
    }
    
    public void EnableCameraAngleControl()
    {
        _playerMovement.Disable();
        _cameraMovementControl.Enable();
    }

    public void DisableCameraAngleControl()
    {
        _playerMovement.Enable();
        _cameraMovementControl.Disable();
    }

    public void EnableCameraCanvasControl()
    {
        _playerMovement.Disable();
        //_uiMap.Enable();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableCameraCanvasControl()
    {
        _playerMovement.Enable();
        //_uiMap.Disable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
