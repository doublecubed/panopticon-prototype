using UnityEngine;

namespace InteractionSystem
{
    public interface ISocket
    {
        public bool CanReceive(IAttachable attachable, InteractionContext context);
    }
}