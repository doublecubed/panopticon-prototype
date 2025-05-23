using System;
using System.Collections.Generic;
using System.Linq;
using DeificGames.Profiler;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

namespace InteractionSystem
{
    public class InteractionSolver : MonoBehaviour
    {
        // Temporary Reference
        public TMP_Text OutputText;
        
        #region REFERENCES
        
        private List<IInteractor> _interactors;
        
        private Dictionary<IInteractor, List<IInteractable>> _interactablesInHand;
        private Dictionary<IInteractor, List<IInteractable>> _interactablesInWorld;

        private Dictionary<IInteractor, List<Interaction>> _handInteractions;
        private Dictionary<IInteractor, List<Interaction>> _worldInteractions;

        private Dictionary<IInteractor, List<InteractionProspect>> _prospects;

        private Dictionary<IInteractor, Dictionary<InteractionCategory, List<InteractionProspect>>>
            _categorizedProspects;

        public Dictionary<IInteractor, Dictionary<InteractionCategory, InteractionContext>> CurrentContexts
        {
            get;
            private set;
        }

        public Dictionary<IInteractor, InteractionSet> InteractionSets;
        
        #endregion
        
        #region VARIABLES
        
        [field: SerializeField] public float MaxDetectionDistance { get; private set; }
        [field: SerializeField] public float DetectionAngle { get; private set; }

        private const int HandWorldPriority = 30;
        private const int HandOnlyPriority = -30;
        private const int WorldOnlyPriority = 10;
        private const int RaycastPriority = 40;
        private const int SpherecastPriority = 30;
        private const int VicinityPriority = 5;
        
        #endregion
        
        #region MONOBEHAVIOUR

        private void Awake()
        {
            InitializeDictionaries();
        }

        private void Update()
        {
            DGProfiler.BeginScope(this, "Performance");
            
            ResetDictionaries();
            GetInteractablesInHand();
            DetectInteractablesInWorld();
            RemoveHandInteractablesFromWorldList();
            
            GatherHandInteractions();
            GatherWorldInteractions();
            
            CreateProspects();
            EliminateProspects();
            CategorizeProspects();
            OrderProspectsForPriority();
            
            CreateInteractionSet();
            
            DGProfiler.EndScope();
            
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
            
            _categorizedProspects = new Dictionary<IInteractor, Dictionary<InteractionCategory, List<InteractionProspect>>>();

            InteractionSets = new Dictionary<IInteractor, InteractionSet>();
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
                
                var categorized = new Dictionary<InteractionCategory, List<InteractionProspect>>();
                foreach (InteractionCategory category in Enum.GetValues(typeof(InteractionCategory)))
                {
                    categorized[category] = new List<InteractionProspect>();
                }
                _categorizedProspects[interactor] = categorized;
                
                InteractionSets[interactor] = new InteractionSet(); 
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

        private void CategorizeProspects()
        {
            foreach (IInteractor interactor in _interactors)
            {
                foreach (InteractionCategory category in Enum.GetValues(typeof(InteractionCategory)))
                {
                    _categorizedProspects[interactor][category] = 
                        _prospects[interactor].Where(prospect => prospect.Interaction.Category == category).ToList();
                }
            }
        }

        // TODO: sorting can be done using enums and dictionaries, but handworld, handonly and worldonly are not types yet.
        private void OrderProspectsForPriority()
        {
            foreach (IInteractor interactor in _interactors)
            {
                foreach (InteractionCategory category in Enum.GetValues(typeof(InteractionCategory)))
                {
                    _categorizedProspects[interactor][category] = _categorizedProspects[interactor][category].OrderBy(x => InteractionPriority(x.Interaction)).ToList();
                }
            }
        }
        
        #endregion
        
        #region Casting

        private void CastForProspects()
        {
            foreach (IInteractor interactor in _interactors)
            {
                InteractionSet interactionSet = new InteractionSet();
                
                foreach (InteractionCategory category in Enum.GetValues(typeof(InteractionCategory)))
                {
                    
                        
                    List<InteractionProspect> prospectList = _categorizedProspects[interactor][category];
                    
                    if (prospectList.Count == 0) continue;
                    for (int i = 0; i < prospectList.Count; i++)
                    {
                        Interaction interaction = prospectList[i].Interaction;

                        interactionSet.InteractionContexts[category] = new InteractionContext();
                        
                        switch (interaction.Targeting)
                        {
                            case InteractionTargeting.Raycast:
                                if (RaycastForProspect(interactor, prospectList[i], out RaycastHit raycastHit))
                                {
                                    InteractionContext raycastContext = new InteractionContext(interactor,
                                        interaction,
                                        prospectList[i].HandInteractable, prospectList[i].WorldInteractable,
                                        raycastHit);
                                    interactionSet.InteractionContexts[category] = raycastContext;
                                }
                                break;
                            case InteractionTargeting.Spherecast:
                                if (SpherecastForProspect(interactor, prospectList[i], out RaycastHit spherecastHit))
                                {
                                    InteractionContext spherecastContext = new InteractionContext(interactor,
                                        interaction,
                                        prospectList[i].HandInteractable, prospectList[i].WorldInteractable,
                                        spherecastHit);
                                    interactionSet.InteractionContexts[category] = spherecastContext;
                                }
                                break;
                            case InteractionTargeting.Vicinity:
                                InteractionContext categoryContext = new InteractionContext(interactor,
                                    interaction,
                                    prospectList[i].HandInteractable, prospectList[i].WorldInteractable);
                                interactionSet.InteractionContexts[category] = categoryContext;
                                break;
                        }
                    }
                }

                InteractionSets[interactor] = interactionSet;

                string outputText = "";
                foreach (InteractionCategory category in Enum.GetValues(typeof(InteractionCategory)))
                {
                    outputText += category + ":\n";
                    outputText += "Interaction: " + InteractionSets[interactor].InteractionContexts[category].Interaction.Name +"\n";
                    outputText += "Hand: " + InteractionSets[interactor].InteractionContexts[category].HandInteractable + "\n";
                    outputText += "World: " + InteractionSets[interactor].InteractionContexts[category].WorldInteractable + "\n";
                }
                
                OutputText.text = outputText;
            }
        }

        private void CreateInteractionSet()
        {
            foreach (IInteractor interactor in _interactors)
            {
                InteractionSet interactionSet = new InteractionSet();

                foreach (InteractionCategory category in Enum.GetValues(typeof(InteractionCategory)))
                {
                    List<InteractionProspect> categoryProspects = _categorizedProspects[interactor][category];
                    if (categoryProspects.Count == 0) continue;

                    bool foundValidInteraction = false;

                    for (int i = 0; i < categoryProspects.Count; i++)
                    {
                        Interaction prospectInteraction =  categoryProspects[i].Interaction;

                        if (prospectInteraction.Targeting != InteractionTargeting.Vicinity)
                        {
                            if (CastForProspect(interactor, categoryProspects[i], out RaycastHit castResult))
                            {
                                InteractionContext castContext =
                                    new InteractionContext(interactor, prospectInteraction,
                                        categoryProspects[i].HandInteractable, categoryProspects[i].WorldInteractable,
                                        castResult);
                                interactionSet.InteractionContexts[category] = castContext;
                                foundValidInteraction = true;
                            }
                        }
                        else
                        {
                            InteractionContext vicinityContext =
                                new InteractionContext(interactor, prospectInteraction,
                                    categoryProspects[i].HandInteractable, categoryProspects[i].WorldInteractable);
                            interactionSet.InteractionContexts[category] = vicinityContext;
                            foundValidInteraction = true;
                        }

                        if (foundValidInteraction) break;
                    }
                }

                InteractionSets[interactor] = interactionSet;
                
                
                string outputText = "";
                foreach (InteractionCategory category in Enum.GetValues(typeof(InteractionCategory)))
                {
                    if (InteractionSets[interactor].InteractionContexts[category].Interaction == null) continue;
                    outputText += category + ":\n";
                    outputText += "Interaction: " + InteractionSets[interactor].InteractionContexts[category].Interaction.Name +"\n";
                    outputText += "Hand: " + InteractionSets[interactor].InteractionContexts[category].HandInteractable + "\n";
                    outputText += "World: " + InteractionSets[interactor].InteractionContexts[category].WorldInteractable + "\n";
                }
                
                OutputText.text = outputText;
            }
        }

        // TODO: No-range interaction (like use) should have its own case. This is wacky at best
        private bool CastForProspect(IInteractor interactor, InteractionProspect prospect, out RaycastHit hitResult)
        {
            Interaction interaction = prospect.Interaction;

            hitResult = new RaycastHit();
            
            if (interaction.InteractionDistance <= 0) return true;

            Ray castingRay = interactor.LookRay();
            bool hitSomething = false;
            
            if (interaction.Targeting == InteractionTargeting.Raycast)
                hitSomething = Physics.Raycast(castingRay, out hitResult, MaxDetectionDistance);

            else if (interaction.Targeting == InteractionTargeting.Spherecast)
                hitSomething = Physics.SphereCast(castingRay, interaction.InteractionRadius, out hitResult, MaxDetectionDistance);

            if (!hitSomething) return false;
            
            if (hitResult.distance > interaction.InteractionDistance) return false;
            
            if (!interaction.RequiresInWorld) return true;
            
            Component worldComponent = prospect.WorldInteractable as Component;
            if (worldComponent != null && worldComponent.gameObject == hitResult.transform.gameObject)
                return true;
            
            return false;
        }
        
        
        
       
        private bool RaycastForProspect(IInteractor interactor, InteractionProspect prospect, out RaycastHit hitResult)
        {
            Interaction interaction = prospect.Interaction;

            // for the negative results.
            hitResult = new RaycastHit();
            
            // This is only possible for self-use items in hand. Not an elegant solution. Must change
            if (interaction.InteractionDistance <= 0) return true;
            
            Ray castingRay = interactor.LookRay();
            if (Physics.Raycast(castingRay, out RaycastHit hit, MaxDetectionDistance))
            {
                hitResult = hit;
                if (hit.distance > interaction.InteractionDistance) return false;

                if (!interaction.RequiresInWorld) return true;

                Component worldComponent = prospect.WorldInteractable as Component;
                if (worldComponent != null && worldComponent.gameObject == hit.transform.gameObject)
                    return true;
            }
            
            return false;
        }

        // Practically same code as raycast. They can be combined
        private bool SpherecastForProspect(IInteractor interactor, InteractionProspect prospect,
            out RaycastHit hitResult)
        {
            Interaction interaction = prospect.Interaction;

            // for the negative results.
            hitResult = new RaycastHit();
            
            // This is only possible for self-use items in hand. Not an elegant solution. Must change
            if (interaction.InteractionDistance <= 0) return true;
            
            Ray castingRay = interactor.LookRay();
            if (Physics.SphereCast(castingRay, interaction.InteractionRadius, out RaycastHit hit, MaxDetectionDistance))
            {
                hitResult = hit;
                if (hit.distance > interaction.InteractionDistance) return false;

                if (!interaction.RequiresInWorld) return true;

                Component worldComponent = prospect.WorldInteractable as Component;
                if (worldComponent != null && worldComponent.gameObject == hit.transform.gameObject)
                    return true;
            }
            
            return false;
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

        private int InteractionPriority(Interaction interaction)
        {
            int typePriority = 0;
            if (interaction.RequiresInHand && interaction.RequiresInWorld)
                typePriority = HandWorldPriority;
            if (interaction.RequiresInHand && !interaction.RequiresInWorld)
                typePriority = HandOnlyPriority;
            if (!interaction.RequiresInHand && interaction.RequiresInWorld)
                typePriority = WorldOnlyPriority;

            int castPriority = 0;
            switch (interaction.Targeting)
            {
                case InteractionTargeting.Raycast:
                    castPriority = RaycastPriority;
                    break;
                case InteractionTargeting.Spherecast:
                    castPriority = SpherecastPriority;
                    break;
                case InteractionTargeting.Vicinity:
                    castPriority = VicinityPriority;
                    break;
            }
            
            return typePriority + castPriority;
        }
        
        #endregion
        
        #endregion
    }
}