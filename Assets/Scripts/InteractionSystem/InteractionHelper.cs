using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace InteractionSystem
{
    public static class InteractionHelper
    {
        

    }

    public enum InteractionCategory
    {
        Primary,        // physical interactions like pickup, drop, attach
        Secondary,      // use-based interactions like use, activate, apply
        Tetriary        // reserved for a third button. May be contextual
    }
    

    public enum InteractionTargeting
    {
        Raycast,
        Spherecast,
        Vicinity
    }
    
    public class InteractionSet
    {
        public InteractionSet()
        {
            InteractionContexts = new Dictionary<InteractionCategory, InteractionContext>();
            
            foreach (InteractionCategory category in Enum.GetValues(typeof(InteractionCategory)))
            {
                InteractionContexts[category] = new InteractionContext();
            }
        }

        public Dictionary<InteractionCategory, InteractionContext> InteractionContexts;
    }
    
    public struct InteractionProspect
    {
        public InteractionProspect(Interaction interaction, IInteractable hand = null, 
            IInteractable world = null)
        {
            Interaction = interaction;
            HandInteractable = hand;
            WorldInteractable = world;
        }
        
        public Interaction Interaction;
        public IInteractable HandInteractable;
        public IInteractable WorldInteractable;
    }
    
    public struct InteractionContext
    {
        public InteractionContext(IInteractor interactor, Interaction interaction, 
            IInteractable hand = null, IInteractable world = null, 
            RaycastHit hit = new RaycastHit())
        {
            Interactor = interactor;
            Interaction = interaction;
            HandInteractable = hand;
            WorldInteractable = world;
            Hit = hit;
        }

        public Interaction Interaction;
        public IInteractor Interactor;
        public IInteractable HandInteractable;
        public IInteractable WorldInteractable;
        public RaycastHit Hit;
    }
}
