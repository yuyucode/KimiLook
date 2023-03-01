using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("拖拽图片")]
        public Image dragItem;

        [Header("玩家背包UI")]
        [SerializeField] private GameObject bagUI;
        private bool bagOpened;


        [SerializeField] private SlotUI[] playerSlots;

        private void OnEnable()
        {
            // 订阅委托
            EventHandler.OnUpdateInventoryUI += OnUpdateInventoryUI;
        }

        private void OnDisable()
        {
            // 取消委托
            EventHandler.OnUpdateInventoryUI -= OnUpdateInventoryUI;   
        }


        private void Start()
        {
            // 给每一个格子赋值
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
        /// 打开关闭背包UI, Button调用事件
        /// </summary>
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }

        /// <summary>
        /// 让点击的格子高亮
        /// </summary>
        /// <param name="index"></param>
        public void UpdateSlotHighlight(int index)
        {
            foreach (var slot in playerSlots)
            {
                // 格子选中，且 格子的序列相同
                if(slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHighlight.gameObject.SetActive(true);// 启动高亮
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

