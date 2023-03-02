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
    /// 设置Tooltip
    /// </summary>
    /// <param name="itemDetails">物品信息</param>
    /// <param name="slotType">物品类型</param>
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

        // 强制立即重新构建受计算影响的布局元素和子布局元素。
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private string GetItemType(IItemType itemType)
    {
        return itemType switch
        {
            IItemType.Seed => "种子",
            IItemType.Commodity => "商品",
            IItemType.Furniture => "家具",
            IItemType.BreakTool => "工具",
            IItemType.ChopTool => "工具",
            IItemType.CollectTool => "工具",
            IItemType.HoeTool => "工具",
            IItemType.ReapTool => "工具",
            IItemType.WaterTool => "工具",
            _ => "无"
        };
    }
}
