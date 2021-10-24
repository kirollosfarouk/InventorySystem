using System;
using Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryItem : MonoBehaviour, ICell
    {
        public Image Background;
        public Image Icon;
        public TextMeshProUGUI Name;
        public Button Button;

        public int itemIndex;

        public void ConfigureCell(InventoryItemData itemData, int index, Sprite image, UnityAction onClick)
        {
            itemIndex = index;
            Icon.sprite = image;
            Name.text = itemData.Name;
            Button.onClick.AddListener(onClick);
        }

        public void SetSelected(bool isSelected)
        {
            Background.color = isSelected ? Color.red : Color.white;
        }
    }
}