using UnityEngine;
using System.Collections.Generic;

namespace InteractionSystem
{
    public class InteractionContext
    {
        public List<InteractionSet> InteractionSets { get; private set; }
        
        public FirstPersonPlayerInteractor Interactor { get; private set; }
    
        public RaycastHit RaycastHit { get; private set; }
    
        public float Distance { get; private set; }
    
        public Vector3 InteractionDirection { get; private set; }
    
        public Camera PlayerCamera { get; private set; }
    
        public bool IsHoldingInteractButton { get; set; }
    
        public InteractionContext(FirstPersonPlayerInteractor interactor, RaycastHit hit, Camera playerCamera, List<InteractionSet> interactionSets)
        {
            InteractionSets = interactionSets;
            Interactor = interactor;
            RaycastHit = hit;
            PlayerCamera = playerCamera;
            Distance = Vector3.Distance(interactor.transform.position, hit.point);
            InteractionDirection = (hit.point - interactor.transform.position).normalized;
        }
    }
}
