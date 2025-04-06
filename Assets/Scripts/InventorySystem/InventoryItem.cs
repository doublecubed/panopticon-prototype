using UnityEngine;

namespace InventorySystem
{
    public class InventoryItem : MonoBehaviour, IInventoryItem
    {
        [field: SerializeField] public string Name { get; private set; }
        [SerializeField] private Sprite _icon;
        [SerializeField] private GameObject _prefab;
        
        public Sprite GetIcon()
        {
            return _icon;
        }

        public GameObject GetInventoryPrefab()
        {
            return _prefab;
        }
    }
}
