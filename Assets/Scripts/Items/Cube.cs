using NewInteractionSystem;
using InventorySystem;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IInteractable, IPickupable, IActivatable, IInventoryItem
{
    [field: SerializeField] public string Name;
    [field: SerializeField] public Sprite Icon; 
    
    public string GetItemName()
    {
        return Name;
    }

    public Sprite GetIcon()
    {
        return Icon;
    }
}
