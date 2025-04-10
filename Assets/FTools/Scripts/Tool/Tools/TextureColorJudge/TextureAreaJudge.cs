using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureAreaJudge : MonoBehaviour
{
    public Texture texture;
    public TextureColorData colorJudge;

    public string GetArea(RaycastHit hit)
    {
        if (hit.collider == null) return null;
        Vector2 textureCoord = hit.textureCoord;
        Renderer renderer = hit.collider.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (texture is Texture2D)
            {
                Texture2D texture2D = (Texture2D)texture;
                float textureX = textureCoord.x * texture2D.width;
                float textureY = textureCoord.y * texture2D.height;
                Color pixelColor = texture2D.GetPixel((int)textureX, (int)textureY);
                if (pixelColor != Color.white)
                {
                    TextureColor myColor = JudgeColor(pixelColor);
                    if (myColor == null)
                    {
                        return null;
                    }
                    return myColor.name.ToString();
                }
            }
        }
        return null;
    }

    //先找到每个区域的颜色，创建TextureColorData录入颜色信息
    public void GetColor(RaycastHit hit)
    {
        if (hit.collider == null) return;
        Vector2 textureCoord = hit.textureCoord;
        Renderer renderer = hit.collider.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (texture is Texture2D)
            {
                Texture2D texture2D = (Texture2D)texture;
                float textureX = textureCoord.x * texture2D.width;
                float textureY = textureCoord.y * texture2D.height;
                Color pixelColor = texture2D.GetPixel((int)textureX, (int)textureY);
                Debug.Log(texture.name + pixelColor);
            }
        }
    }

    static bool ColorsAreClose(Color color1, Color color2, float threshold = 0.005f)
    {
        float sqrDistance = (color1.r - color2.r) * (color1.r - color2.r) +
                            (color1.g - color2.g) * (color1.g - color2.g) +
                            (color1.b - color2.b) * (color1.b - color2.b);
        return sqrDistance < threshold * threshold;
    }

    TextureColor JudgeColor(Color color)
    {
        foreach (TextureColor mycolor in colorJudge.colorList)
        {
            if (ColorsAreClose(color, mycolor.color))
            {
                return mycolor;
            }
        }
        return null;
    }
}
