using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 自定义地图组件
/// </summary>
public class MapImage : Image
{
    /// <summary>
    /// 重写射线检测方法
    /// </summary>
    /// <param name="screenPoint"></param>
    /// <param name="eventCamera"></param>
    /// <returns></returns>
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return GetComponent<PolygonCollider2D>().OverlapPoint(screenPoint);
    }
}