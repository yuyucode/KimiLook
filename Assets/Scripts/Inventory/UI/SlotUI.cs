using TMPro; // 字体
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler,IBeginDragHandler,IDragHandler, IEndDragHandler
    {
        [Header("组件获取")]

        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHighlight;
        [SerializeField] private Button button;

        public SlotType slotType; // 格子类型
        public bool isSelected; // 是否选中

        // 每个格子的物品信息和个数
        public ItemDetails itemDetails;
        public int itemAmount;
        public int slotIndex;
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;

            if (itemDetails.itemID == 0)
            {
                // 物体为空
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// 更新格子UI和信息
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">持有数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            slotImage.enabled = true;
            button.interactable = true;
        }

        /// <summary>
        /// 将Slot更新为空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false; // 关闭图片
            amountText.text = string.Empty; // 文字为空
            button.interactable = false; // Button 不可点击
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0) return;
            isSelected = !isSelected; // 状态切换

            inventoryUI.UpdateSlotHighlight(slotIndex);
        }

        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true; // 设置可见
                inventoryUI.dragItem.sprite = slotImage.sprite; // 设置拖拽的图片
                inventoryUI.dragItem.SetNativeSize(); //设置本来的尺寸

                isSelected = true;
                inventoryUI.UpdateSlotHighlight(slotIndex); // 激活高亮
            }
        }

        /// <summary>
        /// 拖拽过程
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="System.NotImplementedException"></exception>

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition; //拖拽的图片等于鼠标的位置
        }

        /// <summary>
        /// 停止拖拽
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled=false; // 关闭

            // 获取鼠标最后的位置 
            Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        }
    }

}