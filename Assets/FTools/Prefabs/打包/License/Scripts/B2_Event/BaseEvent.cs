/********************************************************************************
 [ 文件 ] 基础事件类
 [ 作者 ] 肖文
 [ 日期 ] 2018/03/15
 [ 描述 ] 规范事件类，提供构造事件的方法
 ********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace WeFrame
{
    /// <summary>
    /// [ 基础事件委托 ]
    /// </summary>
    public delegate void DBaseEvent(BaseEventData data);

    /// <summary>
    /// [ 基础事件数据类 ]
    /// </summary>
    public class BaseEventData : SystemObject
    {
        
    }

    /// <summary>
    /// [ 基础事件类 ] 规范事件类，提供构造事件的方法
    /// </summary>
    public class BaseEvent : SystemObject
    {
        public object mInvoker = null;

        public DBaseEvent mOnInvoke = null;

        public BaseEvent(DBaseEvent evt)
        {
            mOnInvoke = evt;
        }

        public BaseEvent(Object ob, DBaseEvent evt)
        {
            mInvoker = ob;
            mOnInvoke = evt;
        }
    }
}
