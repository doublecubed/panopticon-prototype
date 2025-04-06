using System.Collections.Generic;

namespace InteractionSystem
{
    public interface IInteractable
    {
        public string GetItemName();
        public List<Interaction> GetInteractions();

        public void Interact(InteractionContext context);
    }
}
