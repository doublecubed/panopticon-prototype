using UnityEngine;

namespace InteractionSystem
{
    public interface IInteractablePrimary
    {
        public InteractableInfo GetInfoPrimary(InteractionContext context);
        
        public void InteractPrimary(InteractionContext context);
    }


}