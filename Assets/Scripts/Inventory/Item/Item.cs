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

            // ������
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

            if (itemDetails != null)
            {
                spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;

                // �޸���ײ��ߴ�� offset
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y); // ��ȡͼƬ��ʵ�ʳߴ�
                coll.size = newSize;

                // ƫ�� =�� ����pivot�޸ĵ�Bottom��ʱ�����Ч��
                coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);

            }
        }
    }
}


