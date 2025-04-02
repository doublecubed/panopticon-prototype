using UnityEngine;
using System.Collections.Generic;

namespace InteractionSystem
{
    public class InteractionContext
    {
        public Interactor Interactor { get; private set; }
        
        public IInteractable InventoryInteractable { get; private set; }
        
        public IInteractable WorldInteractable { get; private set; }
        public InteractionType PrimaryInteraction { get; private set; }
        public InteractionType SecondaryInteraction { get; private set; }
        public List<InteractableType> InventoryInteractableTypes { get; private set; }
        public List<InteractableType> WorldInteractableTypes { get; private set; }
        public RaycastHit Hit { get; private set; }
        public Camera PlayerCamera { get; private set; }
        
        
        public InteractionContext(Interactor interactor, 
            IInteractable inventoryInteractable, IInteractable worldInteractable, 
            InteractionType primaryInteraction, InteractionType secondaryInteraction, 
            List<InteractableType> inventoryInteractableTypes, List<InteractableType> worldInteractableTypes, 
            RaycastHit hit, Camera camera)
        {
            Interactor = interactor;
            InventoryInteractable = inventoryInteractable;
            WorldInteractable = worldInteractable;
            PrimaryInteraction = primaryInteraction;
            SecondaryInteraction = secondaryInteraction;
            InventoryInteractableTypes = inventoryInteractableTypes;
            WorldInteractableTypes = worldInteractableTypes;
            Hit = hit;
            PlayerCamera = camera;
        }
    }
}
