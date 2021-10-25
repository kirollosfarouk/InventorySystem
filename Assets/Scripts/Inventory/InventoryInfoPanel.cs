using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryInfoPanel : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI itemDescriptionText;
        [SerializeField] private TextMeshProUGUI itemStatsText;
        [SerializeField] private InventoryItemIcons icons;
        public void UpdateItemInfo(InventoryItemData itemData)
        {
            itemIcon.sprite = icons.itemSprites[itemData.IconIndex];
            itemNameText.text = itemData.Name;
            itemDescriptionText.text = itemData.Description;
            itemStatsText.text = itemData.Stat.ToString();
        }
    }
}
