using System;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using InteractionSystem;
using UnityEngine.InputSystem;

public class FirstPersonInteractor : MonoBehaviour, IInteractor
{
    #region REFERENCES

    private InteractionSolver _solver;
    [SerializeField] private InputController _inputController;
    [SerializeField] private Camera _playerCam;
    private PlayerInventory _inventory;
    
    #endregion
    
    #region MONOBEHAVIOUR

    private void Awake()
    {
        _inventory = GetComponent<PlayerInventory>();
    }

    private void Start()
    {
        _solver = FindFirstObjectByType<InteractionSolver>();
        _solver.RegisterInteractor(this);

        RegisterInputs();
    }


    #endregion

    private void RegisterInputs()
    {
        _inputController.EnableInteractionControl();
        List<InputAction> actions = _inputController.GetInteractionActions();
        actions[0].performed += PrimaryInteract;
        actions[1].performed += SecondaryInteract;
    }

    private void PrimaryInteract(InputAction.CallbackContext obj)
    {
        InteractionContext context = _solver.InteractionSets[this].InteractionContexts[InteractionCategory.Primary];

        if (context.Interaction == null) return;
        
        if (context.Interaction.RequiresInHand) context.HandInteractable.Interact(context);
        if (context.Interaction.RequiresInWorld) context.WorldInteractable.Interact(context);

    }

    private void SecondaryInteract(InputAction.CallbackContext obj)
    {
        InteractionContext context = _solver.InteractionSets[this].InteractionContexts[InteractionCategory.Secondary];
        
        if (context.Interaction == null) return;
        
        if (context.Interaction.RequiresInHand) context.HandInteractable.Interact(context);
        if (context.Interaction.RequiresInWorld) context.WorldInteractable.Interact(context);
    }
    
    public List<IInteractable> GetHeldInteractable()
    {
        List<IInteractable> interactables = new List<IInteractable>();
    
        IInteractable interactable = _inventory.CurrentInventoryItem as IInteractable;
        if (interactable != null) interactables.Add(interactable);
    
        return interactables;
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }

    public Vector3 GetLookDirection()
    {
        return transform.forward;
    }
    
    public Ray LookRay()
    {
        return _playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
    }
}