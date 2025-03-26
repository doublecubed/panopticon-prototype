using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace InteractionSystem
{
    public interface IInteractionInputControl
    {
        public List<InputAction> GetInteractionActions(); 
        
        public void EnableInteractionControl();
    
        public void DisableInteractionControl();
    }
}
