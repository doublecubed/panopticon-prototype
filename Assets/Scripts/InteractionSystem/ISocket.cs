using UnityEngine;

namespace InteractionSystem
{
    public interface ISocket
    {
        public bool CanReceiveAttachable(InteractionContext context);

        public Transform AttachmentPoint();
        
        public void ReceiveAttachable(InteractionContext context);
    }
}
