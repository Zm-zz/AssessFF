using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using UnityEditor.ProjectWindowCallback;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreatFTaskDemoScript : MonoBehaviour
{
    //脚本模板路径
    private const string TemplateScriptPath = "Assets/FTools/Scripts/Task/Editor/FTaskDemo.cs.txt";

    //菜单项
    [MenuItem("Assets/Create/FScripts/FTaskDemo", false, 1)]
    static void CreateScript()
    {
        string path = "Assets";
        foreach (UnityEngine.Object item in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(item);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<CreateFTaskDemoScriptAsset>(),
        path + "/Task.cs",
        null, TemplateScriptPath);

    }

}


class CreateFTaskDemoScriptAsset : EndNameEditAction
{
    public override void Action(int instanceId, string newScriptPath, string templatePath)
    {
        UnityEngine.Object obj = CreateTemplateScriptAsset(newScriptPath, templatePath);
        ProjectWindowUtil.ShowCreatedAsset(obj);
    }

    public static UnityEngine.Object CreateTemplateScriptAsset(string newScriptPath, string templatePath)
    {
        string fullPath = Path.GetFullPath(newScriptPath);
        StreamReader streamReader = new StreamReader(templatePath);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(newScriptPath);
        //替换模板的文件名
        text = Regex.Replace(text, "FTaskDemo", fileNameWithoutExtension);
        bool encoderShouldEmitUTF8Identifier = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(newScriptPath);
        return AssetDatabase.LoadAssetAtPath(newScriptPath, typeof(UnityEngine.Object));
    }

}