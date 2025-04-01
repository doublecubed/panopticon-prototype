using UnityEngine;

namespace NewInteractionSystem
{
    public interface ISocket
    {
        public bool CanReceiveAttachable(InteractionContext context);

        public Transform AttachmentPoint();
    }
}
