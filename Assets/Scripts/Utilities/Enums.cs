/// <summary>
/// ��Ʒ���ͣ�
/// 
/// Seed ����
/// Commodity ��Ʒ|����
/// Furniture �Ҿ�
/// HoeTool ���ݹ��� 1002
/// ChopTool �������� 1001
/// BreakTool ��ʯͷ����  1005
/// ReapTool ��ݹ��� 1004
/// WaterTool ��ˮ���� 1003
/// CollectTool �����ո�Ĺ��� 1006
/// ReapableScenery ���Ա�����Ӳ�
/// </summary>
public enum IItemType
{
    Seed, Commodity, Furniture,
    HoeTool, ChopTool, BreakTool, ReapTool, WaterTool, CollectTool,
    ReapableScenery
}

/// <summary>
/// ��������
/// </summary>
public enum SlotType
{
    Bag, Box, Shop
}


/// <summary>
/// ��Ʒ�ֲ�
/// </summary>
public enum InventoryLocation
{
    Player, Box
}


/// <summary>
/// ����ʲô�������壬��ʲô����
/// </summary>
public enum PartType
{
    None, Carry, Hoe,Break,
}

/// <summary>
/// ����ĸ�������
/// </summary>
public enum PartName
{
    Body, Hair, Arm, Tool
}

/// <summary>
/// ����
/// </summary>
public enum Season
{
    ����,����,����,����
}