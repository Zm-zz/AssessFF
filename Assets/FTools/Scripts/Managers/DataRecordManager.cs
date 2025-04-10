using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRecordManager
{
    public static List<Dictionary<string, object>> dictionaries = new List<Dictionary<string, object>>();

    public static void SetValue<TValue>(string key, TValue value)
    {
        // 查找与 TValue 类型匹配的字典
        Dictionary<string, object> dictionary;
        if (FindDictionary<TValue>() != -1)
        {
            dictionary = dictionaries[FindDictionary<TValue>()];
        }
        else
        {
            dictionary = new Dictionary<string, object>();
            dictionaries.Add(dictionary);
        }
        dictionary[key] = value;
    }

    public static TValue GetValue<TValue>(string key)
    {
        int index = FindDictionary<TValue>();
        Dictionary<string, object> dictionary;
        if (index != -1)
        {
            dictionary = dictionaries[index];
            if (dictionary.ContainsKey(key))
            {
                return (TValue)dictionary[key];
            }
        }
        Debug.LogWarning($"未找到{key}对应{typeof(TValue)}类型的value值");
        return default;
    }

    public static void RemoveValue<TValue>(string key)
    {
        int index = FindDictionary<TValue>();
        Dictionary<string, object> dictionary;
        if (index != -1)
        {
            dictionary = dictionaries[index];
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                if (dictionary.Count == 0)
                {
                    dictionaries.Remove(dictionary);
                }
            }
        }
    }

    public static void ClearValues<TValue>()
    {
        int index = FindDictionary<TValue>();
        if (index != -1)
        {
            Dictionary<string, object> dictionary = dictionaries[index];
            dictionary.Clear();
            dictionaries.Remove(dictionary);
        }
    }

    public static void ClearValues()
    {
        dictionaries.Clear();
    }

    public static bool CompareValue<TValue>(string key, TValue value)
    {
        return value.Equals(GetValue<TValue>(key));
    }

    static int FindDictionary<TValue>()
    {
        for (int i = 0; i < dictionaries.Count; i++)
        {
            foreach (var v in dictionaries[i])
            {
                if (v.Value is TValue)
                {
                    return i;
                }
            }
        }
        return -1;
    }
}
