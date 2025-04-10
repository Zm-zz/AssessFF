using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class BinaryManager
{
    public static void SaveByBinary(string filePath, string fileName, object data)
    {
        FileStream file = null;
        BinaryFormatter bf = new BinaryFormatter();
        var path = Path.Combine(filePath, fileName + ".txt");
        file = File.Open(path, FileMode.Create);
        bf.Serialize(file, data);
        file.Close();
    }

    public static T LoadByBinary<T>(string filePath, string fileName)
    {
        FileStream file = null;
        BinaryFormatter bf = new BinaryFormatter();
        var path = Path.Combine(filePath, fileName + ".txt");
        file = File.Open(path, FileMode.Open);
        T data = (T)bf.Deserialize(file);
        file.Close();
        return data;
    }
}
