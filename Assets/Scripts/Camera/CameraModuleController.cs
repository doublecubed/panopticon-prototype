using System;
using UnityEngine;

public class CameraModuleController : MonoBehaviour
{
    #region REFERENCES
    
    [SerializeField] private Transform _batteryPoint;
    [SerializeField] private Transform _lensPoint;
    [SerializeField] private Transform _module1Point;
    [SerializeField] private Transform _module2Point;
    
    [field: SerializeField] public CameraBattery Battery { get; private set; }
    public CameraModule Lens { get; private set; }
    public CameraModule Module1 { get; private set; }
    public CameraModule Module2 { get; private set; }

    #endregion
    
    #region MONOBEHAVIOUR

    private void Awake()
    {
        CheckForBattery();
    }


    #endregion
    
    #region METHODS

    private void CheckForBattery()
    {
        if (GetComponentInChildren<CameraBattery>() != null)
        {
            Battery = GetComponentInChildren<CameraBattery>();
        }
    }
    
    public bool InsertBattery(CameraBattery newBattery)
    {
        if (Battery == null)
        {
            Battery = newBattery;
            _batteryPoint.gameObject.SetActive(true);
            return true;
        }
        return false;
        
    }

    public bool RemoveBattery(out CameraBattery removedBattery)
    {
        if (Battery != null)
        {
            removedBattery = Battery;
            Battery = null;
            _batteryPoint.gameObject.SetActive(false);
            return true;
        }
        removedBattery = null;
        return false;
    }
    
    #endregion
}
