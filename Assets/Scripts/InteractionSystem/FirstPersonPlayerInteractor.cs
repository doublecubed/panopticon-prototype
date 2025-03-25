using System;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace InteractionSystem
{
    public class FirstPersonPlayerInteractor : MonoBehaviour
    {
        #region REFERENCES

        [SerializeField] private InputActionAsset _inputAsset;
        private InputAction _interact;
        private InputAction _interact2;
        
        [SerializeField] private Camera _playerCam;

        private IInteractable _currentInteractable;
        private IInteractableSecondary _currentSecondaryInteractable;
        private Transform _currentInteractableTransform;
        private InteractionContext _currentInteractionContext;
        
        [SerializeField] private TextMeshProUGUI _interactableNameText;
        [SerializeField] private TextMeshProUGUI _interactablePromptText;
        [SerializeField] private TextMeshProUGUI _secondaryInteractablePromptText;
        
        #endregion
        
        #region VARIABLES

        [SerializeField] private float _interactionRange;
        
        #endregion
        
        #region MONOBEHAVIOUR

        private void Start()
        {
            InputActionMap map = _inputAsset.FindActionMap("Interaction");
            _interact = map.FindAction("PrimaryInteract");
            _interact.performed += Interact;
            _interact2 = map.FindAction("SecondaryInteract");
            _interact2.performed += SecondaryInteract;
        }
        
        private void Update()
        {
            RaycastFromCamera();
        }

        #endregion
        
        #region METHODS

        private void RaycastFromCamera()
        {
            Ray cameraRay = _playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            ResetCurrentInteractable();

            if (!Physics.Raycast(cameraRay, out RaycastHit hit, _interactionRange)) 
                return;
            
            if (!hit.transform.TryGetComponent(out IInteractable interactable) || interactable == null) 
                return;

            if (DistanceToInteractable(hit.transform) > _interactionRange)
                return;
            
            
            SetCurrentInteractable(interactable, hit.transform);
            SetInteractionContext(hit);
            
            hit.transform.TryGetComponent(out IInteractableSecondary secondaryInteractable);
            SetCurrentSecondaryInteractable(secondaryInteractable);
            
            SetInteractableTexts();
        }

        private void SetCurrentSecondaryInteractable(IInteractableSecondary secondaryInteractable)
        {
            _currentSecondaryInteractable = secondaryInteractable;
            ToggleInput(_interact2, true);
        }
        
        private void SetCurrentInteractable(IInteractable interactable, Transform transform)
        {
            _currentInteractable = interactable;
            _currentInteractableTransform = transform;
            ToggleInput(_interact, true);
        }

        private void SetInteractionContext(RaycastHit hit)
        {
            InteractionContext context = new InteractionContext(this, hit, _playerCam);
        }
        
        private void ResetCurrentInteractable()
        {
            ToggleInput(_interact, false);
            ToggleInput(_interact2, false);
            SetCurrentInteractable(null, null);
            SetCurrentSecondaryInteractable(null);
            SetInteractableTexts();
        }

        private void SetInteractableTexts()
        {
            _interactableNameText.text = _currentInteractable != null ? _currentInteractable.GetName() : "";
            _interactablePromptText.text = _currentInteractable != null ? _currentInteractable.InteractionPrompt() : "";
            _secondaryInteractablePromptText.text = _currentSecondaryInteractable != null
                ? _currentSecondaryInteractable.SecondaryInteractionPrompt()
                : "";
        }
        
        private float DistanceToInteractable(Transform interactable)
        {
            return Vector3.Distance(interactable.position, transform.position);
        }

        private void Interact(InputAction.CallbackContext context)
        {
            _currentInteractable?.Interact(_currentInteractionContext);
        }

        private void SecondaryInteract(InputAction.CallbackContext context)
        {
            _currentSecondaryInteractable?.InteractSecondary(_currentInteractionContext);
        }

        private void ToggleInput(InputAction action, bool toggle)
        {
            if (toggle) action.Enable();
            else action.Disable();
        }
        
        #endregion
    }
}