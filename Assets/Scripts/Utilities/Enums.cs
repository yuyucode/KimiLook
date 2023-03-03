/// <summary>
/// 物品类型：
/// 
/// Seed 种子
/// Commodity 商品|货物
/// Furniture 家具
/// HoeTool 锄草工具 1002
/// ChopTool 砍树工具 1001
/// BreakTool 砸石头工具  1005
/// ReapTool 割草工具 1004
/// WaterTool 浇水工具 1003
/// CollectTool 菜栏收割的工具 1006
/// ReapableScenery 可以被割的杂草
/// </summary>
public enum IItemType
{
    Seed, Commodity, Furniture,
    HoeTool, ChopTool, BreakTool, ReapTool, WaterTool, CollectTool,
    ReapableScenery
}

/// <summary>
/// 格子类型
/// </summary>
public enum SlotType
{
    Bag, Box, Shop
}


/// <summary>
/// 物品分布
/// </summary>
public enum InventoryLocation
{
    Player, Box
}


/// <summary>
/// 这是什么样的物体，是什么类型
/// </summary>
public enum PartType
{
    None, Carry, Hoe,Break,
}

/// <summary>
/// 身体的各个部分
/// </summary>
public enum PartName
{
    Body, Hair, Arm, Tool
}