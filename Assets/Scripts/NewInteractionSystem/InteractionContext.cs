using UnityEngine;
using System.Collections.Generic;

namespace NewInteractionSystem
{
    public class InteractionContext
    {
        public Interactor Interactor { get; private set; }
        
        public IInteractable InventoryInteractable { get; private set; }
        
        public IInteractable WorldInteractable { get; private set; }
        public InteractionType PrimaryInteraction { get; private set; }
        public InteractionType SecondaryInteraction { get; private set; }
        public List<InteractableType> InventoryInteractables { get; private set; }
        public List<InteractableType> WorldInteractables { get; private set; }
        public RaycastHit Hit { get; private set; }
        public Camera PlayerCamera { get; private set; }
        
        
        public InteractionContext(Interactor interactor, 
            IInteractable inventoryInteractable, IInteractable worldInteractable, 
            InteractionType primaryInteraction, InteractionType secondaryInteraction, 
            List<InteractableType> inventoryInteractables, List<InteractableType> worldInteractables, 
            RaycastHit hit, Camera camera)
        {
            Interactor = interactor;
            InventoryInteractable = inventoryInteractable;
            WorldInteractable = worldInteractable;
            PrimaryInteraction = primaryInteraction;
            SecondaryInteraction = secondaryInteraction;
            InventoryInteractables = inventoryInteractables;
            WorldInteractables = worldInteractables;
            Hit = hit;
            PlayerCamera = camera;
        }
    }
}
