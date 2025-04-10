using CustomInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
public class UIInitSetting : AssetPostprocessor
{
    /// <summary>
    /// 导入UI图片时自动设置属性
    /// </summary>
    public void OnPreprocessTexture()
    {
        string dirName = System.IO.Path.GetDirectoryName(assetPath);
        if (dirName.ToUpper().Contains("UI"))
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;
            textureImporter.wrapMode = TextureWrapMode.Clamp;
            textureImporter.mipmapEnabled = false;
        }
    }
}
#endif
