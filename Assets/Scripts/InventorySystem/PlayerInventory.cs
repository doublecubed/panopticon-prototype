using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    public class PlayerInventory : MonoBehaviour
    {
        #region REFERENCES
        
        [SerializeField] private InventoryView _inventoryView;
        private InputController _inputController;
        public IInventoryItem[] Inventory {get; private set;}
        [field: SerializeField] public IInventoryItem CurrentInventoryItem { get; private set;}

        #endregion
        
        #region VARIABLES

        [field: SerializeField] public int InventorySize { get; private set;}
        [field: SerializeField] public int CurrentInventoryIndex { get; private set;}
        
        #endregion
        
        #region EVENTS

        private InputAction _scroll;
        
        #endregion
        
        #region MONOBEHAVIOUR

        private void Start()
        {
            _inputController = FindFirstObjectByType<InputController>();
            _scroll = _inputController.InventoryMap.FindAction("Scroll");
            _inputController.EnableInventoryControl();
            Inventory = new IInventoryItem[InventorySize];
            CurrentInventoryIndex = 0;
        }

        private void Update()
        {
            float scrollValue = _scroll.ReadValue<float>();
            if (scrollValue > 0) SelectNextSlot();
            if (scrollValue < 0) SelectPreviousSlot();
        }

        #endregion
        
        #region METHODS

        
        
        public void AddItem(IInventoryItem item, int slot)
        {
            if (slot >= InventorySize || slot < 0) return;
            if (Inventory[slot] != null) return;

            Inventory[slot] = item;
        }

        public void RemoveItem(IInventoryItem item, int slot)
        {
            if (slot >= InventorySize || slot < 0) return;
            if (Inventory[slot] == null) return;
            
            Inventory[slot] = null;
        }

        private void SelectNextSlot()
        {
            CurrentInventoryIndex = (CurrentInventoryIndex + 1) % InventorySize;
            SelectInventoryItem(CurrentInventoryIndex);
            
            _inventoryView.SelectSlot(CurrentInventoryIndex);
        }

        private void SelectPreviousSlot()
        {
            CurrentInventoryIndex = (CurrentInventoryIndex - 1);
            if (CurrentInventoryIndex < 0) CurrentInventoryIndex += InventorySize;
            SelectInventoryItem(CurrentInventoryIndex);
            
            _inventoryView.SelectSlot(CurrentInventoryIndex);
        }

        private void SelectInventoryItem(int index)
        {
            if (Inventory[index] == null) CurrentInventoryItem = null;
            else CurrentInventoryItem = Inventory[index];
        }
        
        #endregion
        
    }
}
