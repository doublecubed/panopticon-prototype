using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerCameraPlacer : MonoBehaviour
{
    #region REFERENCES

    [Header("Prefabs")]
    [SerializeField] private GameObject _cameraPrefab;
    [SerializeField] private GameObject _cameraGhostPrefab;
    
    [Header("References")]
    [SerializeField] private Image _placementIndicator;
    [SerializeField] private CameraCentral _cameraCentral;
    [SerializeField] private CameraPreviewController _cameraPreviewController;
    private Camera _playerCam;

    // Input Action References
    private InputAction _startPlacement;
    private InputAction _finalizePlacement;
    
    // Camera ghost instance reference
    private Transform _ghostCameraInstance;


    
    #endregion
    
    #region VARIABLES

    [SerializeField] private float _maxPlacementDistance;
    
    private Vector3 _screenCenter;

    // Ghost camera spawning variables
    private bool _canSpawnGhost;
    private bool _ghostCameraSpawned;
    private bool _canPlacePortable;
    private Vector3 _lastRaycastHitPosition;
    private float _ghostCameraOffset;

    //TODO: Convert these from constant variables to another good practice.
    private static readonly Vector3 GhostCameraColliderCenter = new Vector3(0f, 0.65f, 0f);
    private static readonly Vector3 GhostCameraColliderSize = new Vector3(0.3f, 1.3f, 0.5f);
    
    #endregion
    
    #region MONOBEHAVIOUR
    
    private void Start()
    {
        _playerCam = Camera.main;
        CalculateScreenCenter();
        SubscribeToInputs();
    }
    private void Update()
    {
        CastCameraRay();
    }

    private void LateUpdate()
    {
        UpdateGhostCameraPosition();
        CheckGhostCameraPlacement();
    }
    
    #endregion
    
    #region METHODS

    #region Camera Raycast
    private void CalculateScreenCenter()
    {
        _screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
    }
    
    private void CastCameraRay()
    {
        Ray screenRay = _playerCam.ScreenPointToRay(_screenCenter);
        if (Physics.Raycast(screenRay, out RaycastHit hit))
        {
            _lastRaycastHitPosition = hit.point;
            _canSpawnGhost = hit.collider.CompareTag("Ground") && RayHitDistance(hit.point) <= _maxPlacementDistance; 
            UpdatePlacementIndicator(_canSpawnGhost);
        }
        else
        {
            _lastRaycastHitPosition = transform.position + Vector3.up * (_maxPlacementDistance * 2);
        }
        
    }

    private void UpdatePlacementIndicator(bool placeable)
    {
        _placementIndicator.color = placeable ? Color.green : Color.red;
    }

    private float RayHitDistance(Vector3 hitPoint)
    {
        return Vector3.Distance(hitPoint, transform.position);
    }
    #endregion
    
    #region Input

    private void SubscribeToInputs()
    {
        _startPlacement = InputSystem.actions.FindAction("Interact");
        _finalizePlacement = InputSystem.actions.FindAction("Attack");

        _startPlacement.performed += PlaceKeyPressed;
        _finalizePlacement.performed += FinalizeKeyPressed;
    }
    
    private void PlaceKeyPressed(InputAction.CallbackContext obj)
    {
        if (_ghostCameraSpawned) DespawnGhostCamera();
        else SpawnGhostCamera();
    }

    private void FinalizeKeyPressed(InputAction.CallbackContext obj)
    {
        PlacePortableCamera();
    }
    
    #endregion
    
    #region Camera Manipulation

    private void SpawnGhostCamera()
    {
        if (_canSpawnGhost && !_ghostCameraSpawned)
        {
            _ghostCameraSpawned = true;
            Vector3 projection = Vector3.ProjectOnPlane(_playerCam.transform.forward, Vector3.up);
            GameObject cameraGhost = Instantiate(_cameraGhostPrefab, _lastRaycastHitPosition, Quaternion.LookRotation(projection));
            _ghostCameraInstance = cameraGhost.transform;
            
            _ghostCameraOffset = (_ghostCameraInstance.position - transform.position).magnitude;
        }
    }

    private void DespawnGhostCamera()
    {
        Destroy(_ghostCameraInstance.gameObject);
        _ghostCameraInstance = null;
        _ghostCameraSpawned = false;
    }
    
    private void UpdateGhostCameraPosition()
    {
        if (_ghostCameraInstance == null) return;
        
        _ghostCameraInstance.position = transform.position + transform.forward * _ghostCameraOffset;
        _ghostCameraInstance.rotation = transform.rotation;
    }

    private void CheckGhostCameraPlacement()
    {
        if (_ghostCameraInstance == null) return;
        
        Collider[] overlapColliders = Physics.OverlapBox(_ghostCameraInstance.position + GhostCameraColliderCenter,
            GhostCameraColliderSize * 0.45f, _ghostCameraInstance.rotation);

        _canPlacePortable = GhostCameraIsGrounded() && overlapColliders.Length <= 0;
        
        _ghostCameraInstance.GetComponent<PortableCameraGhost>().RenderGhostCameraPlaceability(_canPlacePortable);
    }

    private bool GhostCameraIsGrounded()
    {
        Ray groundRay = new Ray(_ghostCameraInstance.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(groundRay, out RaycastHit hit, 0.2f))
        {
            return hit.collider.CompareTag("Ground");
        }

        return false;
    }

    private void PlacePortableCamera()
    {
        if (!_ghostCameraSpawned) return;
        if (!_canPlacePortable) return;

        Vector3 ghostPosition = _ghostCameraInstance.position;
        Quaternion ghostRotation = _ghostCameraInstance.rotation;
        
        DespawnGhostCamera();
        
        GameObject portableCamera = Instantiate(_cameraPrefab, ghostPosition, ghostRotation);
        _cameraCentral.AddCamera(portableCamera.GetComponent<PortableCamera>());
    }
    
    #endregion
    
    #endregion
}
