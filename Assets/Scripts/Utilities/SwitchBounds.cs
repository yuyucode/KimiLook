using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �л�����
/// </summary>
public class SwitchBounds : MonoBehaviour
{
    // TODO: �л���������ĵ���
    private void Start()
    {
        SwitchConfinerShape();
    }

    private void SwitchConfinerShape()
    {
        // ��ȡ Collider2D
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        // ��ȡ CinemachineConfiner ���
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        // ���� CinemachineConfiner �� BoundingShape2D ���Ե�ֵ
        confiner.m_BoundingShape2D = confinerShape;

        // ����߽���״�ĵ�������ʱ�����仯������ô˺�������ֹ������
        confiner.InvalidatePathCache();
    }
}