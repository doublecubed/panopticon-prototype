using System.Collections.Generic;

namespace InteractionSystem
{
    public interface IReceiver : IInteractable
    {
        public bool CanInteractWith(Interaction interaction, IInteractable interactable);
    }
}