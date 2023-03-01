using TMPro; // ����
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler,IBeginDragHandler,IDragHandler, IEndDragHandler
    {
        [Header("�����ȡ")]

        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        public Image slotHighlight;
        [SerializeField] private Button button;

        public SlotType slotType; // ��������
        public bool isSelected; // �Ƿ�ѡ��

        // ÿ�����ӵ���Ʒ��Ϣ�͸���
        public ItemDetails itemDetails;
        public int itemAmount;
        public int slotIndex;
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;

            if (itemDetails.itemID == 0)
            {
                // ����Ϊ��
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// ���¸���UI����Ϣ
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">��������</param>
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
        /// ��Slot����Ϊ��
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }

            slotImage.enabled = false; // �ر�ͼƬ
            amountText.text = string.Empty; // ����Ϊ��
            button.interactable = false; // Button ���ɵ��
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0) return;
            isSelected = !isSelected; // ״̬�л�

            inventoryUI.UpdateSlotHighlight(slotIndex);
        }

        /// <summary>
        /// ��ʼ��ק
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(itemAmount != 0)
            {
                inventoryUI.dragItem.enabled = true; // ���ÿɼ�
                inventoryUI.dragItem.sprite = slotImage.sprite; // ������ק��ͼƬ
                inventoryUI.dragItem.SetNativeSize(); //���ñ����ĳߴ�

                isSelected = true;
                inventoryUI.UpdateSlotHighlight(slotIndex); // �������
            }
        }

        /// <summary>
        /// ��ק����
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="System.NotImplementedException"></exception>

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position = Input.mousePosition; //��ק��ͼƬ��������λ��
        }

        /// <summary>
        /// ֹͣ��ק
        /// </summary>
        /// <param name="eventData"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled=false; // �ر�

            // ��ȡ�������λ�� 
            Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        }
    }

}