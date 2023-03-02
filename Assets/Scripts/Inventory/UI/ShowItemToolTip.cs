using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MFarm.Inventory
{
    // 要求有一个SlotUI属性在这个组件中
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SlotUI slotUI;

        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

      
        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if(slotUI.itemAmount != 0)
            {
                inventoryUI.itemToolTip.gameObject.SetActive(true);
                // 获取到InventoryUI组件 -> ItemToolTip 组件 -> 调用SetupTooltip函数
                inventoryUI.itemToolTip.SetupTooltip(slotUI.itemDetails, slotUI.slotType);

                inventoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0);
                inventoryUI.itemToolTip.transform.position = transform.position + Vector3.up * 60;
            }
            else
            {
                inventoryUI.itemToolTip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemToolTip.gameObject.SetActive(false);
        }

    }



}