/********************************************************************************
 [ 文件 ] Regedit管理类
 [ 作者 ] 肖文
 [ 日期 ] 2018/03/15
 [ 描述 ] 管理Regedit数据，提供设置和获取Regedit数据的方法
 ********************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeFrame
{
	public class RegeditManager : SystemSingleton<RegeditManager>
	{
		public void SetInt(string index, int value)
        {
            PlayerPrefs.SetInt(index, value);
        }

		public void SetFloat(string index, float value)
        {
            PlayerPrefs.SetFloat(index, value);
        }

		public void SetBool(string index, bool value)
        {
            PlayerPrefs.SetInt(index, value ? 1 : 0);
        }

		public void SetString(string index, string value)
        {
            PlayerPrefs.SetString(index, value);
        }

        public void SetObject(string index, object value)
        {
            string strg = JsonUtility.ToJson(value);

            PlayerPrefs.SetString(index, strg);
        }

        public int GetInt(string index)
        {
            return PlayerPrefs.GetInt(index, 0);
        }

		public float GetFloat(string index)
        {
            return PlayerPrefs.GetFloat(index, 0);
        }

		public bool GetBool(string index)
        {
            return PlayerPrefs.GetInt(index, 0) != 0;
        }

		public string GetString(string index)
        {
            return PlayerPrefs.GetString(index, "");
        }

		public TObject GetObject<TObject>(string index) where TObject : class
		{
			string strg = PlayerPrefs.GetString(index, "");

			return JsonUtility.FromJson<TObject>(strg);
		}

        public void RemoveData(string index)
        {
            PlayerPrefs.DeleteKey(index);
        }

        public void RemoveData()
        {
            PlayerPrefs.DeleteAll();
        }

        public void SaveData()
        {
            PlayerPrefs.Save();
        }

		public bool IsContainData(string index)
		{
			return PlayerPrefs.HasKey(index);
		}
	}
}