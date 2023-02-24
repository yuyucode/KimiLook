using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 切换场景
/// </summary>
public class SwitchBounds : MonoBehaviour
{
    // TODO: 切换场景后更改调用
    private void Start()
    {
        SwitchConfinerShape();
    }

    private void SwitchConfinerShape()
    {
        // 获取 Collider2D
        PolygonCollider2D confinerShape = GameObject.FindGameObjectWithTag("BoundsConfiner").GetComponent<PolygonCollider2D>();

        // 获取 CinemachineConfiner 组件
        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        // 设置 CinemachineConfiner 的 BoundingShape2D 属性的值
        confiner.m_BoundingShape2D = confinerShape;

        // 如果边界形状的点在运行时发生变化，则调用此函数，防止被缓存
        confiner.InvalidatePathCache();
    }
}