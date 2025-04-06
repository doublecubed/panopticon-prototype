using System.Collections.Generic;
using InventorySystem;using UnityEngine;
using InteractionSystem;

public class CameraInteraction : MonoBehaviour,  IInventoryItem, IInteractable
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private GameObject _prefab;

    [SerializeField] private List<Interaction> _interactions;
    
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

    public List<Interaction> GetInteractions()
    {
        return _interactions;
    }

    public void Interact(InteractionContext context)
    {
        Debug.Log("Interacted");
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
