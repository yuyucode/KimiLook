using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("��Ʒ����")]
        public ItemDataList_SO itemDataList_SO;

        [Header("��������")]
        public InventoryBag_SO playerBag;

        private void Start()
        {
            // ��ʼ������
            EventHandler.CallOnUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// ͨ��ID�����ض�Ӧ����Ʒ��Ϣ
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID== ID);
        }

        /// <summary>
        /// �����Ʒ��Player������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">�Ƿ���Ҫ������Ʒ</param>
        public void AddItem(Item item, bool toDestory)
        {
            // �����Ƿ��������Ʒ
            var index = GetItemIndexInBag(item.itemID);

            // �����Ʒ
            // ��û���� ������������Ʒ���ұ���û�п�λ��ʱ��
            AddItemAtIndex(item.itemID, index, 1);

            if(toDestory)
            {
                Destroy(item.gameObject);
            }

            // ����UI
            EventHandler.CallOnUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// ��鱳���Ƿ��п�λ
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for(int i = 0; i < playerBag.itemList.Count; i++)
            {
                // ��itemIDΪ0��������λ��
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
                // ��itemIDΪ0��������λ��
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// ��ָ���������λ�������Ʒ
        /// </summary>
        /// <param name="ID">��Ʒ��ID</param>
        /// <param name="index">��Ʒ�ڱ��������</param>
        /// <param name="amount">��ӵ�����</param>
        private void AddItemAtIndex(int ID, int index, int amount =1)
        {
            // index == -1 ����û�������Ʒ
            if(index == -1)
            {
                // 1. ����û�п�λ
                if (!CheckBagCapacity())
                {
                    return;
                }

                // 2. �п�λ
                // �����µ���Ʒ
                var item = new InventoryItem {itemID = ID, itemAmout = amount};

                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    // ��itemIDΪ0��������λ�ã�λ��Ϊ i
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item; // �����Ʒ��Bag
                        break;
                    }
                }

            }
            else  // �����������Ʒ
            {
                int currentAmount = playerBag.itemList[index].itemAmout + amount;
                var item = new InventoryItem { itemID= ID, itemAmout = currentAmount};
                playerBag.itemList[index] = item;
            }
        }
    }
}

