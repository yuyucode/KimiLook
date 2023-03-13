using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TimeUI : MonoBehaviour
{
    public RectTransform dayNightImage; // ��ת��ͼƬ

    public RectTransform clockParent; // ʱ�ӵĸ�����

    public Image seasonImage; // ���ڵ�ͼƬ

    public TextMeshProUGUI dateText; // ʱ���ı�

    public TextMeshProUGUI timeText; // ��ȷʱ���ı�

    public Sprite[] seasonSprites; //����ͼƬ����

    private List<GameObject> clockBlocks = new List<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < clockParent.childCount; i++)
        {
            // ѭ��clockParent���Ӷ��󣬲���ӵ�List�����У������޸�
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
        // ����
        dateText.text = year + "��" + month.ToString("00") + "��" + day.ToString("00") + "��";

        // ��ǰ����ͼ��
        seasonImage.sprite = seasonSprites[(int)season];

        SwitchHourImage(hour);
        DayNightImageRotate(hour);
    }

    /// <summary>
    /// ������Сʱ�л�СʱͼƬ
    /// </summary>
    /// <param name="hour">Сʱ</param>

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
        // - 90 ����ͼƬ�ĳ�ʼλ��
        var target = new Vector3(0, 0, hour * 15 - 90);
        dayNightImage.DORotate(target, 1f, RotateMode.Fast);
    }

 

    
}
