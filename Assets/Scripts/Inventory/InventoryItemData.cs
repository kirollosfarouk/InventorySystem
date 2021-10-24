using System;

namespace Inventory
{
    [Serializable]
    public class InventoryItemData {
    
        public int IconIndex;
        public string Name;
        public string Description;
        public int Stat;
    }
    
    [Serializable]
    public class InventoryItemsData
    {
        public InventoryItemData[] ItemDatas;
    }

}