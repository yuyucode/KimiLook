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

