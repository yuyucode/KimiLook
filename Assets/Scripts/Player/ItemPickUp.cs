using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class ItemPickUp : MonoBehaviour
    {

        /// <summary>
        ///  ��Player ���� item �������ĺ���
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // ��ȡ��������Item
            Item item = collision.GetComponent<Item>();

            // �ж��Ƿ�Ϊ��
            if (item != null)
            {
                // �ж��Ƿ����ʰȡ
                if (item.itemDetails.canPickup)
                {
                    // ʰȡ��Ʒ������
                    InventoryManager.Instance.AddItem(item, true);
                }
            }
        }
    }

}

