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

        public void UpdateItemInfo(InventoryItemData itemData, Sprite itemSprite)
        {
            itemIcon.sprite = itemSprite;
            itemNameText.text = itemData.Name;
            itemDescriptionText.text = itemData.Description;
            itemStatsText.text = itemData.Stat.ToString();
        }
    }
}
