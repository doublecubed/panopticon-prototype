using InventorySystem;using UnityEngine;
using InteractionSystem;

public class CameraInteraction : MonoBehaviour,  IInventoryItem, IInteractable, IPickupable, IDropable, IActivatable
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _prefab;
    
    private CameraAngleAdjuster _angleAdjuster;
    private CameraModuleCanvas _moduleCanvas;
    
    public bool AdjustmentActive { get; private set; }
    public bool CanvasActive { get; private set; }

    private void Awake()
    {
        _angleAdjuster = GetComponent<CameraAngleAdjuster>();
        _moduleCanvas = GetComponent<CameraModuleCanvas>();
    }


    public string GetItemName()
    {
        return _name;
    }

    public void Activate(InteractionContext context)
    {
        if (!AdjustmentActive) _angleAdjuster.Activate();
        else _angleAdjuster.Deactivate();
        AdjustmentActive = !AdjustmentActive;
    }

    public Sprite GetIcon()
    {
        return _sprite;
    }

    public GameObject GetInventoryPrefab()
    {
        return _prefab;
    }
}
