using UnityEngine;

// Ϊ�˸�Unityʶ��д�����к�
[System.Serializable]
public class ItemDetails
{
    public int itemID; // ��Ʒ��ΨһID
    public string itemName; // ��Ʒ������
    public IItemType itemType; // ��Ʒ����
    public Sprite itemIcon; // ��Ʒ��ͼƬ
    public Sprite itemOnWorldSprite; // �����ͼ������ʾ��ͼƬ
    public string itemDescription; // ����
    public int itemUseRadius; // ��Ʒ�ڶ�������Χ��ʹ��
    public bool canPickup; //  �Ƿ�ʰȡ
    public bool canDropped; // �Ƿ񱻶���
    public bool canCarried; // �Ƿ��������
    public int itemPrice; // ��Ʒ�������۸�
    [Range(0, 1)]
    public float sellPercentage; // ���������ۿ۰ٷֱ�
}

// ���ʹ��struct��itemID��itemAmout Ĭ��Ϊ0 

// �����class��ÿ�ζ�Ҫ�жϱ����Ƿ�Ϊ��,��������𲻱�Ҫ���鷳

[System.Serializable]
public struct InventoryItem
{
    public int itemID; // Ĭ��0
    public int itemAmout; // ��ʾ��������Ʒ�ж��ٸ�

}

[System.Serializable]
public class AnimatorType
{
    public PartType partType;
    public PartName partName;

    public AnimatorOverrideController overrideController;
}
