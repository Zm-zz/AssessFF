/********************************************************************************
 [ 文件 ] System基础类
 [ 作者 ] 肖文
 [ 日期 ] 2019/02/12
 [ 描述 ] 规范System类，提供生命周期的基础方法
 ********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace WeFrame
{
    /// <summary>
    /// [ System基础类 ] 规范System类，提供生命周期的基础方法
    /// </summary>
    public abstract class SystemObject : IDisposable
    {
        /// <summary>
        /// [ 构造 ]
        /// </summary>
        public SystemObject() { Init(); }

        /// <summary>
        /// [ 销毁 ]
        /// </summary>
        public void Dispose() { Release(); }

        /// <summary>
        /// [ 初始 ] 构造方法执行时会调用初始方法
        /// </summary>
        protected virtual void Init() { }

        /// <summary>
        /// [ 释放 ] 销毁方法执行时会调用释放方法
        /// </summary>
        protected virtual void Release() { }
    }
}
