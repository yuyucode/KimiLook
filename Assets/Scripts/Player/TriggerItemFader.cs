using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collision 为 Player碰撞到的GameObject Collider2D

        ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();  // 获取collision下的children（树干和树叶）

        if(faders.Length > 0)
        {
            foreach (var item in faders)
            {
                item.FadeOut(); // 循环执行半透明函数
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();

        if (faders.Length > 0)
        {
            foreach (var item in faders)
            {
                item.FadeIn();
            }
        }
    }
}
