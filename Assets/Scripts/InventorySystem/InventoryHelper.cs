using UnityEngine;

namespace InventorySystem
{
    public static class InventoryHelper
    {
        public static GameObject ItemObject(IInventoryItem item)
        {
            return ((Component)item).gameObject;
        }
    }
}