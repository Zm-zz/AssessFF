/********************************************************************************
 [ 文件 ] Unity基础类
 [ 作者 ] 肖文
 [ 日期 ] 2018/03/15
 [ 描述 ] 规范Unity类，提供生命周期的基础方法
 ********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFrame
{
    /// <summary>
    /// [ Unity基础类 ] 规范Unity类，提供生命周期的基础方法
    /// </summary>
    public abstract class UnityObject : MonoBehaviour
    {
        /// <summary>
        /// [ 唤醒 ]
        /// </summary>
        private void Awake() { BeforeInit(); }

        /// <summary>
        /// [ 开始 ]
        /// </summary>
        private void Start() { Init(); StartCoroutine(DoStart()); }

        /// <summary>
        /// [ 更新 ]
        /// </summary>
        private void Update() { Refresh(); }

        /// <summary>
        /// [ 后更新 ]
        /// </summary>
        private void LateUpdate() { AfterRefresh(); }

        /// <summary>
        /// [ 激活 ]
        /// </summary>
        private void OnEnable() { Active(); }

        /// <summary>
        /// [ 失活 ]
        /// </summary>
        private void OnDisable() { Inactive(); }

        /// <summary>
        /// [ 销毁 ]
        /// </summary>
        private void OnDestroy() { Release(); }

        /// <summary>
        /// [ 开始协程 ]
        /// </summary>
        private IEnumerator DoStart() { yield return new WaitForFixedUpdate(); AfterInit(); }

        /// <summary>
        /// [ 前初始 ] 唤醒方法执行时会调用前初始方法
        /// </summary>
        protected virtual void BeforeInit() { }

        /// <summary>
        /// [ 初始 ] 开始方法执行时会调用初始方法
        /// </summary>
        protected virtual void Init() { }

        /// <summary>
        /// [ 后初始 ] 开始协程方法执行时在初始方法之后刷新方法之前会调用后初始方法
        /// </summary>
        protected virtual void AfterInit() { }

        /// <summary>
        /// [ 刷新 ] 更新方法执行时会调用刷新方法
        /// </summary>
        protected virtual void Refresh() { }

        /// <summary>
        /// [ 后刷新 ] 后更新方法执行时会调用后刷新方法
        /// </summary>
        protected virtual void AfterRefresh() { }

        /// <summary>
        /// [ 生效 ] 激活方法执行时会调用生效方法
        /// </summary>
        protected virtual void Active() { }

        /// <summary>
        /// [ 失效 ] 失活方法执行时会调用失效方法
        /// </summary>
        protected virtual void Inactive() { }

        /// <summary>
        /// [ 释放 ] 销毁方法执行时会调用释放方法
        /// </summary>
        protected virtual void Release() { }
    }
}
