using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Unity.VisualScripting;

public class FileManager
{
    /// <summary>
    /// �����ļ���
    /// </summary>
    /// <param name="filePath">�ļ���·��</param>
    /// <param name="fileName">�ļ�����</param>
    public static void CreatFolder(string filePath,string fileName)
    {
        var file = Path.Combine(filePath, fileName);
        //�ж��ļ���·���Ƿ����
        if (!Directory.Exists(file))
        {
            //����
            Directory.CreateDirectory(file);
        }
    }
    public static void CreatFolder(string filePath)
    {
        //�ж��ļ���·���Ƿ����
        if (!Directory.Exists(filePath))
        {
            //����
            Directory.CreateDirectory(filePath);
        }
    }



    /// <summary>
    /// ͨ��·�������ļ��Ƿ����
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }
    public static bool Exists(string filePath, string fileName)
    {
        return Exists(Path.Combine(filePath, fileName));
    }

    /// <summary>
    /// ·��ɾ���ļ�
    /// </summary>
    /// <param name="filePath"></param>
    public static void DeleteFile(string filePath)
    {
        File.Delete(filePath);
    }
    public static void DeleteFile(string filePath, string fileName)
    {
        DeleteFile(Path.Combine(filePath, fileName));
    }


    /// <summary>
    /// ��ȡ·���ļ����������ļ���
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DirectoryInfo[] GetChildFolders(string path)
    {
        if (!Exists(path)) return new DirectoryInfo[] { };

        //�ļ�����һ��������ļ���
        //SearchOption.TopDirectoryOnly�����ѡ��ֻȡ��һ������ļ�
        //SearchOption.AllDirectories�����ѡ���ȡ�������е����ļ�
        DirectoryInfo direction = new DirectoryInfo(path);
        DirectoryInfo[] folders = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
        return folders;
    }
    public static DirectoryInfo[] GetChildFolders(string path,string name)
    {
        return GetChildFolders(Path.Combine(path, name));
    }

    /// <summary>
    /// ��ȡ·���ļ����������ļ�
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static FileInfo[] GetChildFiles(string path)
    {
        if (!Exists(path)) return new FileInfo[] { };

        //�ļ�����һ����������ļ�
        //SearchOption.TopDirectoryOnly�����ѡ��ֻȡ��һ������ļ�
        //SearchOption.AllDirectories�����ѡ���ȡ�������е����ļ�
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        return files;
    }
    public static FileInfo[] GetChildFiles(string path, string name)
    {
        return GetChildFiles(Path.Combine(path, name));
    }

    /// <summary>
    /// ��ȡ�ļ����������ض���׺�ļ�
    /// </summary>
    /// <param name="path"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static FileInfo[] GetChildFilesWithSuffix(string path, string suffix)
    {
        List<FileInfo> fileInfos = new List<FileInfo>();
        foreach (var f in GetChildFiles(path))
        {
            if (f.Name.EndsWith(suffix))
            {
                fileInfos.Add(f);
            }
        }
        return fileInfos.ToArray();
    }
    public static FileInfo[] GetChildFilesWithSuffix(string path, string name,string suffix)
    {
        return GetChildFilesWithSuffix(Path.Combine(path, name),suffix);
    }

}
