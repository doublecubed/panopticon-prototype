using UnityEngine;
using InteractionSystem;
using InventorySystem;

public class CubeSocket : MonoBehaviour, IInteractable, ISocket
{
    [SerializeField] private string _name;
    [SerializeField] private Transform _attachPoint;
    
    public string GetItemName()
    {
        return _name;
    }

    public bool CanReceiveAttachable(InteractionContext context)
    {
        Debug.Log("checking Iattachable compatibility");
        return context.InventoryInteractable is IAttachable;
    }

    public Transform AttachmentPoint()
    {
        return _attachPoint;
    }

    public void ReceiveAttachable(InteractionContext context)
    {
        Debug.Log("Cube successfully received");
    }
}
