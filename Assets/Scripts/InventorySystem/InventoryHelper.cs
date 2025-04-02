using InteractionSystem;
using UnityEngine;

namespace InventorySystem
{
    public static class InventoryHelper
    {
        public static GameObject InventoryItemObject(IInventoryItem item)
        {
            if (item == null) return null;
            return ((Component)item).gameObject;
        }

        public static GameObject InteractableItemObject(IInteractable item)
        {
            if (item == null) return null;
            return ((Component)item).gameObject;
        }
        
        public static GameObject ItemObject<T>(T item) where T : class
        {
            if (item == null) return null;
            if (item is Component component)
                return component.gameObject;
            return null;
        }
    }
}