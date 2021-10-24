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
        
        public void ConfigureCell(InventoryItemData itemData,Sprite image, UnityAction onClick)
        {
          Icon.sprite = image;
          Name.text = itemData.Name;
          Button.onClick.AddListener(onClick);
        }

    }
}
