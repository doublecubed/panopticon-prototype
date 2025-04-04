using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionSolver : MonoBehaviour
    {
        #region REFERENCES
        
        private List<IInteractor> _interactors;
        
        private Dictionary<IInteractor, List<IInteractable>> _interactablesInHand;
        private Dictionary<IInteractor, List<IInteractable>> _interactablesInWorld;

        private Dictionary<IInteractor, List<Interaction>> _handInteractions;
        private Dictionary<IInteractor, List<Interaction>> _worldInteractions;

        private Dictionary<IInteractor, List<InteractionProspect>> _prospects;
        
        #endregion
        
        #region VARIABLES
        
        [field: SerializeField] public float MaxDetectionDistance { get; private set; }
        [field: SerializeField] public float DetectionAngle { get; private set; }
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
            
            GatherHandInteractions();
            GatherWorldInteractions();
            
            CreateProspects();
            EliminateProspects();
            
            Debug.Log($"There are {_prospects[_interactors[0]].Count} prospects");
            foreach (InteractionProspect prospect in _prospects[_interactors[0]])
            {
                Debug.Log($"Interaction: {prospect.Interaction.Name}");
                Debug.Log($"Hand Interactable: {prospect.HandInteractable}");
                Debug.Log($"World Interactable: {prospect.WorldInteractable}");
                
            }
            
            //Eliminate prospects based on viability (distance, angle etc)
            //Sort prospects into categories (primary, secondary, tetriary)
            //Sort categories for priority
            //Cast for the top prospects (if necessary), move down if not viable
            //Combine the first viable ones into one final InteractionSet


        }

        #endregion
        
        #region METHODS

        #region Initialization & Registering

        private void InitializeDictionaries()
        {
            _interactors = new List<IInteractor>();
            _interactablesInHand = new Dictionary<IInteractor, List<IInteractable>>();
            _interactablesInWorld = new Dictionary<IInteractor, List<IInteractable>>();
            _handInteractions = new Dictionary<IInteractor, List<Interaction>>();
            _worldInteractions = new Dictionary<IInteractor, List<Interaction>>();
            _prospects = new Dictionary<IInteractor, List<InteractionProspect>>();
        }

        private void ResetDictionaries()
        {
            foreach (IInteractor interactor in _interactors)
            {
                _interactablesInHand[interactor] = new List<IInteractable>();
                _interactablesInWorld[interactor] = new List<IInteractable>();
                _handInteractions[interactor] = new List<Interaction>();
                _worldInteractions[interactor] = new List<Interaction>();
                _prospects[interactor] = new List<InteractionProspect>();
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
        
        #region Gathering

        private void GatherHandInteractions()
        {
            foreach (IInteractor interactor in _interactors)
            {
                foreach (IInteractable interactable in _interactablesInHand[interactor])
                {
                    _handInteractions[interactor].AddRange(interactable.GetInteractions());
                }
            }
        }

        private void GatherWorldInteractions()
        {
            foreach (IInteractor interactor in _interactors)
            {
                foreach (IInteractable interactable in _interactablesInWorld[interactor])
                {
                    _worldInteractions[interactor].AddRange(interactable.GetInteractions());
                }
            }
        }

        private void EvaluateTwoWayInteractions()
        {
            
        }
        
        #endregion
        
        #region Processing

        private void CreateProspects()
        {
            foreach (IInteractor interactor in _interactors)
            {
                _prospects[interactor].AddRange(GetHandOnlyProspects(interactor));
                _prospects[interactor].AddRange(GetWorldOnlyProspects(interactor));
                _prospects[interactor].AddRange(GetHandWorldProspects(interactor));
            }
        }

        private void EliminateProspects()
        {
            foreach (IInteractor interactor in _interactors)
            {
                List<InteractionProspect> prospectsToRemove = new List<InteractionProspect>();
        
                foreach (InteractionProspect prospect in _prospects[interactor])
                {
                    if (prospect.WorldInteractable == null) continue; // Skip hand-only interactions
            
                    IInteractable worldInteractable = prospect.WorldInteractable;
            
                    Vector3 interactorPosition = interactor.GetWorldPosition();
                    Vector3 worldPosition = (worldInteractable as Component).transform.position;
                    float distance = Vector3.Distance(interactorPosition, worldPosition);
            
                    bool tooFarAway = distance > prospect.Interaction.InteractionDistance;
                    bool outsideViewCone = !prospect.Interaction.VicinityBased && 
                                           Angle(interactor, worldInteractable) > DetectionAngle * 0.5f;
                                  
                    if (tooFarAway || outsideViewCone)
                    {
                        prospectsToRemove.Add(prospect);
                    }
                }
        
                // Remove all invalid prospects at once
                foreach (var prospect in prospectsToRemove)
                {
                    _prospects[interactor].Remove(prospect);
                }
            }
        }
        
        #endregion
        
        #region Utility
        
        private List<InteractionProspect> GetHandOnlyProspects(IInteractor interactor)
        {
            var handOnlyProspects = _interactablesInHand[interactor]
                .SelectMany(handInteractable => handInteractable.GetInteractions()
                    .Where(interaction => interaction.RequiresInHand && !interaction.RequiresInWorld)
                    .Select(interaction => new InteractionProspect(interaction, hand: handInteractable)))
                .ToList();
            return handOnlyProspects;
        }

        private List<InteractionProspect> GetHandWorldProspects(IInteractor interactor)
        {
            var handWorldProspects = _interactablesInHand[interactor]
                .SelectMany(handInteractable => handInteractable.GetInteractions()
                    .Where(interaction => interaction.RequiresInHand && interaction.RequiresInWorld)
                    .SelectMany(interaction => _interactablesInWorld[interactor]
                        .Where(worldInteractable => worldInteractable.GetInteractions()
                            .Any(worldInteraction => worldInteraction.Name == interaction.Name))
                        .Select(worldInteractable => new InteractionProspect(
                            interaction, 
                            hand: handInteractable, 
                            world: worldInteractable))))
                .ToList();

            handWorldProspects.RemoveAll(prospect => {
                IReceiver receiver = prospect.WorldInteractable as IReceiver;
                return receiver == null || !receiver.CanInteractWith(prospect.Interaction, prospect.HandInteractable);
            });
            
            return handWorldProspects;
        }

        private List<InteractionProspect> GetWorldOnlyProspects(IInteractor interactor)
        {
            var worldOnlyProspects = _interactablesInWorld[interactor]
                .SelectMany(worldInteractable => worldInteractable.GetInteractions()
                    .Where(interaction => interaction.RequiresInWorld && !interaction.RequiresInHand)
                    .Select(interaction => new InteractionProspect(
                        interaction, 
                        hand: null, // No hand interactable for world-only interactions
                        world: worldInteractable)))
                .ToList();
            return worldOnlyProspects;
        }

        private float Angle(IInteractor interactor, IInteractable interactable)
        {
            Vector3 interactorPosition = interactor.GetWorldPosition();
            Vector3 interactablePosition = (interactable as Component).transform.position;
            
            Vector3 interactorDirection = interactor.GetLookDirection();
            Vector3 interactableDirection = interactablePosition - interactorPosition;
            
            return Vector3.Angle(interactorDirection, interactableDirection);
        }
        
        #endregion
        
        #endregion
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

    public struct NewInteractionContext
    {
        public NewInteractionContext(Interactor interactor, Interaction interaction, 
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
        public Interactor Interactor;
        public IInteractable HandInteractable;
        public IInteractable WorldInteractable;
        public RaycastHit Hit;
    }
}