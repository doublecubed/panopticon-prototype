using UnityEngine;
using InteractionSystem;
using InventorySystem;

public class Cube : Interactable, IInventoryItem
{
    private PlayerInventory _playerInventory;
    
    public override void Interact(InteractionContext context)
    {
        _playerInventory = FindFirstObjectByType<PlayerInventory>();
        
    }
}
