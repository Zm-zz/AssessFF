using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Mono管理器
/// </summary>
public class MonoManager :SingletonPatternBase<MonoManager>
{
    //构造方法私有化，防止外部new对象。
    private MonoManager() { }

    private MonoController monoExecuter;
    private MonoController MonoExecuter
    {
        get
        {
            if (monoExecuter==null)
            {
                GameObject go = new GameObject(typeof(MonoController).Name);//相当于就是"MonoController"
                monoExecuter = go.AddComponent<MonoController>();
            }

            return monoExecuter;
        }
    }

    /// <summary>
    /// 让外部通过它来开启协程
    /// </summary>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return MonoExecuter.StartCoroutine(routine);
    }

    /// <summary>
    /// 让外部通过它来停止协程
    /// </summary>
    public void StopCoroutine(IEnumerator routine)
    {
        if(routine!=null)
            MonoExecuter.StopCoroutine(routine);
    }

    /// <summary>
    /// 让外部通过它来停止协程
    /// </summary>
    public void StopCoroutine(Coroutine routine)
    {
        if(routine!=null)
            MonoExecuter.StopCoroutine(routine);
    }

    /// <summary>
    /// 让外部通过它来停止所有协程
    /// </summary>
    public void StopAllCoroutines()
    {
        MonoExecuter.StopAllCoroutines();
    }

    /// <summary>
    /// 添加Update事件
    /// </summary>
    public void AddUpdateListener(UnityAction call)
    {
        MonoExecuter.AddUpdateListener(call);
    }

    /// <summary>
    /// 移除Update事件
    /// </summary>
    public void RemoveUpdateListener(UnityAction call)
    {
        MonoExecuter.RemoveUpdateListener(call);
    }

    /// <summary>
    /// 移除所有Update事件
    /// </summary>
    public void RemoveAllUpdateListeners()
    {
        MonoExecuter.RemoveAllUpdateListeners();
    }

    /// <summary>
    /// 添加FixedUpdate事件
    /// </summary>
    public void AddFixedUpdateListener(UnityAction call)
    {
        MonoExecuter.AddFixedUpdateListener(call);
    }

    /// <summary>
    /// 移除FixedUpdate事件
    /// </summary>
    public void RemoveFixedUpdateListener(UnityAction call)
    {
        MonoExecuter.RemoveFixedUpdateLinstener(call);
    }

    /// <summary>
    /// 移除所有FixedUpdate事件
    /// </summary>
    public void RemoveAllFixedUpdateListeners()
    {
        MonoExecuter.RemoveAllFixedUpdateListeners();
    }

    /// <summary>
    /// 添加LateUpdate事件
    /// </summary>
    public void AddLateUpdateListener(UnityAction call)
    {
        MonoExecuter.AddLateUpdateListener(call);
    }

    /// <summary>
    /// 移除LateUpdate事件
    /// </summary>
    public void RemoveLateUpdateListener(UnityAction call)
    {
        MonoExecuter.RemoveLateUpdateListener(call);
    }

    /// <summary>
    /// 移除所有LateUpdate事件
    /// </summary>
    public void RemoveAllLateUpdateListeners()
    {
        MonoExecuter.RemoveAllLateUpdateListeners();
    }

    /// <summary>
    /// 移除FixedUpdate、Update、LateUpdate的所有事件
    /// </summary>
    public void RemoveAllListeners()
    {
        MonoExecuter.RemoveAllListeners();
    }


    public class MonoController : MonoBehaviour
    {
        event UnityAction updateEvent;//在生命周期方法Update中执行的事件

        event UnityAction fixedUpdateEvent;//在生命周期方法FixedUpdate中执行的事件

        event UnityAction lateUpdateEvent;//在生命周期方法LateUpdate中执行的事件

        void FixedUpdate()
        {
            fixedUpdateEvent?.Invoke();
        }


        void Update()
        {
            updateEvent?.Invoke();
        }

        void LateUpdate()
        {
            lateUpdateEvent?.Invoke();
        }

        public void AddUpdateListener(UnityAction call)
        {
            updateEvent += call;
        }

        public void RemoveUpdateListener(UnityAction call)
        {
            updateEvent -= call;
        }

        public void RemoveAllUpdateListeners()
        {
            updateEvent = null;
        }

        public void AddFixedUpdateListener(UnityAction call)
        {
            fixedUpdateEvent += call;
        }

        public void RemoveFixedUpdateLinstener(UnityAction call)
        {
            fixedUpdateEvent -= call;
        }

        public void RemoveAllFixedUpdateListeners()
        {
            fixedUpdateEvent = null;
        }

        public void AddLateUpdateListener(UnityAction call)
        {
            lateUpdateEvent += call;
        }

        public void RemoveLateUpdateListener(UnityAction call)
        {
            lateUpdateEvent -= call;
        }

        public void RemoveAllLateUpdateListeners()
        {
            lateUpdateEvent = null;
        }

        public void RemoveAllListeners()
        {
            RemoveAllFixedUpdateListeners();
            RemoveAllUpdateListeners();
            RemoveAllLateUpdateListeners();
        }
    }
}
