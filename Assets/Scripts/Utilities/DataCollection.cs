using UnityEngine;

// 为了给Unity识别，写上序列号
[System.Serializable]
public class ItemDetails
{
    public int itemID; // 物品的唯一ID
    public string name; // 物品的名字
    public IItemType itemType; // 物品类型
    public Sprite itemIcon; // 物品的图片
    public Sprite itemOnWorldSprite; // 世界地图上所显示的图片
    public string itemDescription; // 详情
    public int itemUseRadius; // 物品在多少网格范围内使用
    public bool canPickup; //  是否被拾取
    public bool canDropped; // 是否被丢弃
    public bool canCarried; // 是否可以拿起
    public int itemPrice; // 物品的售卖价格
    [Range(0, 1)]
    public float sellPercentage; // 所售卖的折扣百分比
}

