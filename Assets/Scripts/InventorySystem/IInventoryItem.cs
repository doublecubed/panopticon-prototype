using UnityEngine;

namespace InventorySystem
{
    public interface IInventoryItem
    {
        public Sprite GetIcon();
        public GameObject GetInventoryPrefab();
    }
}
