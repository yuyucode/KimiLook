using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // �룬�֣�Сʱ���գ��£���
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    private Season gameSeason = Season.����;

    private int monthInSeason = 3;

    public bool gameClockPause;
    private float tikTime;

    private void Awake()
    {
        NewGameTime();
    }

    private void Update()
    {
        if(!gameClockPause)
        {
            tikTime += Time.deltaTime;

            if(tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }
    }

    private void NewGameTime()
    {
        gameSecond = 0;
        gameMinute = 0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2022;
        gameSeason = Season.����;
    }

    private void UpdateGameTime()
    {
        // ����
        gameSecond++;

        // ���� ����  ������ֵ��59�� ʱ
        if (gameSecond > Settings.secondHold)
        {
            // ����+1������Ϊ0
            gameMinute++;
            gameSecond = 0;

            // ���� ���� ������ֵ��59��ʱ
            if(gameMinute > Settings.minuteHold)
            {
                // Сʱ+1������Ϊ0
                gameHour++;
                gameMinute = 0;

                // Сʱ ���� Сʱ��ֵ��23��ʱ
                if (gameHour > Settings.hourHold)
                {
                    // ����+1��СʱΪ0
                    gameDay++;
                    gameHour = 0;

                    // ���� ���� ������ֵ��30��ʱ
                    if (gameDay > Settings.dayHold)
                    {
                        // �·�+1������Ϊ1
                        gameMonth++;
                        gameDay = 1;
                        
                        // �·ݴ���12
                        if(gameMonth > 12)
                        {
                            gameMonth = 1;
                        }

                        // ����һ���£�����--
                        monthInSeason--;

                        //  ������ == 0ʱ��
                        if(monthInSeason == 0)
                        {
                            // ����һ�����ȣ���ԭ3
                            monthInSeason = 3;

                            // ����һ�����ȣ�seasonNumber ����
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            // ���ȹ���4�Ρ�����һ��
                            if(seasonNumber > Settings.seasonHold)
                            {
                                // ������
                                seasonNumber = 0;
                                gameYear++;
                            }

                            // �������
                            gameSeason = (Season)seasonNumber;

                            if(gameYear > 9999)
                            {
                                gameYear = 2022;
                            }
                        }
                    }
                }
            }
        }
       
    }
}
