using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("��קͼƬ")]
        public Image dragItem;

        [Header("��ұ���UI")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpened;


        [SerializeField] private SlotUI[] playerSlots;

        private void OnEnable()
        {
            // ����ί��
            EventHandler.OnUpdateInventoryUI += OnUpdateInventoryUI;
        }

        private void OnDisable()
        {
            // ȡ��ί��
            EventHandler.OnUpdateInventoryUI -= OnUpdateInventoryUI;   
        }


        private void Start()
        {
            // ��ÿһ�����Ӹ�ֵ
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex= i;
            }

            bagOpened = bagUI.activeInHierarchy;
        }


        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.B)) {
                OpenBagUI();
            }
        }

        private void OnUpdateInventoryUI(InventoryLocation lcation, List<InventoryItem> list)
        {
            switch(lcation)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemAmout > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmout);
                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// �򿪹رձ���UI, Button�����¼�
        /// </summary>
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }

        /// <summary>
        /// �õ���ĸ��Ӹ���
        /// </summary>
        /// <param name="index"></param>
        public void UpdateSlotHighlight(int index)
        {
            foreach (var slot in playerSlots)
            {
                // ����ѡ�У��� ���ӵ�������ͬ
                if(slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHighlight.gameObject.SetActive(true);// ��������
                }
                else
                {
                    slot.isSelected= false;
                    slot.slotHighlight.gameObject.SetActive(false);
                }
            }
        }
    }

}

