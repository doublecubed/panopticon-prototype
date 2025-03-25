using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCameraPlacer : MonoBehaviour
{
    #region REFERENCES

    [Header("Prefabs")]
    [SerializeField] private GameObject _cameraPrefab;
    
    [Header("References")]
    [SerializeField] private Image _placementIndicator;
    private Camera _playerCam;
    
    #endregion
    
    #region VARIABLES

    [SerializeField] private float _maxPlacementDistance;
    
    private Vector3 _screenCenter;
    
    #endregion
    
    #region MONOBEHAVIOUR
    
    private void Start()
    {
        _playerCam = Camera.main;
        CalculateScreenCenter();

    }

    private void Update()
    {
        CastCameraRay();
    }
    
    #endregion
    
    #region METHODS

    private void CalculateScreenCenter()
    {
        _screenCenter = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
    }
    
    private void CastCameraRay()
    {
        Ray screenRay = _playerCam.ScreenPointToRay(_screenCenter);
        if (Physics.Raycast(screenRay, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Ground") && RayHitDistance(hit.point) <= _maxPlacementDistance)
            {
                UpdatePlacementIndicator(true);
            }
            else
            {
                UpdatePlacementIndicator(false);
            }
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
}
