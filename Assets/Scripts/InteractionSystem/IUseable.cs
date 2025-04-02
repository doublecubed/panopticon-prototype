using UnityEngine;

namespace InteractionSystem
{
    public interface IUseable
    {
        public void Use(InteractionContext context);
    }
}
