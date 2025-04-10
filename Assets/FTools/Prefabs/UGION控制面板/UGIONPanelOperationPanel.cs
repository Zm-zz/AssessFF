using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class UGIONPanelOperationPanel : SingletonPatternMonoBase<UGIONPanelOperationPanel>
{
    public Toggle tog_开屏动画;
    public Toggle tog_Logo;
    public Toggle tog_License;
    public InputField text_版本号;
    public InputField text_左下角;
    public InputField text_右下角;

    public void SetOperationPanelData()
    {
        if (Exists())
        {
            UGIONPanelOperation operation = LoadOperation();
            if (operation != null)
            {
                tog_开屏动画.isOn = operation.active_开屏动画;
                tog_Logo.isOn = operation.active_Logo;
                tog_License.isOn = operation.active_License;
                text_版本号.text = operation.str_版本号;
                text_左下角.text = operation.str_左下角;
                text_右下角.text = operation.str_右下角;
            }
        }
    }

    public void SaveOperation()
    {
        UGIONPanelOperation operation = new UGIONPanelOperation();
        operation.active_开屏动画 = tog_开屏动画.isOn;
        operation.active_Logo = tog_Logo.isOn;
        operation.active_License = tog_License.isOn;
        operation.str_版本号 = text_版本号.text;
        operation.str_左下角 = text_左下角.text;
        operation.str_右下角 = text_右下角.text;

        FileManager.CreatFolder(Application.streamingAssetsPath);
        BinaryManager.SaveByBinary(Application.streamingAssetsPath, "UGIONOperation",operation);
        Debug.Log("UGION控制台保存成功");
    }

    public void SaveOperation(UGIONPanelOperation operation)
    {
        FileManager.CreatFolder(Application.streamingAssetsPath);
        BinaryManager.SaveByBinary(Application.streamingAssetsPath, "UGIONOperation", operation);
        Debug.Log("UGION控制台保存成功");
    }

    public UGIONPanelOperation LoadOperation()
    {
        if (!Exists())
        {
            Debug.Log("UGION控制台不存在");
            return null;
        }
        UGIONPanelOperation operation = new UGIONPanelOperation();
        try
        {
            operation = BinaryManager.LoadByBinary<UGIONPanelOperation>(Application.streamingAssetsPath, "UGIONOperation");
            return operation;
        }
        catch
        {
            FMethod.DelayAction(1, () => { FileManager.DeleteFile(Application.streamingAssetsPath, "UGIONOperation.txt"); });
            return null;
        }
    }

    public bool Exists()
    {
        return File.Exists(Application.streamingAssetsPath + "/UGIONOperation.txt");
    }
}

[Serializable]
public class UGIONPanelOperation
{
    public bool active_开屏动画;
    public bool active_Logo;
    public bool active_License;
    public string str_版本号;
    public string str_左下角;
    public string str_右下角;
}