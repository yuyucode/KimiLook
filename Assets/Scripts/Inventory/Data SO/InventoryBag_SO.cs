using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ��������SO�ļ�
[CreateAssetMenu(fileName ="InventoryBag_SO" ,menuName= "Inventory/InventoryBag_SO")]

public class InventoryBag_SO : ScriptableObject
{
    public List<InventoryItem> itemList;
}
