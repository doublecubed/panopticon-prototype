using System;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private GameObject _inventorySlotPrefab;
        [SerializeField] private Transform _inventorySlotParent;
        [SerializeField] private PlayerInventory _playerInventory;

        [SerializeField] private List<InventoryItemSlot> _slots;

        private void Start()
        {
            InitializeInventoryView();
            SelectSlot(0);
        }

        private void InitializeInventoryView()
        {
            _slots = new List<InventoryItemSlot>();
            int numberOfSlots = _playerInventory.InventorySize;
            for (int i = 0; i < numberOfSlots; i++)
            {
                GameObject slot = Instantiate(_inventorySlotPrefab, _inventorySlotParent);
                InventoryItemSlot slotScript = slot.GetComponent<InventoryItemSlot>();
                _slots.Add(slotScript);
                slotScript.Initialize(i+1);
                
            }
        }

        public void SelectSlot(int slotIndex)
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                float alpha = i == slotIndex ? 0.8f : 0.2f;
                _slots[i].SetBackgroundAlpha(alpha);
            }
        }

        public void DressSlot(int slotIndex, Sprite icon)
        {
            _slots[slotIndex].SetIcon(icon);
        }

        public void UndressSlot(int slotIndex)
        {
            _slots[slotIndex].SetIcon(null);
        }
    }
}
