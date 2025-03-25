using UnityEngine;

namespace InteractionSystem
{
    public interface IInteractable
    {
        public string GetName();
        
        public void Interact(InteractionContext context);
        
        public string InteractionPrompt();
    }
}