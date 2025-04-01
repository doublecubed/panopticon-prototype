using UnityEngine;

namespace NewInteractionSystem
{
    public interface ISocket
    {
        public bool CanReceiveAttachable(InteractionContext context);

        public Transform AttachmentPoint();
        
        public void ReceiveAttachable(InteractionContext context);
    }
}
