using UnityEngine;

namespace NewInteractionSystem
{
    public interface IAppliable
    {
        public void Apply(InteractionContext context);
    }
}
