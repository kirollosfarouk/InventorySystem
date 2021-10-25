using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "ItemIcons", menuName = "ScriptableObjects/ItemIcons", order = 1)]
    public class InventoryItemIcons : ScriptableObject
    {
        public List<Sprite> itemSprites = new List<Sprite>();
    }
}