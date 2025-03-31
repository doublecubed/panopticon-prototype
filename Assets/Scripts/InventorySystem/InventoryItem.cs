using UnityEngine;

namespace InventorySystem
{
    public class InventoryItem : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}
