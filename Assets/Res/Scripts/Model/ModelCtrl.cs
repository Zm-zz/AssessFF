using HighlightPlus;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ModelCtrl : SingletonPatternMonoAutoBase<ModelCtrl>
{
    public Dictionary<string, InteractionModel> interactionDic = new Dictionary<string, InteractionModel>();

    public LayerMask layer;

    /// <summary>
    /// 当前点击的模型
    /// </summary>
    public InteractionModel currClickModel;
    public InteractionModel currEnterModel;

    public void Init()
    {
        List<InteractionModel> objectClickWithRays = transform.GetComponentsInChildren<InteractionModel>(true).ToList();

        foreach (var item in objectClickWithRays)
        {
            item.gameObject.layer = LayerMask.NameToLayer("Model");
            item.layer = layer;
            item.highlight = item.GetComponent<HighlightEffect>();

            interactionDic.Add(item.gameObject.name, item);
            Debug.Log(item.name);
        }

        Debug.Log($"<color=green>交互模型字典长度：</color>{interactionDic.Count}");
    }

    public InteractionModel Get(string key)
    {
        if (interactionDic.ContainsKey(key))
        {
            return interactionDic[key];
        }
        else
        {
            Debug.Log($"<color=red>交互模型字典中没有物体：</color>{key}");
            return null;
        }
    }

    /// <summary>
    /// 获取所有目前可互动的模型
    /// </summary>
    /// <returns></returns>
    public List<InteractionModel> GetInteractiveModels()
    {
        List<InteractionModel> models = new List<InteractionModel>();

        foreach (var item in interactionDic.Values)
        {
            if (item.enable)
            {
                models.Add(item);
            }
        }

        return models;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetInteractiveModelsHighlight(true);
        }
    }

    /// <summary>
    /// 设置所有可交互模型的高亮
    /// </summary>
    /// <param name="enable"></param>
    public void SetInteractiveModelsHighlight(bool enable)
    {
        foreach (var item in GetInteractiveModels())
        {
            item.highlight.SetHighlighted(enable);
        }
    }

    /// <summary>
    /// 设置某个物体的事件
    /// </summary>
    /// <param name="key"></param>
    /// <param name="actions"></param>
    public void SetClickByKey(string key, UnityAction[] actions = null, InteractionType type = InteractionType.None)
    {
        if (!interactionDic.ContainsKey(key))
        {
            Debug.Log($"<color=red>交互模型字典中没有物体：</color>{key}");
        }

        interactionDic[key].OnMouseDown.RemoveAllListeners();

        if (actions != null && type != InteractionType.None)
        {
            foreach (var action in actions)
            {
                switch (type)
                {
                    case InteractionType.ClickDown:
                        interactionDic[key].OnMouseDown.AddListener(action);
                        break;
                    case InteractionType.ClickUp:
                        interactionDic[key].OnMouseUp.AddListener(action);
                        break;
                    case InteractionType.Enter:
                        interactionDic[key].OnMouseEnter.AddListener(action);
                        break;
                    case InteractionType.Exit:
                        interactionDic[key].OnMouseExit.AddListener(action);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 设置某个物体的事件
    /// </summary>
    public void SetClickByKey(string key, UnityAction action = null, InteractionType type = InteractionType.None)
    {
        if (!interactionDic.ContainsKey(key))
        {
            Debug.Log($"<color=red>交互模型字典中没有物体：</color>{key}");
        }


        if (action != null && type != InteractionType.None)
        {
            switch (type)
            {
                case InteractionType.ClickDown:
                    interactionDic[key].OnMouseDown.RemoveAllListeners();
                    interactionDic[key].OnMouseDown.AddListener(action);
                    break;
                case InteractionType.ClickUp:
                    interactionDic[key].OnMouseUp.RemoveAllListeners();
                    interactionDic[key].OnMouseUp.AddListener(action);
                    break;
                case InteractionType.Enter:
                    interactionDic[key].OnMouseEnter.RemoveAllListeners();
                    interactionDic[key].OnMouseEnter.AddListener(action);
                    break;
                case InteractionType.Exit:
                    interactionDic[key].OnMouseExit.RemoveAllListeners();
                    interactionDic[key].OnMouseExit.AddListener(action);
                    break;
            }
        }
    }

    /// <summary>
    /// 给一些物体设置事件
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="action"></param>
    public void SetClickByKey(string[] keys, UnityAction action = null, InteractionType type = InteractionType.None)
    {
        foreach (var key in keys)
        {
            SetClickByKey(key, action, type);
        }
    }

    /// <summary>
    /// 给一些交互物体设置事件（常在事件）
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="action"></param>
    /// <param name="type"></param>
    public void SetClickByKeyToAlways(string[] keys, UnityAction action = null, InteractionType type = InteractionType.None)
    {
        foreach (var key in keys)
        {
            SetClickByKeyToAlways(key, action, type);
        }
    }

    public void SetClickByKeyToAlways(string key, UnityAction action = null, InteractionType type = InteractionType.None)
    {
        if (!interactionDic.ContainsKey(key))
        {
            Debug.Log($"<color=red>交互模型字典中没有物体：</color>{key}");
        }


        if (action != null && type != InteractionType.None)
        {
            switch (type)
            {
                case InteractionType.ClickDown:
                    interactionDic[key].AlwaysOnMouseDown.RemoveAllListeners();
                    interactionDic[key].AlwaysOnMouseDown.AddListener(action);
                    break;
                case InteractionType.ClickUp:
                    interactionDic[key].AlwaysOnMouseUp.RemoveAllListeners();
                    interactionDic[key].AlwaysOnMouseUp.AddListener(action);
                    break;
                case InteractionType.Enter:
                    interactionDic[key].AlwaysOnMouseEnter.RemoveAllListeners();
                    interactionDic[key].AlwaysOnMouseEnter.AddListener(action);
                    break;
                case InteractionType.Exit:
                    interactionDic[key].AlwaysOnMouseExit.RemoveAllListeners();
                    interactionDic[key].AlwaysOnMouseExit.AddListener(action);
                    break;
            }
        }
    }

    /// <summary>
    /// 开启交互物体的交互，同时打开其碰撞，其余关闭交互
    /// </summary>
    /// <param name="objs"></param>
    /// <param name="openHighLight">是否开启高光</param>
    public List<InteractionModel> OpenInteraction(string[] objs, bool openHighLight = true)
    {
        List<InteractionModel> models = new List<InteractionModel>();

        foreach (var key in interactionDic.Keys)
        {
            if (objs.Contains(key))
            {
                models.Add(interactionDic[key]);

                interactionDic[key].gameObject.SetActive(true);
                interactionDic[key].highlight.SetHighlighted(false);
                interactionDic[key].enable = true;
                interactionDic[key].enableHighLight = openHighLight;

                SetModelActive(key, true);
                SetModelCollider(key, true);
            }
            else
            {
                interactionDic[key].enable = false;
                SetModelCollider(key, false);
            }
        }

        return models;
    }

    /// <summary>
    /// 设置物体回到初始位置
    /// </summary>
    /// <param name="keys"></param>
    public void SetModelTransformToOriginal(string[] keys)
    {
        foreach (var key in keys)
        {
            if (interactionDic.ContainsKey(key))
            {
                interactionDic[key].transform.parent = interactionDic[key].trans_Parent;
                interactionDic[key].transform.localPosition = interactionDic[key].originalPos;
                interactionDic[key].transform.localRotation = interactionDic[key].originalRot;
            }
        }
    }

    /// <summary>
    /// 设置模型的碰撞
    /// </summary>
    /// <param name="key"></param>
    /// <param name="enable"></param>
    public void SetModelCollider(string key, bool enable)
    {
        if (interactionDic.ContainsKey(key))
        {
            interactionDic[key].GetComponent<Collider>().enabled = enable;
        }
    }


    /// <summary>
    /// 设置物体显示
    /// </summary>
    /// <param name="key"></param>
    /// <param name="active"></param>
    private void SetModelActive(string key, bool active)
    {
        if (interactionDic.ContainsKey(key))
        {
            interactionDic[key].gameObject.SetActive(active);
        }
        else
        {
            Debug.Log($"<color=red>交互模型字典中没有物体：</color>{key}");
        }
    }

    /// <summary>
    /// 打开交互物体显示
    /// </summary>
    /// <param name="objs"></param>
    public void OpenModel(string[] objs)
    {
        foreach (var obj in objs)
        {
            SetModelActive(obj, true);
        }
    }

    /// <summary>
    /// 打开物体显示，并关闭其他物体
    /// </summary>
    /// <param name="objs"></param>
    public void OpenModelAndCloseOther(string[] objs)
    {
        foreach (var key in interactionDic.Keys)
        {
            if (objs.Contains(key))
            {
                interactionDic[key].gameObject.SetActive(true);
            }
            else
            {
                interactionDic[key].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 关闭交互物体显示
    /// </summary>
    /// <param name="objs"></param>
    public void CloseModel(string[] objs)
    {
        foreach (var obj in objs)
        {
            SetModelActive(obj, false);
        }
    }

    /// <summary>
    /// 关闭交互物体交互
    /// </summary>
    /// <param name="objs"></param>
    public void CloseInteraction(string[] objs)
    {
        foreach (var key in interactionDic.Keys)
        {
            if (objs.Contains(key))
            {
                interactionDic[key].enable = false;
                interactionDic[key].enableHighLight = false;
                interactionDic[key].highlight.SetHighlighted(false);
            }
        }
    }

    /// <summary>
    /// 关闭所有交互物体
    /// </summary>
    public void CloseAllInteraction()
    {
        foreach (var key in interactionDic.Keys)
        {
            interactionDic[key].enable = false;
            interactionDic[key].enableHighLight = false;
            interactionDic[key].highlight.SetHighlighted(false);
        }
    }

}
