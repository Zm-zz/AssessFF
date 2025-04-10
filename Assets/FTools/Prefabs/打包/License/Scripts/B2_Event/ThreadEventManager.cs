/********************************************************************************
 [ 文件 ] 线程事件管理类
 [ 作者 ] 肖文
 [ 日期 ] 2018/03/15
 [ 描述 ] 管理线程事件，提供执行线程事件的方法
 ********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace WeFrame
{
	/// <summary>
	/// [ 线程事件委托 ]
	/// </summary>
	public delegate void DThreadEvent();

	/// <summary>
	/// [ 线程事件管理类 ] 管理线程事件，提供执行线程事件的方法
	/// </summary>
	public class ThreadEventManager : UnitySingleton<ThreadEventManager>
	{
		/// <summary>
		/// [ 事件链表 ]
		/// </summary>
		private List<DThreadEvent> mEventList = new List<DThreadEvent>();

		protected override void Refresh()
		{
			base.Refresh();

			for (int i = 0; i < mEventList.Count; i++)
			{
				mEventList[i].Invoke();
			}

			mEventList.Clear();
		}

		public void InvokeEvent(Enum type, BaseEventData data)
		{
			DThreadEvent del = () =>
			{
				EventManager.Self.InvokeEvent(type, data);
			};

			mEventList.Add(del);
		}

        public void InvokeEvent(string index, BaseEventData data)
        {
			DThreadEvent del = () =>
			{
				EventManager.Self.InvokeEvent(index, data);
			};

			mEventList.Add(del);
        }
	}
}
