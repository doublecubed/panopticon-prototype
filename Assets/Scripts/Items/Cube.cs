using InteractionSystem;
using InventorySystem;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IInteractable, IPickupable, IActivatable, IInventoryItem, IDropable, IAttachable, IUseable, IReceiver
{
    [SerializeField] private GameObject _inventoryPrefab;
    [field: SerializeField] public string Name;
    [field: SerializeField] public Sprite Icon;

    [SerializeField] private List<Interaction> _interactions; 
    
    public string GetItemName()
    {
        return Name;
    }

    public List<Interaction> GetInteractions()
    {
        return _interactions;
    }

    public bool CanInteractWith(Interaction interaction, IInteractable interactable)
    {
        Component comp = interactable as Component;
        if (comp == null) return false;
        return true;
    }

    public Sprite GetIcon()
    {
        return Icon;
    }

    public GameObject GetInventoryPrefab()
    {
        return _inventoryPrefab;
    }

    public void Attach(InteractionContext context)
    {
        Debug.Log("Cube successfully attached");
    }

    public void Activate(InteractionContext context)
    {
        Debug.Log("Cube successfully activated");
    }

    public void Use(InteractionContext context)
    {
        Debug.Log("Cube successfully used");
    }
}
