using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public const float fadeDuration = 0.3f;
    public const float targetAlpha = 0.65f;

    // 时间相关
    public const float secondThreshold = 0.05f; // 数值越少，时间越快
    public const int secondHold = 59; // 一分钟多少秒
    public const int minuteHold = 59;// 一小时多少分钟
    public const int hourHold = 23;// 一天多少小时
    public const int dayHold = 10; //一个月多少天
    public const int seasonHold = 3; // 一季节多少个月
}
