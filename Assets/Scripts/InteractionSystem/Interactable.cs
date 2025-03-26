using InteractionSystem;
using UnityEngine;

namespace InteractionSystem
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _interactableName;
        [SerializeField] private string _interactionPrompt;
        
        public virtual string GetName()
        {
            return _interactableName;
        }

        public virtual void Interact(InteractionContext context)
        {
            Debug.Log($"Interacted with {GetName()}");
        }

        public virtual string InteractionPrompt()
        {
            return _interactionPrompt;
        }
    }
}
