using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextureColorData", menuName = "FDatas/TextureColorData")]
[Serializable]
public class TextureColorData : ScriptableObject
{
    [SerializeField]
    public List<TextureColor> colorList = new List<TextureColor>();
}

[Serializable]
public class TextureColor
{
    public string name;
    public Color color;
}