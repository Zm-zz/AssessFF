using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleController : MonoBehaviour
{
    public bool enable = true;
    public KeyCode key_加速键 = KeyCode.F;
    float speed = 5;
    bool has加速;

    bool canSpeed_打包后 = false;

    private void Awake()
    {
        EventCenterManager.AddListener(GameCommand.加速, (float scale) => { Time.timeScale = scale; });
    }

    public void Update()
    {
        if (CheatingInstructions.CompareInstruction("UGIONACCELERATE"))
        {
            canSpeed_打包后 = !canSpeed_打包后;
            if (canSpeed_打包后)
            {
                MonoFunction.Instance.boxFade.PopLog("加速开启", 1f);
            }
            else
            {
                MonoFunction.Instance.boxFade.PopLog("加速关闭", 1f);
            }
        }
        if (!enable) return;
#if !UNITY_EDITOR
        if (!canSpeed_打包后) return;
#endif
        if (InputManager.GetKey(key_加速键))
        {
            if (!has加速)
            {
                EventCenterManager.Broadcast(GameCommand.加速, speed);
                has加速 = true;
            }
            if(InputManager.GetNumberKeyDown(out int value))
            {
                speed = value;
                MonoFunction.Instance.boxFade.PopLog($"加速速度{value}", 1f);
                has加速 = false;
            }
        }
        else
        {
            if (has加速)
            {
                EventCenterManager.Broadcast(GameCommand.加速, 1f);
                has加速 = false;
            }
        }
    }
}
