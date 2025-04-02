using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionSolver : MonoBehaviour
    {
        #region REFERENCES
        
        private List<IInteractor> _interactors;
        
        private Dictionary<IInteractor, List<IInteractable>> _interactablesInHand;
        private Dictionary<IInteractor, List<IInteractable>> _interactablesInWorld;
        
        #endregion
        
        #region VARIABLES
        
        [field: SerializeField] public float MaxDetectionDistance { get; private set; }
        
        #endregion
        
        #region MONOBEHAVIOUR

        private void Awake()
        {
            InitializeDictionaries();
        }

        private void Update()
        {
            ResetDictionaries();
            GetInteractablesInHand();
            DetectInteractablesInWorld();
            RemoveHandInteractablesFromWorldList();
        }

        #endregion
        
        #region METHODS

        #region Initialization & Registering

        private void InitializeDictionaries()
        {
            _interactors = new List<IInteractor>();
            _interactablesInHand = new Dictionary<IInteractor, List<IInteractable>>();
            _interactablesInWorld = new Dictionary<IInteractor, List<IInteractable>>();
        }

        private void ResetDictionaries()
        {
            foreach (IInteractor interactor in _interactors)
            {
                _interactablesInHand[interactor] = new List<IInteractable>();
                _interactablesInWorld[interactor] = new List<IInteractable>();
            }
        }
        
        public void RegisterInteractor(IInteractor interactor)
        {
            _interactors.Add(interactor);
            _interactablesInHand[interactor] = new List<IInteractable>();
            _interactablesInWorld[interactor] = new List<IInteractable>();
        }
        
        #endregion
        
        #region Detection

        private void DetectInteractablesInWorld()
        {
            foreach (IInteractor interactor in _interactors)
            {
                Vector3 centerPoint = interactor.GetWorldPosition();
                
                // TODO: NonAlloc can be used, along with layermasks
                Collider[] results = Physics.OverlapSphere(centerPoint, MaxDetectionDistance);

                foreach (Collider col in results)
                {
                    IInteractable interactable = col.GetComponent<IInteractable>();
                    if (interactable != null) _interactablesInWorld[interactor].Add(interactable);
                }
            }
        }

        private void GetInteractablesInHand()
        {
            foreach (IInteractor interactor in _interactors)
            {
                _interactablesInHand[interactor] = interactor.GetHeldInteractable();
            }
        }

        private void RemoveHandInteractablesFromWorldList()
        {
            foreach (IInteractor interactor in _interactors)
            {
                foreach (IInteractable interactable in _interactablesInHand[interactor])
                {
                    if (_interactablesInWorld[interactor].Contains(interactable))
                        _interactablesInWorld[interactor].Remove(interactable);
                }
            }
        }
        
        #endregion
        
        #endregion
    }
}