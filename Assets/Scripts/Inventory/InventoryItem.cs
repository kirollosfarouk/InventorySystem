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
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private Button button;
        [SerializeField] private InventoryItemIcons icons;
        
        [HideInInspector] public int itemIndex;

        public void ConfigureCell(InventoryItemData itemData, int index, UnityAction onClick)
        {
            itemIndex = index;
            icon.sprite = icons.itemSprites[itemData.IconIndex];
            itemNameText.text = itemData.Name;
            button.onClick.AddListener(onClick);
        }

        public void SetSelected(bool isSelected)
        {
            background.color = isSelected ? Color.red : Color.white;
        }
    }
}