using UnityEngine;


namespace InteractionSystem.Components
{
    public abstract class BaseInteractionComponent : MonoBehaviour, IInteractionComponent
    {
        [SerializeField] private Interaction _interaction;
        
        public virtual Interaction Interaction()
        {
            return _interaction;
        }

        public virtual bool CanInteractWith(Interaction interaction, IInteractable interactable)
        {
            return true;
        }

        public virtual void Interact(InteractionContext context)
        {
            Debug.Log("Abstract class interaction triggered; Have you forgotten to implement interaction logic?");
        }
    }
}