using NewInteractionSystem;
using InventorySystem;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IInteractable, IPickupable, IActivatable, IInventoryItem
{
    [SerializeField] private GameObject _inventoryPrefab;
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

    public GameObject GetInventoryPrefab()
    {
        return _inventoryPrefab;
    }
}
