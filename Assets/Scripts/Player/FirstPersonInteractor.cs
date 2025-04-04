using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using InteractionSystem;

public class FirstPersonInteractor : MonoBehaviour, IInteractor
{
    #region REFERENCES
    
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
        InteractionSolver solver = FindFirstObjectByType<InteractionSolver>();
        solver.RegisterInteractor(this);
    }

    #endregion
    
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