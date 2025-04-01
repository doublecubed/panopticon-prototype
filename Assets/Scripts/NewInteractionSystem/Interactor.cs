using System;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace NewInteractionSystem
{
    public class Interactor : MonoBehaviour
    {
        #region REFERENCES

        #region Self Components
        [SerializeField] private Camera _playerCam;
        private PlayerInventory _inventory;
        private InteractionExecuter _executer;
        #endregion
        
        #region Input
        
        private InputController _inputController;
        private InputAction _interactPrimary;
        private InputAction _interactSecondary;
        
        #endregion
        
        #region Interactable Info
        
        private IInteractable _inventoryInteractable;
        private IInteractable _worldInteractable;
        
        [SerializeField] private List<InteractableType> _inventoryInteractableTypes;
        [SerializeField] private List<InteractableType> _worldInteractableTypes;

        [SerializeField] private InteractionType _primaryInteraction;
        [SerializeField] private InteractionType _secondaryInteraction;

        private RaycastHit _hit;
        
        public InteractionContext CurrentInteractionContext { get; private set; }
        
        #endregion

        #region View Info

        [SerializeField] private TMP_Text _interactableNameText;
        [SerializeField] private TMP_Text _primaryInteractionPromptText;
        [SerializeField] private TMP_Text _secondaryInteractionPromptText;
        
        #endregion
        
        #endregion
        
        #region VARIABLES

        [SerializeField] private float _interactionDistance;
        private bool _inputEnabled;
        
        #endregion
        
        #region MONOBEHAVIOUR

        private void Awake()
        {
            _inventory = GetComponent<PlayerInventory>();
            _executer = GetComponent<InteractionExecuter>();
        }

        private void Start()
        {
            InitializeControls();
        }

        private void Update()
        {
            ResetInteractables();
            DetectInventoryInteractables();
            DetectWorldInteractables();
            DetectPrimaryInteraction();
            DetectSecondaryInteraction();
            PrepareInteractionContext();
            
            ActivateInteractionInput();
            
            DressInteractionView();
        }

        #endregion
        
        #region METHODS

        #region Initialization

        private void InitializeControls()
        {
            _inputController = InputController.Instance;
            List<InputAction> actions = _inputController.GetInteractionActions();
            _interactPrimary = actions[0];
            _interactPrimary.performed += InteractPrimary;
            _interactSecondary = actions[1];
            _interactSecondary.performed += InteractSecondary;
        }
        
        #endregion
        
        #region Interaction Processing
        private void ResetInteractables()
        {
            _inventoryInteractable = null;
            _worldInteractable = null;
            
            _inventoryInteractableTypes = new List<InteractableType>();
            _worldInteractableTypes = new List<InteractableType>();
            
            _primaryInteraction = InteractionType.None;
            _secondaryInteraction = InteractionType.None;
        }
        
        private void DetectInventoryInteractables()
        {
            IInventoryItem currentItem = _inventory.CurrentInventoryItem;
           
            _inventoryInteractable = currentItem as IInteractable;
            
            if (currentItem is IDropable) _inventoryInteractableTypes.Add(InteractableType.Dropable);
            if (currentItem is IAttachable) _inventoryInteractableTypes.Add(InteractableType.Attachable);
            if (currentItem is IUseable) _inventoryInteractableTypes.Add(InteractableType.Useable);
            if (currentItem is IAppliable) _inventoryInteractableTypes.Add(InteractableType.Appliable);
        }

        private void DetectWorldInteractables()
        {
            Ray cameraRay = _playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Physics.Raycast(cameraRay, out RaycastHit hit, Mathf.Infinity);
            _hit = hit;
            if (_hit.transform == null) return;
            
            if (!hit.transform.TryGetComponent(out IInteractable primary)) return;
            _worldInteractable = primary;
            
            if (hit.transform.TryGetComponent(out IPickupable pickupable)) 
                _worldInteractableTypes.Add(InteractableType.Pickupable);
            if (hit.transform.TryGetComponent(out ISocket socket))
                _worldInteractableTypes.Add(InteractableType.Socket);
            if (hit.transform.TryGetComponent(out IActivatable activatable))
                _worldInteractableTypes.Add(InteractableType.Activatable);
            if (hit.transform.TryGetComponent(out IReceiving receiving))
                _worldInteractableTypes.Add(InteractableType.Receiving);
        }

        private void DetectPrimaryInteraction()
        {
            _primaryInteraction = _hit.distance > _interactionDistance ?
                InteractionType.None :
                InteractionHelper.CanInteractPrimary(_inventoryInteractableTypes, _worldInteractableTypes);
        }

        private void DetectSecondaryInteraction()
        {
            _secondaryInteraction = _hit.distance > _interactionDistance ?
                InteractionType.None :
                InteractionHelper.CanInteractSecondary(_inventoryInteractableTypes, _worldInteractableTypes);
        }

        private void PrepareInteractionContext()
        {
            CurrentInteractionContext = new InteractionContext(this,
                _inventoryInteractable, _worldInteractable,
                _primaryInteraction, _secondaryInteraction,
                _inventoryInteractableTypes, _worldInteractableTypes,
                _hit, _playerCam);
        }
        #endregion
        
        #region Interaction HUD
        
        private void DressInteractionView()
        {
            string inventoryName = _inventoryInteractable == null ? "" : _inventoryInteractable.GetItemName();
            string intermediary = (_inventoryInteractable == null || _worldInteractable == null) ? "" : "-";
            string worldName =  _worldInteractable == null ? "" : _worldInteractable.GetItemName();
            
            _interactableNameText.text = inventoryName + intermediary + worldName;

            _primaryInteractionPromptText.text =
                _primaryInteraction == InteractionType.None ? "" : _primaryInteraction.ToString();
            
            _secondaryInteractionPromptText.text =
                _secondaryInteraction == InteractionType.None ? "" : _secondaryInteraction.ToString();
        }

        #endregion
        
        #region Input
        
        private void ActivateInteractionInput()
        {
            bool shouldBeEnabled = CurrentInteractionContext.PrimaryInteraction != InteractionType.None 
                                   || CurrentInteractionContext.SecondaryInteraction != InteractionType.None;
    
            if (_inputEnabled != shouldBeEnabled)
            {
                _inputEnabled = shouldBeEnabled;
        
                if (shouldBeEnabled)
                    _inputController.EnableInteractionControl();
                else
                    _inputController.DisableInteractionControl();
            }
        }

        private void InteractPrimary(InputAction.CallbackContext context)
        {
            _executer.ExecutePrimaryInteraction(CurrentInteractionContext);
        }

        private void InteractSecondary(InputAction.CallbackContext context)
        {
            _executer.ExecuteSecondaryInteraction(CurrentInteractionContext);
        }
        
        #endregion
        
        #endregion
    }
}
