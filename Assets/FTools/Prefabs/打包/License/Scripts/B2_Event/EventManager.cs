/********************************************************************************
 [ 文件 ] 事件管理类
 [ 作者 ] 肖文
 [ 日期 ] 2018/03/15
 [ 描述 ] 管理事件，提供注册注销和执行事件的方法
 ********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace WeFrame   
{
    /// <summary>
    /// [ 事件管理类 ] 管理事件，提供注册注销和执行事件的方法
    /// </summary>
    public class EventManager : SystemSingleton<EventManager>
    {
        /// <summary>
        /// [ 事件字典 ]
        /// </summary>
        private Dictionary<string, List<BaseEvent>> mEventDict = new Dictionary<string, List<BaseEvent>>();

        /// <summary>
        /// [ 注册事件 ] 根绝事件类型和事件委托注册事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="del">事件委托</param>
        public void LoginEvent(Enum type, DBaseEvent del)
        {
            LoginEvent(EnumToString(type), new BaseEvent(del));
        }

        /// <summary>
        /// [ 注册事件 ] 根绝事件索引和事件委托注册事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="del">事件委托</param>
        public void LoginEvent(string index, DBaseEvent del)
        {
            LoginEvent(index, new BaseEvent(del));
        }

        /// <summary>
        /// [ 注册事件 ] 根绝事件类型和事件执行者和事件委托注册事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="del">事件委托</param>
        public void LoginEvent(Enum type, object ob, DBaseEvent del)
        {
            LoginEvent(EnumToString(type), new BaseEvent(ob, del));
        }

        /// <summary>
        /// [ 注册事件 ] 根绝事件索引和事件执行者和事件委托注册事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="del">事件委托</param>
        public void LoginEvent(string index, object ob, DBaseEvent del)
        {
            LoginEvent(index, new BaseEvent(ob, del));
        }

        /// <summary>
        /// [ 注册事件 ] 根绝事件类型和事件注册事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="evt">事件</param>
        public void LoginEvent(Enum type, BaseEvent evt)
        {
            LoginEvent(EnumToString(type), evt);
        }

        /// <summary>
        /// [ 注册事件 ] 根绝事件索引和事件注册事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="evt">事件</param>
        public void LoginEvent(string index, BaseEvent evt)
        {
            if (index != null &&
                evt != null &&
                evt.mOnInvoke != null)
            {
                if (!mEventDict.ContainsKey(index))
                {
                    mEventDict.Add(index, new List<BaseEvent>());
                }

                mEventDict[index].Add(evt);
            }
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件类型注销事件
        /// </summary>
        /// <param name="type">事件类型</param>
        public void LogoutEvent(Enum type)
        {
            LogoutEvent(EnumToString(type));
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件索引注销事件
        /// </summary>
        /// <param name="index">事件索引</param>
        public void LogoutEvent(string index)
        {
            if (mEventDict.ContainsKey(index))
            {
                mEventDict[index].Clear();
                mEventDict.Remove(index);
            }
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件类型和事件执行者注销事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="ob">事件执行者</param>
        public void LogoutEvent(Enum type, object ob)
        {
            LogoutEvent(EnumToString(type), ob);
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件索引和事件执行者注销事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="ob">事件执行者</param>
        public void LogoutEvent(string index, object ob)
        {
            Func<BaseEvent, bool> condit = (item) => 
            {
                return item.mInvoker == ob;
            };

            LogoutEvent(index, condit);
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件类型和事件委托注销事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="del">事件委托</param>
        public void LogoutEvent(Enum type, DBaseEvent del)
        {
            LogoutEvent(EnumToString(type), del);
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件索引和事件委托注销事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="del">事件委托</param>
        public void LogoutEvent(string index, DBaseEvent del)
        {
            Func<BaseEvent, bool> condit = (item) => 
            {
                return item.mOnInvoke == del;
            };

            LogoutEvent(index, condit);
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件类型和事件执行者和事件委托注销事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="del">事件委托</param>
        public void LogoutEvent(Enum type, object ob, DBaseEvent del)
        {
            LogoutEvent(EnumToString(type), ob, del);
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件索引和事件执行者和事件委托注销事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="del">事件委托</param>
        public void LogoutEvent(string index, object ob, DBaseEvent del)
        {
            Func<BaseEvent, bool> condit = (item) => 
            {
                return item.mInvoker == ob && item.mOnInvoke == del;
            };

            LogoutEvent(index, condit);
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件类型和事件注销事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="evt">事件</param>
        public void LogoutEvent(Enum type, BaseEvent evt)
        {
            LogoutEvent(EnumToString(type), evt);
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件索引和事件注销事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="evt">事件</param>
        public void LogoutEvent(string index, BaseEvent evt)
        {
            Func<BaseEvent, bool> condit = (item) => 
            {
                return mEventDict[index].Contains(item);
            };

            LogoutEvent(index, condit);
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件类型和事件条件注销事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="condit">事件条件</param>
        public void LogoutEvent(Enum type, Func<BaseEvent, bool> condit)
        {
            LogoutEvent(EnumToString(type), condit);
        }

        /// <summary>
        /// [ 注销事件 ] 根据事件索引和事件条件注销事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="condit">事件条件</param>
        public void LogoutEvent(string index, Func<BaseEvent, bool> condit)
        {
            if (mEventDict.ContainsKey(index))
            {
                List<BaseEvent> list = new List<BaseEvent>();

                foreach (BaseEvent item in mEventDict[index])
                {
                    if (condit == null ||
                        condit(item))
                    {
                        list.Add(item);
                    }
                }

                foreach (BaseEvent item in list)
                {
                    mEventDict[index].Remove(item);
                }
            }
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型执行事件
        /// </summary>
        /// <param name="type">事件类型</param>
        public void InvokeEvent(Enum type)
        {
            InvokeEvent(EnumToString(type));
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        public void InvokeEvent(string index)
        {
            InvokeEvent(index, (BaseEventData)null);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型和事件数据执行事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(Enum type, BaseEventData data)
        {
            InvokeEvent(EnumToString(type), data);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引和事件数据执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(string index, BaseEventData data)
        {
            if (mEventDict.ContainsKey(index))
            {
                foreach (BaseEvent item in mEventDict[index])
                {
                    item.mOnInvoke.Invoke(data);
                }
            }
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型和事件执行者执行事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="ob">事件执行者</param>
        public void InvokeEvent(Enum type, object ob)
        {
            InvokeEvent(EnumToString(type), ob);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引和事件执行者执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="ob">事件执行者</param>
        public void InvokeEvent(string index, object ob)
        {
            InvokeEvent(index, ob, (BaseEventData)null);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型和事件执行者和事件数据执行事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(Enum type, object ob, BaseEventData data)
        {
            InvokeEvent(EnumToString(type), ob, data);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引和事件执行者和事件数据执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(string index, object ob, BaseEventData data)
        {
            Func<BaseEvent, bool> condit = (item) => 
            {
                return item.mInvoker == ob;
            };

            InvokeEvent(index, condit, data);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型和事件委托执行事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="del">事件委托</param>
        public void InvokeEvent(Enum type, DBaseEvent del)
        {
            InvokeEvent(EnumToString(type), del);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引和事件委托执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="del">事件委托</param>
        public void InvokeEvent(string index, DBaseEvent del)
        {
            InvokeEvent(index, del, (BaseEventData)null);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型和事件委托和事件数据执行事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="del">事件委托</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(Enum type, DBaseEvent del, BaseEventData data)
        {
            InvokeEvent(EnumToString(type), del, data);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引和事件委托和事件数据执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="del">事件委托</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(string index, DBaseEvent del, BaseEventData data)
        {
            Func<BaseEvent, bool> condit = (item) => 
            {
                return item.mOnInvoke == del;
            };

            InvokeEvent(index, condit, data);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型和事件执行者和事件委托执行事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="del">事件委托</param>
        public void InvokeEvent(Enum type, object ob, DBaseEvent del)
        {
            InvokeEvent(EnumToString(type), ob, del);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引和事件执行者和事件委托执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="del">事件委托</param>
        public void InvokeEvent(string index, object ob, DBaseEvent del)
        {
            InvokeEvent(index, ob, del, (BaseEventData)null);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型和事件执行者和事件委托和事件数据执行事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="del">事件委托</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(Enum type, object ob, DBaseEvent del, BaseEventData data)
        {
            InvokeEvent(EnumToString(type), ob, del, data);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引和事件执行者和事件委托和事件数据执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="ob">事件执行者</param>
        /// <param name="del">事件委托</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(string index, object ob, DBaseEvent del, BaseEventData data)
        {
            Func<BaseEvent, bool> condit = (item) => 
            {
                return item.mInvoker == ob && item.mOnInvoke == del;
            };

            InvokeEvent(index, condit, data);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型和事件条件执行事件
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <param name="condit">事件条件</param>
        public void InvokeEvent(Enum type, Func<BaseEvent, bool> condit)
        {
            InvokeEvent(EnumToString(type), condit);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引和事件条件执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="condit">事件条件</param>
        public void InvokeEvent(string index, Func<BaseEvent, bool> condit)
        {
            InvokeEvent(index, condit, (BaseEventData)null);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件类型和事件条件和事件数据执行事件
        /// </summary>
        /// <param name="index">事件类型</param>
        /// <param name="condit">事件条件</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(Enum type, Func<BaseEvent, bool> condit, BaseEventData data)
        {
             InvokeEvent(EnumToString(type), condit, data);
        }

        /// <summary>
        /// [ 执行事件 ] 根据事件索引和事件条件和事件数据执行事件
        /// </summary>
        /// <param name="index">事件索引</param>
        /// <param name="condit">事件条件</param>
        /// <param name="data">事件数据</param>
        public void InvokeEvent(string index, Func<BaseEvent, bool> condit, BaseEventData data)
        {
            if (mEventDict.ContainsKey(index))
            {
                foreach (BaseEvent item in mEventDict[index])
                {
                    if (condit == null ||
                        condit(item))
                    {
                        item.mOnInvoke.Invoke(data);
                    }
                }
            }
        }

        public string EnumToString(Enum type)
		{
			string data = type.GetType().Name + "_" + type.ToString();

			return data;
		}
    }
}
