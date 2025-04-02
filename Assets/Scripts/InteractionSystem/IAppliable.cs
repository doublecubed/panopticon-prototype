using UnityEngine;

namespace InteractionSystem
{
    public interface IAppliable
    {
        public void Apply(InteractionContext context);
    }
}
