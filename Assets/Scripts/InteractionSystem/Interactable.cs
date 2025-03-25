using InteractionSystem;
using UnityEngine;

namespace InteractionSystem
{
    public class Interactable : MonoBehaviour, IInteractable, IInteractableSecondary
    {
        [SerializeField] private string _interactableName;
        [SerializeField] private string _interactionPrompt;
        [SerializeField] private string _secondaryInteractionPrompt;
        
        public string GetName()
        {
            return _interactableName;
        }

        public void Interact(InteractionContext context)
        {
            Debug.Log($"Interacted with {GetName()}");
        }

        public string InteractionPrompt()
        {
            return _interactionPrompt;
        }

        public void InteractSecondary(InteractionContext context)
        {
            Debug.Log($"Interacted with {GetName()} in a slightly different manner");
        }

        public string SecondaryInteractionPrompt()
        {
            return _secondaryInteractionPrompt;
        }
    }
}
