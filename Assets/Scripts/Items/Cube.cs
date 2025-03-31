using NewInteractionSystem;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IInteractable
{
    [SerializeField] private string _itemName;
    
    public string GetItemName()
    {
        return _itemName;
    }
}
