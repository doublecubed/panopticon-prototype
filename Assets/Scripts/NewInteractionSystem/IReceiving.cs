using UnityEngine;

namespace NewInteractionSystem
{
    public interface IReceiving
    {
        public bool CanReceiveAppliable(InteractionContext context);
        
        public void ReceiveAppliable(InteractionContext context);
    }
}
