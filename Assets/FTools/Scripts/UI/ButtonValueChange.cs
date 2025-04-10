using CustomInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonValueChange : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    private Button button;

    [Header("开关图片变化")]
    public Image image;
    bool showImage() => image != null;
    [ShowIf(nameof(showImage))] public Sprite sprite_normal;
    [ShowIf(nameof(showImage))] public Sprite sprite_enter;
    [ShowIf(nameof(showImage))] public Sprite sprite_press;
    [ShowIf(nameof(showImage))] public Sprite sprite_disable;

    [Header("关联文字颜色变化")]
    public Text text;
    bool showText() => text != null;
    [ShowIf(nameof(showText))] public Color textColor_normal;
    [ShowIf(nameof(showText))] public Color textColor_enter;
    [ShowIf(nameof(showText))] public Color textColor_press;
    [ShowIf(nameof(showText))] public Color textColor_disable;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.transition = Selectable.Transition.None;
        Navigation na = new Navigation();
        na.mode = Navigation.Mode.None;
        button.navigation = na;
        if (image == null) image = GetComponent<Image>();
    }

    private void Reset()
    {
        Awake();
    }

    private void Update()
    {
        if (button.interactable == false)
        {
            if (image != null && sprite_disable != null)
            {
                image.sprite = sprite_disable;
            }
            if (text != null && textColor_disable.a != 0)
            {
                text.color = textColor_disable;
            }
            return;
        }
        if (isIn)
        {
            if (isDown)
            {
                if (button.interactable == false) return;
                if (image != null && sprite_press != null)
                {
                    image.sprite = sprite_press;
                }
                if (text != null && textColor_press.a != 0)
                {
                    text.color = textColor_press;
                }
            }
            else
            {
                if (button.interactable == false) return;
                if (image != null && sprite_enter != null)
                {
                    image.sprite = sprite_enter;
                }
                if (text != null && textColor_enter.a != 0)
                {
                    text.color = textColor_enter;
                }
            }
        }
        else
        {
            if (isDown)
            {
                if (button.interactable == false) return;
                if (image != null && sprite_press != null)
                {
                    image.sprite = sprite_press;
                }
                if (text != null && textColor_press.a != 0)
                {
                    text.color = textColor_press;
                }
            }
            else
            {
                if (button.interactable == false) return;
                if (image != null && sprite_normal != null)
                {
                    image.sprite = sprite_normal;
                }
                if (text != null && textColor_normal.a != 0)
                {
                    text.color = textColor_normal;
                }
            }
        }
    }

    bool isIn;
    bool isDown;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isIn = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isIn)
        {
            isDown = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }

    private void OnEnable()
    {
        isIn = false;
        isDown = false;
    }
}
