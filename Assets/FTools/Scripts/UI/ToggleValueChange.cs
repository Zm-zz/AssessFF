
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 开关控件切换状态变化
/// </summary>
[RequireComponent(typeof(Toggle))]
public class ToggleValueChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Toggle toggle;

    public UnityEvent eventOn;
    public UnityEvent eventOff;

    public void OnValueChange()
    {
        if (toggle.isOn)
        {
            eventOn?.Invoke();
        }
        else
        {
            eventOff?.Invoke();
        }
    }

    [Header("开关图片变化")]
    public Image image;
    bool showImage() => image != null;
    public Sprite sprite_off;
    public Sprite sprite_on;
    public Sprite sprite_highlight_on;
    public Sprite sprite_highlight_off;
    public Sprite sprite_disable;

    [Header("关联文字颜色变化")]
    public Text text;
    bool showText() => text != null;
    public Color textColor_off;
    public Color textColor_on;
    public Color textColor_highlight_on;
    public Color textColor_highlight_off;
    public Color textColor_disable;

    [Header("勾选图片显示")]
    public Image checkImg_single;

    [Header("勾选图片切换")]
    public Image checkImg_double;
    bool showSwitch() => checkImg_double != null;
    public Sprite check_on;
    public Sprite check_off;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.transition = Selectable.Transition.None;
        if (image == null) image = GetComponent<Image>();
        //toggle.onValueChanged.AddListener((bool b) => { OnValueChange(); });
    }

    //private void Reset()
    //{
    //    Awake();
    //}

    private void Update()
    {
        if (toggle.interactable == false)
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
            if (toggle.isOn)
            {
                if (image != null && sprite_highlight_on != null)
                {
                    image.sprite = sprite_highlight_on;
                }
                else if (image != null && sprite_on != null)
                {
                    image.sprite = sprite_on;
                }
                if (text != null && textColor_highlight_on.a != 0)
                {
                    text.color = textColor_highlight_on;
                }
                else if (text != null && textColor_on.a != 0)
                {
                    text.color = textColor_on;
                }
                if (checkImg_single != null)
                {
                    checkImg_single.gameObject.SetActive(true);
                }
                if (checkImg_double != null && check_on != null)
                {
                    checkImg_double.sprite = check_on;
                    checkImg_double.SetNativeSize();
                }
            }
            else
            {
                if (image != null && sprite_highlight_off != null)
                {
                    image.sprite = sprite_highlight_off;
                }
                else if (image != null && sprite_off != null)
                {
                    image.sprite = sprite_off;
                }
                if (text != null && textColor_highlight_off.a != 0)
                {
                    text.color = textColor_highlight_off;
                }
                else if (text != null && textColor_off.a != 0)
                {
                    text.color = textColor_off;
                }
                if (checkImg_single != null)
                {
                    checkImg_single.gameObject.SetActive(false);
                }
                if (checkImg_double != null && check_on != null)
                {
                    checkImg_double.sprite = check_off;
                    checkImg_double.SetNativeSize();
                }
            }
        }
        else
        {
            if (toggle.isOn)
            {
                if (image != null && sprite_on != null)
                {
                    image.sprite = sprite_on;
                }
                if (text != null && textColor_on.a != 0)
                {
                    text.color = textColor_on;
                }
                if (checkImg_single != null)
                {
                    checkImg_single.gameObject.SetActive(true);
                }
                if (checkImg_double != null && check_on != null)
                {
                    checkImg_double.sprite = check_on;
                    checkImg_double.SetNativeSize();
                }
            }
            else
            {
                if (image != null && sprite_off != null)
                {
                    image.sprite = sprite_off;
                }
                if (text != null && textColor_off.a != 0)
                {
                    text.color = textColor_off;
                }
                if (checkImg_single != null)
                {
                    checkImg_single.gameObject.SetActive(false);
                }
                if (checkImg_double != null && check_on != null)
                {
                    checkImg_double.sprite = check_off;
                    checkImg_double.SetNativeSize();
                }
            }
        }
    }

    bool isIn;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isIn = false;
    }

    private void OnEnable()
    {
        isIn = false;
    }
}
