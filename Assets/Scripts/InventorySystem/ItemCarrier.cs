using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ItemCarrier
    {
        [SerializeField] private Transform _carryTransform;
        [SerializeField] private List<GameObject> _items;
    }
}