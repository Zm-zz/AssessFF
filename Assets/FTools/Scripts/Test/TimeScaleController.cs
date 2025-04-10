using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleController : MonoBehaviour
{
    public bool enable = true;
    public KeyCode key_���ټ� = KeyCode.F;
    float speed = 5;
    bool has����;

    bool canSpeed_����� = false;

    private void Awake()
    {
        EventCenterManager.AddListener(GameCommand.����, (float scale) => { Time.timeScale = scale; });
    }

    public void Update()
    {
        if (CheatingInstructions.CompareInstruction("UGIONACCELERATE"))
        {
            canSpeed_����� = !canSpeed_�����;
            if (canSpeed_�����)
            {
                MonoFunction.Instance.boxFade.PopLog("���ٿ���", 1f);
            }
            else
            {
                MonoFunction.Instance.boxFade.PopLog("���ٹر�", 1f);
            }
        }
        if (!enable) return;
#if !UNITY_EDITOR
        if (!canSpeed_�����) return;
#endif
        if (InputManager.GetKey(key_���ټ�))
        {
            if (!has����)
            {
                EventCenterManager.Broadcast(GameCommand.����, speed);
                has���� = true;
            }
            if(InputManager.GetNumberKeyDown(out int value))
            {
                speed = value;
                MonoFunction.Instance.boxFade.PopLog($"�����ٶ�{value}", 1f);
                has���� = false;
            }
        }
        else
        {
            if (has����)
            {
                EventCenterManager.Broadcast(GameCommand.����, 1f);
                has���� = false;
            }
        }
    }
}
