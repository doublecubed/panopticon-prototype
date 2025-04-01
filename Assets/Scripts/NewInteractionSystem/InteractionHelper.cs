using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace NewInteractionSystem
{
    public static class InteractionHelper
    {
        public static InteractionType CanInteractPrimary(List<InteractableType> inventory, List<InteractableType> world)
        {
            if (inventory.Contains(InteractableType.Dropable) && world.Count == 0)
            {
                return InteractionType.Drop;                
            }


            if (inventory.Count == 0 && world.Contains(InteractableType.Pickupable))
            {
                return InteractionType.Pickup;                
            }


            if (inventory.Contains(InteractableType.Attachable) && world.Contains(InteractableType.Socket))
                return InteractionType.Attach;

            return InteractionType.None;
        }
        
        public static InteractionType CanInteractSecondary(List<InteractableType> inventory, List<InteractableType> world)
        {
            if (inventory.Contains(InteractableType.Useable) && world.Count == 0)
                return InteractionType.Use;

            if (inventory.Count == 0 && world.Contains(InteractableType.Activatable))
                return InteractionType.Activate;

            if (inventory.Contains(InteractableType.Appliable) && world.Contains(InteractableType.Receiving))
                return InteractionType.Apply;
            
            return InteractionType.None;
        }
    }

    public enum InteractionType
    {
        None,
        Pickup,
        Drop,
        Attach,
        Use,
        Activate,
        Apply
    }
    
    public enum InteractableType
    {
        None,
        Attachable,
        Socket,
        Pickupable,
        Dropable,
        Useable,
        Activatable,
        Appliable,
        Receiving,
        Damaging,
        Damageable
    }
}
