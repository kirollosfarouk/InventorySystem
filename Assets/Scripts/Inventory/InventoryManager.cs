using System;
using System.Collections;
using System.Collections.Generic;
using Extension;
using Pooling;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour, IPoolDataSource
    {
        public PooledScrollRectTransform pooledScrollRectTransform;
        public InventoryInfoPanel InfoPanel;
        public InventoryItem InventoryItemPrefab;

        public GameObject Container;

        [Tooltip(tooltip:"Loads the list using this format.")]
        [Multiline]
        public string ItemJson;

        [Tooltip(tooltip:"This is used in generating the items list. The number of additional copies to concat the list parsed from ItemJson.")]
        public int ItemGenerateScale = 10;

        [Tooltip(tooltip:"Icons referenced by ItemData.IconIndex when instantiating new items.")]
        public Sprite[] Icons;

        [Serializable]
        private class InventoryItemDatas
        {
            public InventoryItemData[] ItemDatas;
        }

        private InventoryItemData[] ItemDatas;

        private List<InventoryItem> Items;

        private void Awake()
        {
            pooledScrollRectTransform.DataSource = this;
            pooledScrollRectTransform.blueprintCell = InventoryItemPrefab.GetComponent<RectTransform>();
        }

        private void Start()
        {
            // Clear existing items already in the list.
            var items = Container.GetComponentsInChildren<InventoryItem>();
            foreach (InventoryItem item in items) {
                item.gameObject.transform.SetParent(null);
            }

            ItemDatas = GenerateItemDatas(ItemJson, ItemGenerateScale);
            StartCoroutine(clickOnItem());
        }

        private IEnumerator clickOnItem()
        {
            yield return new WaitForSeconds(0.5f);
            InventoryItemOnClick(Container.GetComponentsInChildren<InventoryItem>()[0], ItemDatas[0]);
        }
        /// <summary>
        /// Generates an item list.
        /// </summary>
        /// <param name="json">JSON to generate items from. JSON must be an array of InventoryItemData.</param>
        /// <param name="scale">Concats additional copies of the array parsed from json.</param>
        /// <returns>An array of InventoryItemData</returns>
        private InventoryItemData[] GenerateItemDatas(string json, int scale) 
        {
            var itemDatas = JsonUtility.FromJson<InventoryItemDatas>(json).ItemDatas;
            var finalItemDatas = new InventoryItemData[itemDatas.Length * scale];
            for (var i = 0; i < itemDatas.Length; i++) {
                for (var j = 0; j < scale; j++) {
                    finalItemDatas[i + j*itemDatas.Length] = itemDatas[i];
                }
            }

            return finalItemDatas;
        }

        private void InventoryItemOnClick(InventoryItem itemClicked, InventoryItemData itemData)
        {
            var items = Container.GetComponentsInChildren<InventoryItem>();
            foreach (var item in items) {
                item.Background.color = Color.white;
            }
            itemClicked.Background.color = Color.red;
            
            InfoPanel.UpdateItemInfo(itemData,Icons[itemData.IconIndex]);
        }

        public int GetItemCount()
        {
            return ItemDatas.Length;
        }

        public void SetCell(ICell cell, int index)
        {
            InventoryItem item = cell as InventoryItem;
            
            Debug.Assert(item != null, nameof(item) + " != null");
            item.ConfigureCell(ItemDatas[index], Icons[ItemDatas[index].IconIndex],()=>InventoryItemOnClick(item,ItemDatas[index]));
        }
    }
}