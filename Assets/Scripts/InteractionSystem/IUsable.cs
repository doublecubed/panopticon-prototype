using UnityEngine;

namespace InteractionSystem
{
    public interface IUsable
    {
        public bool Use(InteractionContext context);
    }
}
