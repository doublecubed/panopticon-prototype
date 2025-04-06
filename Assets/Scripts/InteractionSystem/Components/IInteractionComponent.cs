using UnityEngine;
using InteractionSystem;

namespace InteractionSystem.Components
{
    public interface IInteractionComponent
    {
        public Interaction Interaction();
        
        public bool CanInteractWith(Interaction interaction, IInteractable interactable);

        public void Interact(InteractionContext context);
    }
}