using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler
{
    // 1. 定义一个委托：注册更新InventoryUI
    public static event Action<InventoryLocation, List<InventoryItem>> OnUpdateInventoryUI;

    // 用来被订阅的委托
    public static void CallOnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
    {
        OnUpdateInventoryUI?.Invoke(location, list);
    }


    // 2. 定义一个委托，生成物品到地图
    public static event Action<int, Vector3> OnInstantiateItenScene;

    public static void CallOnInstantiateInScene(int ID, Vector3 pos)
    {
        OnInstantiateItenScene?.Invoke(ID, pos);
    }


    public static event Action<ItemDetails, bool> OnItemSelectedEvent;

    public static void CallOnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        OnItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }


    // 分钟委托调用的更新
    public static event Action<int, int> OnGameMinuteEvent;

    /// <summary>
    /// 分钟委托调用的更新
    /// </summary>
    /// <param name="minute">分钟</param>
    /// <param name="hour">小时</param>
    public static void CallOnGameMinuteEvent(int minute, int hour)
    {
        OnGameMinuteEvent?.Invoke(minute, hour);
    }

    
    public static event Action<int, int, int, int, Season> OnGameDateEvent;

    /// <summary>
    /// 委托大体时间调用的更新
    /// </summary>
    /// <param name="hour">小时</param>
    /// <param name="day">天</param>
    /// <param name="month">月</param>
    /// <param name="year">年</param>
    /// <param name="season">季节</param>
    public static void CallOnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        OnGameDateEvent?.Invoke(hour, day, month, year, season);
    }
}
