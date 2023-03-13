using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // 秒，分，小时，日，月，年
    private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    private Season gameSeason = Season.春天;

    private int monthInSeason = 3;

    public bool gameClockPause;
    private float tikTime;

    private void Awake()
    {
        // 初始化
        NewGameTime();
    }

    // Start周期会在Enable后面执行，防止没触发（更新日期）
    private void Start()
    {
        EventHandler.CallOnGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        EventHandler.CallOnGameMinuteEvent(gameMinute, gameHour);
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

        // 空格 + H ,快速跳过一分钟
        if(Input.GetKey(KeyCode.T)) { 
            for(int i = 0; i < 60; i++)
            {
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
        gameSeason = Season.春天;
    }

    private void UpdateGameTime()
    {
        // 秒数
        gameSecond++;

        // 秒数 大于  秒数阈值（59） 时
        if (gameSecond > Settings.secondHold)
        {
            // 分钟+1，秒数为0
            gameMinute++;
            gameSecond = 0;

            // 分钟 大于 分钟阈值（59）时
            if(gameMinute > Settings.minuteHold)
            {
                // 小时+1，分钟为0
                gameHour++;
                gameMinute = 0;

                // 小时 大于 小时阈值（23）时
                if (gameHour > Settings.hourHold)
                {
                    // 天数+1，小时为0
                    gameDay++;
                    gameHour = 0;

                    // 天数 大于 天数阈值（30）时
                    if (gameDay > Settings.dayHold)
                    {
                        // 月份+1，天数为1
                        gameMonth++;
                        gameDay = 1;
                        
                        // 月份大于12
                        if(gameMonth > 12)
                        {
                            gameMonth = 1;
                        }

                        // 过了一个月，季度--
                        monthInSeason--;

                        //  当季度 == 0时，
                        if(monthInSeason == 0)
                        {
                            // 过了一个季度，复原3
                            monthInSeason = 3;

                            // 到了一个季度，seasonNumber 增加
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;

                            // 季度过了4次。增加一年
                            if(seasonNumber > Settings.seasonHold)
                            {
                                // 序列数
                                seasonNumber = 0;
                                gameYear++;
                            }

                            // 变更季节
                            gameSeason = (Season)seasonNumber;

                            if(gameYear > 9999)
                            {
                                gameYear = 2022;
                            }
                        }
                    }
                }

               
            }
            EventHandler.CallOnGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        }


        EventHandler.CallOnGameMinuteEvent(gameMinute, gameHour);
    }
}
