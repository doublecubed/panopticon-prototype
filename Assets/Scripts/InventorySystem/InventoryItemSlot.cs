using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace InventorySystem
{
    public class InventoryItemSlot : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _icon;
        [SerializeField] private Transform _itemTransform;
        [SerializeField] private TMP_Text _indexText;
        
        private InventoryItem _item;
        
        public void Initialize(int number)
        {
            _indexText.text = number.ToString();
        }

        public void SetBackgroundColor(Color color)
        {
            _background.color = color;
        }

        public void SetBackgroundAlpha(float alpha)
        {
            Color color = new Color(_background.color.r, _background.color.g, _background.color.b, alpha);
            SetBackgroundColor(color);
        }

        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }
    }
}
