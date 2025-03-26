using UnityEngine;

namespace InteractionSystem
{
    public class InteractableDouble : Interactable, IInteractableSecondary
    {
        [SerializeField] private string _secondaryInteractionPrompt;
        
        public virtual void InteractSecondary(InteractionContext context)
        {
            Debug.Log("Secondary interaction performed");
        }

        public virtual string SecondaryInteractionPrompt()
        {
            return _secondaryInteractionPrompt;
        }
    }
}
