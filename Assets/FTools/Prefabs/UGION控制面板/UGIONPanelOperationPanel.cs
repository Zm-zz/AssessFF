using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class UGIONPanelOperationPanel : SingletonPatternMonoBase<UGIONPanelOperationPanel>
{
    public Toggle tog_��������;
    public Toggle tog_Logo;
    public Toggle tog_License;
    public InputField text_�汾��;
    public InputField text_���½�;
    public InputField text_���½�;

    public void SetOperationPanelData()
    {
        if (Exists())
        {
            UGIONPanelOperation operation = LoadOperation();
            if (operation != null)
            {
                tog_��������.isOn = operation.active_��������;
                tog_Logo.isOn = operation.active_Logo;
                tog_License.isOn = operation.active_License;
                text_�汾��.text = operation.str_�汾��;
                text_���½�.text = operation.str_���½�;
                text_���½�.text = operation.str_���½�;
            }
        }
    }

    public void SaveOperation()
    {
        UGIONPanelOperation operation = new UGIONPanelOperation();
        operation.active_�������� = tog_��������.isOn;
        operation.active_Logo = tog_Logo.isOn;
        operation.active_License = tog_License.isOn;
        operation.str_�汾�� = text_�汾��.text;
        operation.str_���½� = text_���½�.text;
        operation.str_���½� = text_���½�.text;

        FileManager.CreatFolder(Application.streamingAssetsPath);
        BinaryManager.SaveByBinary(Application.streamingAssetsPath, "UGIONOperation",operation);
        Debug.Log("UGION����̨����ɹ�");
    }

    public void SaveOperation(UGIONPanelOperation operation)
    {
        FileManager.CreatFolder(Application.streamingAssetsPath);
        BinaryManager.SaveByBinary(Application.streamingAssetsPath, "UGIONOperation", operation);
        Debug.Log("UGION����̨����ɹ�");
    }

    public UGIONPanelOperation LoadOperation()
    {
        if (!Exists())
        {
            Debug.Log("UGION����̨������");
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
    public bool active_��������;
    public bool active_Logo;
    public bool active_License;
    public string str_�汾��;
    public string str_���½�;
    public string str_���½�;
}