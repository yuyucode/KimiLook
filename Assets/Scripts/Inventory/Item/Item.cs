using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;

        private SpriteRenderer spriteRenderer;

        private BoxCollider2D coll;

        public ItemDetails itemDetails;

        private void Awake()
        {
            spriteRenderer= GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if(itemID != 0)
            {
                Init(itemID);
            }
        }


        public void Init(int ID)
        {
            itemID = ID;

            // 拿数据
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

            if (itemDetails != null)
            {
                spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;

                // 修改碰撞体尺寸和 offset
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y); // 获取图片的实际尺寸
                coll.size = newSize;

                // 偏移 =》 处理pivot修改到Bottom的时候处理的效果
                coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);

            }
        }
    }
}


