using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;

    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private Text valueText;
    [SerializeField] private GameObject bottomPart;

    /// <summary>
    /// ����Tooltip
    /// </summary>
    /// <param name="itemDetails">��Ʒ��Ϣ</param>
    /// <param name="slotType">��Ʒ����</param>
    public void SetupTooltip(ItemDetails itemDetails, SlotType slotType)
    {
        nameText.text = itemDetails.itemName;

        typeText.text = GetItemType(itemDetails.itemType);

        descriptionText.text = itemDetails.itemDescription;

        if (itemDetails.itemType == IItemType.Seed ||
            itemDetails.itemType == IItemType.Commodity ||
            itemDetails.itemType == IItemType.Furniture)
        {
            bottomPart.SetActive(true);
            var price = itemDetails.itemPrice;
            
            if(slotType == SlotType.Bag)
            {
                price = (int)(price * itemDetails.sellPercentage);
            }

            valueText.text = price.ToString();
        }else
        {
            bottomPart.SetActive(false);
        }

        // ǿ���������¹����ܼ���Ӱ��Ĳ���Ԫ�غ��Ӳ���Ԫ�ء�
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private string GetItemType(IItemType itemType)
    {
        return itemType switch
        {
            IItemType.Seed => "����",
            IItemType.Commodity => "��Ʒ",
            IItemType.Furniture => "�Ҿ�",
            IItemType.BreakTool => "����",
            IItemType.ChopTool => "����",
            IItemType.CollectTool => "����",
            IItemType.HoeTool => "����",
            IItemType.ReapTool => "����",
            IItemType.WaterTool => "����",
            _ => "��"
        };
    }
}
