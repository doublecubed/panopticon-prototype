using System;
using UnityEngine;
using InventorySystem;

namespace InteractionSystem.Components
{
    public class PickupInteraction : BaseInteractionComponent
    {
        private InventoryItem _inventoryItem;
        private PlayerInventory _inventory;

        private void Awake()
        {
            _inventoryItem = GetComponent<InventoryItem>();
        }

        public override void Interact(InteractionContext context)
        {
            _inventory = (context.Interactor as Component).GetComponent<PlayerInventory>();
            _inventory.AddItem(_inventoryItem);
            _inventory.AddCarryObject(_inventoryItem.GetInventoryPrefab());
        }
    }
}