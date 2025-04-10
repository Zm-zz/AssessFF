using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonManager
{
    public static void SaveByJson(string filePath, string fileName, object data, bool prettyPrint = false)
    {
        var json = JsonUtility.ToJson(data, prettyPrint);
        var path = Path.Combine(filePath, fileName + ".txt");
        File.WriteAllText(path, json);
    }

    public static T LoadFromJson<T>(string filePath, string fileName)
    {
        var path = Path.Combine(filePath, fileName + ".txt");

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);

            return data;
        }
        catch (Exception)
        {
            return default;
        }
    }
}
