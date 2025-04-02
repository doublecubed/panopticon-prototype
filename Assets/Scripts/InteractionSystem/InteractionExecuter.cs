using System;
using InventorySystem;
using UnityEngine;

namespace InteractionSystem
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
        
        #region General

        public void ExecutePrimaryInteraction(InteractionContext interactionContext)
        {
            switch (interactionContext.PrimaryInteraction)
            {
                case InteractionType.None:
                    return;
                case InteractionType.Pickup:
                    ExecutePickup(interactionContext);
                    return;
                case InteractionType.Drop:
                    ExecuteDrop(interactionContext);
                    return;
                case InteractionType.Attach:
                    ExecuteAttach(interactionContext);
                    return;
            }
        }
        
        public void ExecuteSecondaryInteraction(InteractionContext interactionContext)
        {
            switch (interactionContext.SecondaryInteraction)
            {
                case InteractionType.None:
                    return;
                case InteractionType.Use:
                    ExecuteUse(interactionContext);
                    return;
                case InteractionType.Activate:
                    ExecuteActivate(interactionContext);
                    return;
                case InteractionType.Apply:
                    ExecuteApply(interactionContext);
                    return;
            }
        }
        
        #endregion
        
        #region Pickup
        
        private void ExecutePickup(InteractionContext context)
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
            _inventory.AddCarryObject(prefabInstance, _inventory.CurrentInventoryIndex);
        }

        private void DeactivateInventoryItem(IInventoryItem item)
        {
            GameObject itemObject = InventoryHelper.ItemObject(item);
            itemObject.transform.SetParent(_inventoryStorageTransform);
            itemObject.transform.position = _inventoryStorageTransform.position;
            itemObject.SetActive(false);
        }
        
        #endregion
        
        #region Drop

        private void ExecuteDrop(InteractionContext context)
        {
            int slot = _inventory.CurrentInventoryIndex;
            _inventory.RemoveCarryObject(slot);
            Vector3 dropPoint = context.Hit.point;
            
            GameObject dropObject = _inventory.InventoryObjects[slot];
            _inventory.RemoveItem(slot);
            
            dropObject.transform.SetParent(null);
            dropObject.transform.position = dropPoint;
            dropObject.SetActive(true);
        }
        
        #endregion
        
        #region Attach

        //TODO: This is very similar to Drop, so they can be combined. Maybe with overload methods
        private void ExecuteAttach(InteractionContext context)
        {
            Transform attachTransform = InventoryHelper.ItemObject(context.WorldInteractable).transform;
            Vector3 attachPosition = attachTransform.position;
            
            int slot = _inventory.CurrentInventoryIndex;
            _inventory.RemoveCarryObject(slot);
            
            GameObject dropObject = _inventory.InventoryObjects[slot];
            _inventory.RemoveItem(slot);
            
            dropObject.transform.SetParent(attachTransform);
            dropObject.transform.position = attachPosition;
            dropObject.transform.rotation = attachTransform.rotation;
            dropObject.SetActive(true);
            
            (context.InventoryInteractable as IAttachable).Attach(context);
            (context.WorldInteractable as ISocket).ReceiveAttachable(context);
        }
        
        #endregion
        
        #region Use

        private void ExecuteUse(InteractionContext context)
        {
            (context.InventoryInteractable as IUseable).Use(context);
        }
        
        #endregion
        
        #region Activate

        private void ExecuteActivate(InteractionContext context)
        {
            (context.WorldInteractable as IActivatable).Activate(context);
        }
        
        #endregion

        #region Apply
        
        private void ExecuteApply(InteractionContext context)
        {
            (context.InventoryInteractable as IAppliable).Apply(context);
            (context.WorldInteractable as IReceiving).ReceiveAppliable(context);
        }
        
        #endregion
        
        #endregion
    }
}
