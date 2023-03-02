using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    // 1. ����һ��ί�У�ע�����InventoryUI
    public static event Action<InventoryLocation, List<InventoryItem>> OnUpdateInventoryUI;

    // ���������ĵ�ί��
    public static void CallOnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        OnUpdateInventoryUI?.Invoke(location, list);
    }


    // 2. ����һ��ί�У�������Ʒ����ͼ
    public static event Action<int, Vector3> OnInstantiateItenScene;

    public static void CallOnInstantiateInScene(int ID, Vector3 pos)
    {
        OnInstantiateItenScene?.Invoke(ID, pos);
    }

}
