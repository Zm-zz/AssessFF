/********************************************************************************
 [ 文件 ] 公共日志类
 [ 作者 ] 肖文
 [ 日期 ] 2020/10/10
 [ 描述 ] 公共类库，提供输出统一格式日志的方法
 ********************************************************************************/
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFrame
{
	/// <summary>
	/// [ 公共日志类 ] 公共类库，提供输出统一格式日志的方法
	/// </summary>
	public static class CLog
	{
		/// <summary>
		/// 日志字符串
		/// </summary>
		private static StringBuilder mLog = new StringBuilder();

		public static void InputOperate(string text)
		{
			InputOperate("操作", text);
		}

		public static void InputOperate(string title, string text)
		{
			Input(title, text, "Cyan");
		}

		public static void InputTarget(string text)
		{
			InputTarget("目标", text);
		}

		public static void InputTarget(string title, string text)
		{
			Input(title, text, "Yellow");
		}

		public static void InputRight()
		{
			InputRight("成功");
		}

		public static void InputRight(string text)
		{
			InputRight("正确", text);
		}

		public static void InputRight(string title, string text)
		{
			Input(title, text, "Lime");
		}

		public static void InputWrong()
		{
			InputWrong("失败");
		}

		public static void InputWrong(string text)
		{
			InputWrong("错误", text);
		}

		public static void InputWrong(string title, string text)
		{
			Input(title, text, "Red");
		}

		public static void Input(string title, string text, string color = "White")
        {
            StringBuilder strg = new StringBuilder();
            
            strg.Append("<color=");
            strg.Append(color);
            strg.Append(">");
            strg.Append("[");
            strg.Append(title);
            strg.Append("]");
			strg.Append("</color>");
            strg.Append("<color=White>");
			strg.Append(" ");
			strg.Append(text);
			strg.Append(" ");
			strg.Append("</color>");

			mLog.Append(strg.ToString());
        }

		public static void Output()
		{
			Debug.Log(mLog);
			mLog.Length = 0;
		}
	}
}
