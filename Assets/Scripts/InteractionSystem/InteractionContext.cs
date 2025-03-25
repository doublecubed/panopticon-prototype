using UnityEngine;

namespace InteractionSystem
{
    public class InteractionContext
    {
        // The player/character initiating the interaction
        public FirstPersonPlayerInteractor Interactor { get; private set; }
    
        // Original raycast data for precise interaction point
        public RaycastHit RaycastHit { get; private set; }
    
        // Interaction distance
        public float Distance { get; private set; }
    
        // Direction vector from player to interactable
        public Vector3 InteractionDirection { get; private set; }
    
        // Optional: Camera for determining view angles
        public Camera PlayerCamera { get; private set; }
    
        // Optional: Input state
        public bool IsHoldingInteractButton { get; set; }
    
        public InteractionContext(FirstPersonPlayerInteractor interactor, RaycastHit hit, Camera playerCamera)
        {
            Interactor = interactor;
            RaycastHit = hit;
            PlayerCamera = playerCamera;
            Distance = Vector3.Distance(interactor.transform.position, hit.point);
            InteractionDirection = (hit.point - interactor.transform.position).normalized;
        }
    }
}
