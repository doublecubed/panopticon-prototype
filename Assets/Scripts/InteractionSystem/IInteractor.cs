using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem
{
    public interface IInteractor
    {
        public List<IInteractable> GetHeldInteractable();

        public Vector3 GetWorldPosition();
        
        public Ray LookRay();
    }
}