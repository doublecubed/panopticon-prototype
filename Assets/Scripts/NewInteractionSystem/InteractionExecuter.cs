using System;
using InventorySystem;
using UnityEngine;

namespace NewInteractionSystem
{ 
    public class InteractionExecuter : MonoBehaviour
    {
        #region REFERENCES

        private PlayerInventory _inventory;
        [SerializeField] private Transform _carryTransform;
        [SerializeField] private Transform _inventoryStorageTransform;
        
        #endregion
        
        #region MONOBEHAVIOUR

        private void Awake()
        {
            _inventory = GetComponent<PlayerInventory>();
        }

        #endregion
        
        #region METHODS
        
        public void ExecutePickup(InteractionContext context)
        {
            IInventoryItem item = context.WorldInteractable as IInventoryItem; 
            _inventory.AddItem(item);
            SpawnInventoryPrefab(item.GetInventoryPrefab());
            DeactivateInventoryItem(item);
        }

        private void SpawnInventoryPrefab(GameObject prefab)
        {
            GameObject prefabInstance = 
                Instantiate(prefab,  _carryTransform.position, _carryTransform.rotation, _carryTransform);
        }

        private void DeactivateInventoryItem(IInventoryItem item)
        {
            GameObject itemObject = InventoryHelper.ItemObject(item);
            itemObject.transform.SetParent(_inventoryStorageTransform);
            itemObject.transform.position = _inventoryStorageTransform.position;
            itemObject.SetActive(false);
        }
        
        #endregion
    }
}
