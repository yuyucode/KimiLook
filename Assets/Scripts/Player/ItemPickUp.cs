using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class ItemPickUp : MonoBehaviour
    {

        /// <summary>
        ///  当Player 碰到 item 所触发的函数
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 获取触碰到的Item
            Item item = collision.GetComponent<Item>();

            // 判断是否为空
            if (item != null)
            {
                // 判断是否可以拾取
                if (item.itemDetails.canPickup)
                {
                    // 拾取物品到背包
                    InventoryManager.Instance.AddItem(item, true);
                }
            }
        }
    }

}

