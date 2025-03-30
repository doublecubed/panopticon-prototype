using System;
using UnityEngine;

public class CameraPower : MonoBehaviour
{
    #region REFERENCES

    [SerializeField] private MeshRenderer _bulb;
    [SerializeField] private Material _powerOnMaterial;
    [SerializeField] private Material _powerOffMaterial;

    private CameraModuleCanvas _moduleCanvas;
    private CameraModuleController _moduleController;
    private PortableCamera _portableCamera;
    
    #endregion
    
    #region VARIABLES
    
    public bool IsTurnedOn { get; private set; }
    [SerializeField] private float _drainRate;

    #endregion
    
    #region MONOBEHAVIOUR
    
    private void Awake()
    {
        _portableCamera = GetComponent<PortableCamera>();
        _moduleController = GetComponent<CameraModuleController>();
        _moduleCanvas = GetComponent<CameraModuleCanvas>();
    }

    private void Start()
    {
        if (CanDrainPower()) SwitchOn();
    }

    private void Update()
    {
        if (IsTurnedOn) DrainPower();
    }

    #endregion
    
    #region METHODS

    private void DrainPower()
    {
        if (CanDrainPower()) _moduleController.Battery.DrainPower(_drainRate * Time.deltaTime);
        else SwitchOff();
    }

    public float PowerLevel()
    {
        return _moduleController.Battery.CurrentCapacity;
    }

    public void TogglePower()
    {
        if (IsTurnedOn) SwitchOff();
        else SwitchOn();
        _moduleCanvas.DressPowerButton();
    }
    
    public void SwitchOff()
    {
        IsTurnedOn = false;
        _bulb.material = _powerOffMaterial;
    }

    public bool SwitchOn()
    {
        bool success = CanDrainPower();
        IsTurnedOn = success;
        _bulb.material = success? _powerOnMaterial: _powerOffMaterial;
        return success;
    }

    private bool CanDrainPower()
    {
        return _moduleController.Battery != null && _moduleController.Battery.CurrentCapacity > 0;
    }
    
    #endregion
}
