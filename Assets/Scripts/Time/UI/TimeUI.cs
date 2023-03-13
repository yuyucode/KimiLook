using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public RectTransform dayNightImage; // 旋转的图片

    public RectTransform clockParent; // 时钟的父对象

    public Image seasonImage; // 季节的图片

    public TextMeshProUGUI dateText; // 时间文本

    public TextMeshProUGUI timeText; // 精确时间文本

    public Sprite[] seasonSprites; //季节图片数组

    private List<GameObject> clockBlocks = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < clockParent.childCount; i++)
        {
            // 循环clockParent的子对象，并添加到List数组中，方便修改
            clockBlocks.Add(clockParent.GetChild(i).gameObject);
            clockParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        EventHandler.OnGameMinuteEvent += OnGameMinuteEvent;
        EventHandler.OnGameDateEvent += OnGameDateEvent;
    }

    private void OnDisable()
    {
        EventHandler.OnGameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.OnGameDateEvent -= OnGameDateEvent;
    }

    private void OnGameMinuteEvent(int minute, int hour)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }

    private void OnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        // 日期
        dateText.text = year + "年" + month.ToString("00") + "月" + day.ToString("00") + "日";

        // 当前季节图标
        seasonImage.sprite = seasonSprites[(int)season];

        SwitchHourImage(hour);
        DayNightImageRotate(hour);
    }

    /// <summary>
    /// 根据是小时切换小时图片
    /// </summary>
    /// <param name="hour">小时</param>

    private void SwitchHourImage(int hour)
    {
        int index = hour / 4;

        if(index == 0)
        {
            foreach (var item in clockBlocks)
            {
                item.SetActive(false);
            }
        }else
        {
            for (int i = 0; i < clockBlocks.Count; i++)
            {
                if(i < index + 1)
                {
                    clockBlocks[i].SetActive(true);
                }else
                {
                    clockBlocks[i].SetActive(false);
                }
            }
        }
    }


    private void DayNightImageRotate(int hour)
    {
        // - 90 调整图片的初始位置
        var target = new Vector3(0, 0, hour * 15 - 90);
        dayNightImage.DORotate(target, 1f, RotateMode.Fast);
    }

 

    
}
