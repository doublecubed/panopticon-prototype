using System;
using InventorySystem;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace InteractionSystem
{
    public class FirstPersonPlayerInteractor : MonoBehaviour
    {
        #region REFERENCES

        #region Input
        private InputController _interactionControl;
        private InputAction _interact;
        private InputAction _interact2;
        #endregion

        #region Raycast
        [SerializeField] private Camera _playerCam;
        private IInteractablePrimary _currentWorldInteractablePrimary;
        private IInteractableSecondary _currentWorldInteractableSecondary;
        private Transform _currentWorldInteractableTransform;
        private RaycastHit _currentHit;

        #endregion
        
        #region Inventory

        private PlayerInventory _playerInventory;
        private IInventoryItem _currentInventoryItem;
        private IInteractablePrimary _currentInventoryInteractablePrimary;
        private IInteractableSecondary _currentInventoryInteractableSecondary;
        
        #endregion
        
        #region Processing
        
        private Dictionary<int, List<InteractionSet>> _interactionMap;
        private InteractionContext _currentInteractionContext;
        
        #endregion
        
        #region View
        
        [SerializeField] private TextMeshProUGUI _interactableNameText;
        [SerializeField] private TextMeshProUGUI _interactablePromptText;
        [SerializeField] private TextMeshProUGUI _secondaryInteractablePromptText;
        
        #endregion
        
        #endregion
        
        #region VARIABLES

        [SerializeField] private float _interactionRange;

        private bool _hasInventoryPrimary;
        private bool _hasInventorySecondary;
        private bool _hasWorldPrimary;
        private bool _hasWorldSecondary;
        
        #endregion
        
        #region MONOBEHAVIOUR

        private void Awake()
        {
            _playerInventory = GetComponent<PlayerInventory>();
        }

        private void Start()
        {
            CreateInteractionMap();
            ReceiveInputActions();
        }
        
        private void Update()
        {
            DetectWorldInteractables();
            GetCurrentInventoryItem();
        }

        #endregion
        
        #region METHODS

        #region Initialization

        private void CreateInteractionMap()
        {
            _interactionMap = InteractionUtility.CreateInteractionMap();
        }
        
        private void ReceiveInputActions()
        {
            _interactionControl = InputController.Instance; 
            List<InputAction> actions = _interactionControl.GetInteractionActions();
            _interact = actions[0];
            _interact.performed += Interact;
            _interact2 = actions[1];
            _interact2.performed += SecondaryInteract;
        }
        
        #endregion
        
        #region Interactable Detection
        
        private void DetectWorldInteractables()
        {
            Ray cameraRay = _playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            ResetCurrentWorldInteractables();

            if (!Physics.Raycast(cameraRay, out RaycastHit hit, _interactionRange)) 
                return;
            
            if (!hit.transform.TryGetComponent(out IInteractablePrimary primary)
                || !hit.transform.TryGetComponent(out IInteractableSecondary secondary)) return;
            
            if (DistanceToInteractable(hit.transform) > _interactionRange)
                return;

            if (primary != null) SetCurrentPrimaryInteractable(primary, hit.transform);
            if (secondary != null) SetCurrentSecondaryInteractable(secondary, hit.transform);
            
            _currentHit = hit;
        }
        
        private void SetCurrentSecondaryInteractable(IInteractableSecondary secondaryInteractable, Transform transform)
        {
            _currentWorldInteractableSecondary = secondaryInteractable;
            _currentWorldInteractableTransform = transform;
            ToggleInput(_interact2, true);
        }
        
        private void SetCurrentPrimaryInteractable(IInteractablePrimary interactablePrimary, Transform transform)
        {
            _currentWorldInteractablePrimary = interactablePrimary;
            _currentWorldInteractableTransform = transform;
            ToggleInput(_interact, true);
        }

        private void ResetCurrentWorldInteractables()
        {
            ToggleInput(_interact, false);
            ToggleInput(_interact2, false);
            SetCurrentPrimaryInteractable(null, null);
            SetCurrentSecondaryInteractable(null, null);
            SetInteractableTexts();
        }

       
        private float DistanceToInteractable(Transform interactable)
        {
            return Vector3.Distance(interactable.position, transform.position);
        }

        #endregion
        
        #region Inventory Detection

        private void GetCurrentInventoryItem()
        {
            _currentInventoryItem = _playerInventory.CurrentInventoryItem;
            if (_currentInventoryItem is IInteractablePrimary)
                _currentInventoryInteractablePrimary = _currentInventoryItem as IInteractablePrimary;
            if (_currentInventoryItem is IInteractableSecondary)
                _currentInventoryInteractableSecondary = _currentInventoryItem as IInteractableSecondary;
        }

        private void ResetCurrentInventoryInteractables()
        {
            _currentInventoryItem = null;
            _currentInventoryInteractablePrimary = null;
            _currentInventoryInteractableSecondary = null;
        }
        
        #endregion
        
        #region Interaction Processing

        private void SetInteractionContext(RaycastHit hit, List<InteractionSet> interactionSets)
        {
            _currentInteractionContext = new InteractionContext(this, hit, _playerCam,interactionSets);
        }
        
        private void SetInteractableTexts()
        {
            _interactableNameText.text = _currentWorldInteractablePrimary != null ? _currentWorldInteractablePrimary.GetInfoPrimary(_currentInteractionContext).Name : "";
            _interactablePromptText.text = _currentWorldInteractablePrimary != null ? _currentWorldInteractablePrimary.GetInfoPrimary(_currentInteractionContext).Prompt : "";
            _secondaryInteractablePromptText.text = _currentWorldInteractableSecondary != null
                ? _currentWorldInteractableSecondary.GetInfoSecondary(_currentInteractionContext).Prompt
                : "";
        }
        
        private void ProcessInteractions()
        {
            bool worldPrimary = _currentWorldInteractablePrimary != null;
            bool worldSecondary = _currentWorldInteractableSecondary != null;
            bool inventoryPrimary = _currentWorldInteractablePrimary != null;
            bool inventorySecondary = _currentWorldInteractableSecondary != null;
           
            int interactionCaseIndex = InteractionUtility.InteractionInteger(worldPrimary, worldSecondary, inventoryPrimary, inventorySecondary);
            
            List<InteractionSet> interactionSets = _interactionMap[interactionCaseIndex];
            
            SetInteractionContext(_currentHit, interactionSets);
        }
        
        #endregion
        
        #region Interaction Execution
        
        private void Interact(InputAction.CallbackContext context)
        {
            _currentWorldInteractablePrimary?.InteractPrimary(_currentInteractionContext);
        }

        private void SecondaryInteract(InputAction.CallbackContext context)
        {
            _currentWorldInteractableSecondary?.InteractSecondary(_currentInteractionContext);
        }

        private void ToggleInput(InputAction action, bool toggle)
        {
            if (toggle) _interactionControl.EnableInteractionControl();
            else _interactionControl.DisableInteractionControl();;

            //if (toggle) action.Enable();
            //else action.Disable();
        }
        
        #endregion
        
        #endregion
    }
}