using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;

        [Header("背包数据")]
        public InventoryBag_SO playerBag;

        private void Start()
        {
            // 初始化背包
            EventHandler.CallOnUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// 通过ID，返回对应的物品信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID== ID);
        }

        /// <summary>
        /// 添加物品到Player背包里
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">是否需要销毁物品</param>
        public void AddItem(Item item, bool toDestory)
        {
            // 背包是否有这个物品
            var index = GetItemIndexInBag(item.itemID);

            // 添加物品
            // 还没处理： 背包不存在物品，且背包没有空位的时候
            AddItemAtIndex(item.itemID, index, 1);

            if(toDestory)
            {
                Destroy(item.gameObject);
            }

            // 更新UI
            EventHandler.CallOnUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// 检查背包是否有空位
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for(int i = 0; i < playerBag.itemList.Count; i++)
            {
                // 当itemID为0，背包有位置
                if (playerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                // 当itemID为0，背包有位置
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 在指定背包序号位置添加物品
        /// </summary>
        /// <param name="ID">物品的ID</param>
        /// <param name="index">物品在背包的序号</param>
        /// <param name="amount">添加的数量</param>
        private void AddItemAtIndex(int ID, int index, int amount =1)
        {
            // index == -1 背包没有这个物品
            if(index == -1)
            {
                // 1. 背包没有空位
                if (!CheckBagCapacity())
                {
                    return;
                }

                // 2. 有空位
                // 生成新的物品
                var item = new InventoryItem {itemID = ID, itemAmout = amount};

                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    // 当itemID为0，背包有位置，位置为 i
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item; // 添加物品到Bag
                        break;
                    }
                }

            }
            else  // 背包有这个物品
            {
                int currentAmount = playerBag.itemList[index].itemAmout + amount;
                var item = new InventoryItem { itemID= ID, itemAmout = currentAmount};
                playerBag.itemList[index] = item;
            }
        }

        /// <summary>
        /// Player背包范围内交换物品
        /// </summary>
        /// <param name="fromIndex">起始序号</param>
        /// <param name="targetIndex">目标序号</param>
        public void SwapItem(int fromIndex, int targetIndex)
        {

            InventoryItem currentItem = playerBag.itemList[fromIndex]; // 当前物品
            InventoryItem targetItem = playerBag.itemList[targetIndex]; // 目标物品

            if(targetItem.itemID != 0)
            {
                // 说明目标物品不为空
                playerBag.itemList[fromIndex] = targetItem;
                playerBag.itemList[targetIndex] = currentItem;
            }else
            {
                playerBag.itemList[targetIndex] = currentItem;
                playerBag.itemList[fromIndex] = new InventoryItem();
            }

            //  更新背包
            EventHandler.CallOnUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    }
}

