using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collision Ϊ Player��ײ����GameObject Collider2D

        ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>();  // ��ȡcollision�µ�children�����ɺ���Ҷ��

        if(faders.Length > 0)
        {
            foreach (var item in faders)
            {
                item.FadeOut(); // ѭ��ִ�а�͸������
            }
        }

    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemFader[] faders = collision.GetComponentsInChildren<ItemFader>(); // ��ȡcollision�µ�children�����ɺ���Ҷ��

        if (faders.Length > 0)
        {
            foreach (var item in faders)
            {
                item.FadeIn(); // �ָ�ԭ��
            }
        }
    }
}
