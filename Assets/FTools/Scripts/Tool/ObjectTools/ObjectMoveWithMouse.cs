using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 物体跟随鼠标移动
/// </summary>
public class ObjectMoveWithMouse : MonoBehaviour
{
    bool canMove = false;
    float distance;

    private void Update()
    {
        if(canMove)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
        }
    }

    public void MoveWithMouse(float distance)
    {
        this.distance = distance;
        canMove = true;
    }

    public void StopMove()
    {
        canMove = false;
    }
}
