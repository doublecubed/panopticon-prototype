using UnityEngine;

public class CameraBattery : CameraModule
{
    [field: SerializeField] public float MaxCapacity { get; private set; }
    [field: SerializeField] public float CurrentCapacity { get; private set; }

    public bool RechargePower(float amount)
    {
        CurrentCapacity = Mathf.Min(CurrentCapacity + amount, MaxCapacity);
        return CurrentCapacity < MaxCapacity;
    }
    
    public bool DrainPower(float amount)
    {
        CurrentCapacity = Mathf.Max(CurrentCapacity - amount, 0);
        return CurrentCapacity > 0;
    }
}
