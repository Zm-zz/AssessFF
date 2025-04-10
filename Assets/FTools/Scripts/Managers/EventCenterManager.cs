using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 事件中心管理器
/// </summary>
public class EventCenterManager
{
    //键表示命令的名字
    //值表示命令具体要执行的逻辑
    static Dictionary<string, IEventInfo> eventsDictionary = new Dictionary<string, IEventInfo>();

    #region 监听命令
    /// <summary>
    /// 监听命令
    /// </summary>
    /// <param name="command">命令</param>
    /// <param name="call">这条命令要干嘛</param>
    public static void AddListener(object command, Action call)
    {
        string key = command.GetType().Name + "_" + command.ToString();
        if (eventsDictionary.ContainsKey(key))
            ((EventInfo)eventsDictionary[key]).action += call;
        else
            eventsDictionary.Add(key, new EventInfo(call));
    }
    public static void AddListener<T1>(object command, Action<T1> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1>).action += call;
        else
            eventsDictionary.Add(key, new EventInfo<T1>(call));
    }
    public static void AddListener<T1, T2>(object command, Action<T1, T2> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2>).action += call;
        else
            eventsDictionary.Add(key, new EventInfo<T1, T2>(call));
    }
    public static void AddListener<T1, T2, T3>(object command, Action<T1, T2, T3> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3>).action += call;
        else
            eventsDictionary.Add(key, new EventInfo<T1, T2, T3>(call));
    }
    public static void AddListener<T1, T2, T3, T4>(object command, Action<T1, T2, T3, T4> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4>).action += call;
        else
            eventsDictionary.Add(key, new EventInfo<T1, T2, T3, T4>(call));
    }
    public static void AddListener<T1, T2, T3, T4, T5>(object command, Action<T1, T2, T3, T4, T5> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name + typeof(T5).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4, T5>).action += call;
        else
            eventsDictionary.Add(key, new EventInfo<T1, T2, T3, T4, T5>(call));
    }
    public static void AddListener<T1, T2, T3, T4, T5, T6>(object command, Action<T1, T2, T3, T4, T5, T6> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name + typeof(T5).Name + typeof(T6).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4, T5, T6>).action += call;
        else
            eventsDictionary.Add(key, new EventInfo<T1, T2, T3, T4, T5, T6>(call));
    }
    #endregion

    #region 广播
    /// <summary>
    /// 发送没有参数的命令
    /// </summary>
    /// <param name="command">命令</param>
    public static void Broadcast(object command)
    {
        string key = command.GetType().Name + "_" + command.ToString();
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo).action?.Invoke();
    }
    /// <summary>
    /// 发送有一个参数的命令
    /// </summary>
    public static void Broadcast<T1>(object command, T1 arg1)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1>).action?.Invoke(arg1);
    }
    /// <summary>
    /// 发送有两个参数的命令
    /// </summary>
    public static void Broadcast<T1, T2>(object command, T1 arg1, T2 arg2)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2>).action?.Invoke(arg1, arg2);
    }
    /// <summary>
    /// 发送有三个参数的命令
    /// </summary>
    public static void Broadcast<T1, T2, T3>(object command, T1 arg1, T2 arg2, T3 arg3)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3>).action?.Invoke(arg1, arg2, arg3);
    }
    /// <summary>
    /// 发送有四个参数的命令
    /// </summary>
    public static void Broadcast<T1, T2, T3, T4>(object command, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4>).action?.Invoke(arg1, arg2, arg3, arg4);
    }
    /// <summary>
    /// 发送有五个参数的命令
    /// </summary>
    public static void Broadcast<T1, T2, T3, T4, T5>(object command, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name + typeof(T5).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4, T5>).action?.Invoke(arg1, arg2, arg3, arg4, arg5);
    }
    /// <summary>
    /// 发送有六个参数的命令
    /// </summary>
    public static void Broadcast<T1, T2, T3, T4, T5, T6>(object command, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name + typeof(T5).Name + typeof(T6).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4, T5, T6>).action?.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }
    #endregion

    #region 取消监听
    /// <summary>
    /// 取消监听的命令
    /// </summary>
    public static void RemoveListener(object command, Action call)
    {
        string key = command.GetType().Name + "_" + command.ToString();
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo).action -= call;
    }
    /// <summary>
    /// 移除一条命令的所有事件
    /// </summary>
    public static void RemoveListeners(object command)
    {
        string key = command.GetType().Name + "_" + command.ToString();
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo).action = null;
    }

    public static void RemoveListener<T1>(object command, Action<T1> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1>).action -= call;
    }
    public static void RemoveListeners<T1>(object command)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1>).action = null;
    }

    public static void RemoveListener<T1, T2>(object command, Action<T1, T2> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2>).action -= call;
    }
    public static void RemoveListeners<T1, T2>(object command)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2>).action = null;
    }

    public static void RemoveListener<T1, T2, T3>(object command, Action<T1, T2, T3> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3>).action -= call;
    }
    public static void RemoveListeners<T1, T2, T3>(object command)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3>).action = null;
    }

    public static void RemoveListener<T1, T2, T3, T4>(object command, Action<T1, T2, T3, T4> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4>).action -= call;
    }
    public static void RemoveListeners<T1, T2, T3, T4>(object command)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4>).action = null;
    }

    public static void RemoveListener<T1, T2, T3, T4, T5>(object command, Action<T1, T2, T3, T4, T5> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name + typeof(T5).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4, T5>).action -= call;
    }
    public static void RemoveListeners<T1, T2, T3, T4, T5>(object command)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name + typeof(T5).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4, T5>).action = null;
    }

    public static void RemoveListener<T1, T2, T3, T4, T5, T6>(object command, Action<T1, T2, T3, T4, T5, T6> call)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name + typeof(T5).Name + typeof(T6).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4, T5, T6>).action -= call;
    }
    public static void RemoveListeners<T1, T2, T3, T4, T5, T6>(object command)
    {
        string key = command.GetType().Name + "_" + command.ToString() + "_" + typeof(T1).Name + typeof(T2).Name + typeof(T3).Name + typeof(T4).Name + typeof(T5).Name + typeof(T6).Name;
        if (eventsDictionary.ContainsKey(key))
            (eventsDictionary[key] as EventInfo<T1, T2, T3, T4, T5, T6>).action = null;
    }
    #endregion

    /// <summary>
    /// 移除事件中心的所有事件。可以考虑在切换场景时调用。
    /// </summary>
    public static void RemoveAllListeners()
    {
        eventsDictionary.Clear();
    }

    private interface IEventInfo { }//用于里氏替换原则
    private class EventInfo : IEventInfo
    {
        public Action action;
        public EventInfo(Action call)
        {
            action += call;
        }
    }
    private class EventInfo<T1> : IEventInfo
    {
        public Action<T1> action;

        public EventInfo(Action<T1> call)
        {
            action += call;
        }
    }
    private class EventInfo<T1, T2> : IEventInfo
    {
        public Action<T1, T2> action;

        public EventInfo(Action<T1, T2> call)
        {
            action += call;
        }
    }
    private class EventInfo<T1, T2, T3> : IEventInfo
    {
        public Action<T1, T2, T3> action;

        public EventInfo(Action<T1, T2, T3> call)
        {
            action += call;
        }
    }
    private class EventInfo<T1, T2, T3, T4> : IEventInfo
    {
        public Action<T1, T2, T3, T4> action;

        public EventInfo(Action<T1, T2, T3, T4> call)
        {
            action += call;
        }
    }
    private class EventInfo<T1, T2, T3, T4, T5> : IEventInfo
    {
        public Action<T1, T2, T3, T4, T5> action;

        public EventInfo(Action<T1, T2, T3, T4, T5> call)
        {
            action += call;
        }
    }
    private class EventInfo<T1, T2, T3, T4, T5, T6> : IEventInfo
    {
        public Action<T1, T2, T3, T4, T5, T6> action;

        public EventInfo(Action<T1, T2, T3, T4, T5, T6> call)
        {
            action += call;
        }
    }
}
