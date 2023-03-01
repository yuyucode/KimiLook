using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    // 定义一个委托：注册更新InventoryUI
    public static event Action<InventoryLocation, List<InventoryItem>> OnUpdateInventoryUI;


    // 用来被订阅的委托
    public static void CallOnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        OnUpdateInventoryUI?.Invoke(location, list);
    }
}
