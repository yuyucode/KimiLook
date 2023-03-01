using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    // ����һ��ί�У�ע�����InventoryUI
    public static event Action<InventoryLocation, List<InventoryItem>> OnUpdateInventoryUI;


    // ���������ĵ�ί��
    public static void CallOnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        OnUpdateInventoryUI?.Invoke(location, list);
    }
}
