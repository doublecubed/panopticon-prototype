using System;
using System.Collections.Generic;
using UnityEngine;


namespace InteractionSystem.Components
{
    public class InteractionCoordinator : MonoBehaviour, IReceiver
    {
        #region REFERENCES

        private InteractableData _data;
        [SerializeField] private List<Interaction> _interactions;
        private Dictionary<Interaction, IInteractionComponent> _components;
        
        #endregion
        
        #region VARIABLES

        [SerializeField] private string _name;
        
        #endregion
        
        #region MONOBEHAVIOUR
        
        private void Awake()
        {
            InitializeReferences();
        }

        #endregion
        
        #region INTERFACE
        
        public string GetItemName()
        {
            return _name;
        }

        public List<Interaction> GetInteractions()
        {
            return _interactions;
        }

        public void Interact(InteractionContext context)
        {
            _components[context.Interaction].Interact(context);
        }

        public bool CanInteractWith(Interaction interaction, IInteractable interactable)
        {
            return _components[interaction].CanInteractWith(interaction, interactable);
        }
        
        #endregion
        
        #region METHODS

        private void InitializeReferences()
        {
            _data = GetComponent<InteractableData>();
            _interactions = new List<Interaction>();
            _components = new Dictionary<Interaction, IInteractionComponent>();
            
            IInteractionComponent[] components = GetComponents<IInteractionComponent>();
            foreach (IInteractionComponent component in components)
            {
                _interactions.Add(component.Interaction());
                _components[component.Interaction()] = component;
            }
        }
        
        #endregion
    }
}