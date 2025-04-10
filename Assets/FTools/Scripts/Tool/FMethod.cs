using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public static class FMethod
{
    //设置alpha值
    public static Color SetColor_A(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    public static Color SetColor_A(ref Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    //颜色值获取
    public static Color GetColor(string colorStr)
    {
        Color color;
        if (colorStr[0] != '#') colorStr = "#" + colorStr;
        ColorUtility.TryParseHtmlString(colorStr, out color);
        return color;
    }

    //获取vector3
    public static Vector3 GetVector3_X(Vector3 vector)
    {
        return new Vector3(vector.x, 0, 0);
    }
    public static Vector3 GetVector3_Y(Vector3 vector)
    {
        return new Vector3(0, vector.y, 0);
    }
    public static Vector3 GetVector3_Z(Vector3 vector)
    {
        return new Vector3(0, 0, vector.z);
    }
    public static Vector3 GetVector3_XY(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, 0);
    }
    public static Vector3 GetVector3_XZ(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }
    public static Vector3 GetVector3_YZ(Vector3 vector)
    {
        return new Vector3(0, vector.y, vector.z);
    }
    public static Vector3 GetVector3(Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, vector.z);
    }

    //设置vector3
    public static Vector3 SetVector3_X(Vector3 vector, float x)
    {
        return new Vector3(x, vector.y, vector.z);
    }
    public static Vector3 SetVector3_Y(Vector3 vector, float y)
    {
        return new Vector3(vector.x, y, vector.z);
    }
    public static Vector3 SetVector3_Z(Vector3 vector, float z)
    {
        return new Vector3(vector.x, vector.y, z);
    }
    public static Vector3 SetVector3_XY(Vector3 vector, float x, float y)
    {
        return new Vector3(x, y, vector.z);
    }
    public static Vector3 SetVector3_XZ(Vector3 vector, float x, float z)
    {
        return new Vector3(x, vector.y, z);
    }
    public static Vector3 SetVector3_YZ(Vector3 vector, float y, float z)
    {
        return new Vector3(vector.x, y, z);
    }

    //三角函数转换
    public static float TransAngleToRadian(float angle)
    {
        return angle * Mathf.Deg2Rad;
    }
    public static float TransRadianToAngle(float rad)
    {
        return rad * Mathf.Rad2Deg;
    }
    public static float Sin(float angle)
    {
        return Mathf.Sin(TransAngleToRadian(angle));
    }
    public static float Cos(float angle)
    {
        return Mathf.Cos(TransAngleToRadian(angle));
    }
    public static float Tan(float angle)
    {
        return Mathf.Tan(TransAngleToRadian(angle));
    }

    //获取顶层UI
    public static GameObject GetCurrentUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, result);
            return result[0].gameObject;
        }
        return null;
    }

    //退出程序
    public static void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif PLATFORM_WEBGL
        if(WebInteract.IsFullScreen())
        {
            WebInteract.ExitFullScreen();
        }
        else
        {
            WebInteract.Reload();
        }
#else
        Application.Quit();
#endif
    }

    //延迟调用
    public static void DelayAction(float time, UnityAction action)
    {
        MonoManager.Instance.StartCoroutine(DelayActionCoroutine(time, action));
    }
    static IEnumerator DelayActionCoroutine(float time, UnityAction action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    //枚举获取(typeof(枚举类型))
    public static int GetEnumLength(Type enumType)
    {
        return Enum.GetValues(enumType).Length;
    }
    public static string[] GetEnumValues(Type enumType)
    {
        return Enum.GetNames(enumType);
    }
    public static string GetEnumValue(Type enumType,int index)
    {
        return Enum.GetNames(enumType)[index];
    }

    //数组随机排序
    public static List<T> RandomList<T>(List<T> list)
    {
        int index;
        T temp;
        for (int i = 0; i < list.Count; i++)
        {
            index = UnityEngine.Random.Range(0, list.Count);
            if (index != i)
            {
                temp = list[i];
                list[i] = list[index];
                list[index] = temp;
            }
        }
        return list;
    }
    public static T[] RandomArray<T>(T[] list)
    {
        int index;
        T temp;
        for (int i = 0; i < list.Length; i++)
        {
            index = UnityEngine.Random.Range(0, list.Length);
            if (index != i)
            {
                temp = list[i];
                list[i] = list[index];
                list[index] = temp;
            }
        }
        return list;
    }

    //数组列表转换
    public static T[] TransformListToArray<T>(List<T> list)
    {
        return list.ToArray();
    }
    public static List<T> TransformArrayToList<T>(T[] list)
    {
        return list.ToList();
    }

    //射线调用
    public static bool GetHit(string layerMask,out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit, 9999, LayerMask.GetMask(layerMask));
    }

    //时间转换
    public static string TransTime_时分秒(float time)
    {
        int hour = (int)time / 3600;
        int minute = (int)(time % 3600) / 60;
        int second = (int)(time % 3600) % 60;
        return hour.ToString("00") + ":" + minute.ToString("00") + ":" + second.ToString("00");
    }
    public static string TransTime_分秒(float time)
    {
        int minute = (int)(time % 3600) / 60;
        int second = (int)(time % 3600) % 60;
        return minute.ToString("00") + ":" + second.ToString("00");
    }

    //获取粘贴板
    public static string GetCopy()
    {
        return GUIUtility.systemCopyBuffer;
    }
    public static string[] GetCopyRow()
    {
        return GUIUtility.systemCopyBuffer.Split("\t");
    }
    public static string[] GetCopyCol()
    {
        return GUIUtility.systemCopyBuffer.Split("\r\n");
    }
    public static string[][] GetCopyExcelForm()
    {
        string[] raw = GetCopyCol();
        string[][] result = new string[raw.Length][];
        for (int i = 0; i < raw.Length; i++)
        {
            result[i] = raw[i].Split("\t");
        }
        return result;
    }

    //数组初始化数值
    public static void ResetArray<T>(T[] array, T resetNum)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = resetNum;
        }
    }
    public static void ResetList<T>(List<T> list, T resetNum)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = resetNum;
        }
    }
}
