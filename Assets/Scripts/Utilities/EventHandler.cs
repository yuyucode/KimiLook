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


    public static event Action<ItemDetails, bool> OnItemSelectedEvent;

    public static void CallOnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        OnItemSelectedEvent?.Invoke(itemDetails, isSelected);
    }


    // ����ί�е��õĸ���
    public static event Action<int, int> OnGameMinuteEvent;

    /// <summary>
    /// ����ί�е��õĸ���
    /// </summary>
    /// <param name="minute">����</param>
    /// <param name="hour">Сʱ</param>
    public static void CallOnGameMinuteEvent(int minute, int hour)
    {
        OnGameMinuteEvent?.Invoke(minute, hour);
    }

    
    public static event Action<int, int, int, int, Season> OnGameDateEvent;

    /// <summary>
    /// ί�д���ʱ����õĸ���
    /// </summary>
    /// <param name="hour">Сʱ</param>
    /// <param name="day">��</param>
    /// <param name="month">��</param>
    /// <param name="year">��</param>
    /// <param name="season">����</param>
    public static void CallOnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        OnGameDateEvent?.Invoke(hour, day, month, year, season);
    }
}
