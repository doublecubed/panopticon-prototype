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
        public GameObject[] InventoryObjects {get; private set;}
        
        public GameObject[] CarryObjects {get; private set;}
        public IInventoryItem CurrentInventoryItem { get; private set;}
        
        
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
            InventoryObjects = new GameObject[InventorySize];
            CarryObjects = new GameObject[InventorySize];
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
            InventoryObjects[slot] = InventoryHelper.ItemObject(item);
            
            Sprite icon = item.GetIcon();
            _inventoryView.DressSlot(slot, icon);
            
            SelectInventoryItem(slot);
        }

        public void AddItem(IInventoryItem item)
        {
            AddItem(item, CurrentInventoryIndex);
        }

        public void AddCarryObject(GameObject carry, int slot)
        {
            if (!IsValidSlot(slot)) return;
            if (!SlotIsFilled(slot)) return;
            CarryObjects[slot] = carry;
        }

        public void AddCarryObject(GameObject carry)
        {
            AddCarryObject(carry, CurrentInventoryIndex);
        }
        
        public void RemoveCarryObject(int slot)
        {
            if (!IsValidSlot(slot)) return;
            GameObject carryObject = CarryObjects[slot];
            CarryObjects[slot] = null;
            Destroy(carryObject);
        }
        
        public void RemoveItem(int slot)
        {
            if (!IsValidSlot(slot)) return;
            
            Inventory[slot] = null;
            InventoryObjects[slot] = null;
            
            _inventoryView.UndressSlot(slot);
            SelectInventoryItem(slot);
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

        public bool IsValidSlot(int slot)
        {
            if (slot >= InventorySize || slot < 0) return false;
            return true;
        }

        public bool SlotIsFilled(int slot)
        {
            return Inventory[slot] != null;
        }
        
        #endregion
        
    }
}
