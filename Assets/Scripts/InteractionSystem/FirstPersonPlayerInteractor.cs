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
        private IInteractablePrimary _currentInteractablePrimary;
        private IInteractableSecondary _currentInteractableSecondary;
        private Transform _currentInteractableTransform;
        private InteractionContext _currentInteractionContext;
        #endregion
        
        #region Inventory

        private PlayerInventory _playerInventory;
        private IInventoryItem _currentInventoryItem;
        
        #endregion
        
        #region Processing
        
        private Dictionary<int, List<InteractionSet>> _interactionMap;
        
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
            RaycastFromCamera();
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
        
        private void RaycastFromCamera()
        {
            Ray cameraRay = _playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            ResetCurrentInteractable();

            if (!Physics.Raycast(cameraRay, out RaycastHit hit, _interactionRange)) 
                return;
            
            if (!hit.transform.TryGetComponent(out IInteractablePrimary interactable) || interactable == null) 
                return;

            if (DistanceToInteractable(hit.transform) > _interactionRange)
                return;
            
            SetCurrentPrimaryInteractable(interactable, hit.transform);
            SetInteractionContext(hit);
            
            hit.transform.TryGetComponent(out IInteractableSecondary secondaryInteractable);
            SetCurrentSecondaryInteractable(secondaryInteractable, hit.transform);
            
            SetInteractableTexts();
        }

        private void DetectWorldInteractables()
        {
            
        }
        
        private void SetCurrentSecondaryInteractable(IInteractableSecondary secondaryInteractable, Transform transform)
        {
            _currentInteractableSecondary = secondaryInteractable;
            ToggleInput(_interact2, true);
        }
        
        private void SetCurrentPrimaryInteractable(IInteractablePrimary interactablePrimary, Transform transform)
        {
            _currentInteractablePrimary = interactablePrimary;
            _currentInteractableTransform = transform;
            ToggleInput(_interact, true);
        }


        
        private void ResetCurrentInteractable()
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
        }
        
        #endregion
        
        #region Interaction Processing

        private void SetInteractionContext(RaycastHit hit)
        {
            // PROBLEM
            InteractionContext context = new InteractionContext(this, hit, _playerCam, InteractionType.None);
        }
        
        private void SetInteractableTexts()
        {
            _interactableNameText.text = _currentInteractablePrimary != null ? _currentInteractablePrimary.GetInfoPrimary(_currentInteractionContext).Name : "";
            _interactablePromptText.text = _currentInteractablePrimary != null ? _currentInteractablePrimary.GetInfoPrimary(_currentInteractionContext).Prompt : "";
            _secondaryInteractablePromptText.text = _currentInteractableSecondary != null
                ? _currentInteractableSecondary.GetInfoSecondary(_currentInteractionContext).Prompt
                : "";
        }
        
        private void ProcessInteractions()
        {
            
        }
        
        #endregion
        
        #region Interaction Execution
        
        private void Interact(InputAction.CallbackContext context)
        {
            _currentInteractablePrimary?.InteractPrimary(_currentInteractionContext);
        }

        private void SecondaryInteract(InputAction.CallbackContext context)
        {
            _currentInteractableSecondary?.InteractSecondary(_currentInteractionContext);
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