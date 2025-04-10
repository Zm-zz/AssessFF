using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Unity.VisualScripting;

public class FileManager
{
    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="filePath">文件夹路径</param>
    /// <param name="fileName">文件夹名</param>
    public static void CreatFolder(string filePath,string fileName)
    {
        var file = Path.Combine(filePath, fileName);
        //判断文件夹路径是否存在
        if (!Directory.Exists(file))
        {
            //创建
            Directory.CreateDirectory(file);
        }
    }
    public static void CreatFolder(string filePath)
    {
        //判断文件夹路径是否存在
        if (!Directory.Exists(filePath))
        {
            //创建
            Directory.CreateDirectory(filePath);
        }
    }



    /// <summary>
    /// 通过路径查找文件是否存在
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
    /// 路径删除文件
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
    /// 获取路径文件夹下所有文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static DirectoryInfo[] GetChildFolders(string path)
    {
        if (!Exists(path)) return new DirectoryInfo[] { };

        //文件夹下一层的所有文件夹
        //SearchOption.TopDirectoryOnly：这个选项只取下一层的子文件
        //SearchOption.AllDirectories：这个选项会取其下所有的子文件
        DirectoryInfo direction = new DirectoryInfo(path);
        DirectoryInfo[] folders = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
        return folders;
    }
    public static DirectoryInfo[] GetChildFolders(string path,string name)
    {
        return GetChildFolders(Path.Combine(path, name));
    }

    /// <summary>
    /// 获取路径文件夹下所有文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static FileInfo[] GetChildFiles(string path)
    {
        if (!Exists(path)) return new FileInfo[] { };

        //文件夹下一层的所有子文件
        //SearchOption.TopDirectoryOnly：这个选项只取下一层的子文件
        //SearchOption.AllDirectories：这个选项会取其下所有的子文件
        DirectoryInfo direction = new DirectoryInfo(path);
        FileInfo[] files = direction.GetFiles("*", SearchOption.TopDirectoryOnly);
        return files;
    }
    public static FileInfo[] GetChildFiles(string path, string name)
    {
        return GetChildFiles(Path.Combine(path, name));
    }

    /// <summary>
    /// 获取文件夹下所有特定后缀文件
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
