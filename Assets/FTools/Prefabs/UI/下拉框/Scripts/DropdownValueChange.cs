using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownValueChange : MonoBehaviour
{
    string ValueStr = "";
    public string valueStr { get { return ValueStr; }set { ValueStr = value;showText.text = value; } }

    [SerializeField] Toggle dropTog;
    [SerializeField] Text showText;
    [SerializeField] GameObject prefab_item;
    [SerializeField] GameObject panel_选项框;

    public List<string> itemValues;

    private void Awake()
    {
        //EventCenterManager.AddListener(TaskManager.Instance.command_Init, Init);
        dropTog.onValueChanged.AddListener(OnValueChange);
    }

    void Init()
    {
        //showText.text = "<color=#8A8A8A>未选择</color>";
        //valueStr = "未选择";
    }

    public void OnClick_Item(GameObject obj)
    {
        showText.text = obj.name;
        valueStr = obj.name;
        dropTog.isOn = false;
    }

    public void OnValueChange(bool b)
    {
        panel_选项框.SetActive(b);
        Debug.Log(GetComponent<RectTransform>().rect.height);
        panel_选项框.transform.localPosition = new Vector3(0, -GetComponent<RectTransform>().rect.height/2 , 0);
        prefab_item.SetActive(true);
        for (int i = panel_选项框.transform.childCount - 1; i > 0; i--)
        {
            Destroy(panel_选项框.transform.GetChild(i).gameObject);
        }
        bool isNull = false;
        if(itemValues.Count == 0)
        {
            itemValues.Add("");
            isNull = true;
        }
        foreach (var i in itemValues)
        {
            GameObject item = Instantiate(prefab_item, panel_选项框.transform);
            item.name = i;
            item.GetComponentInChildren<Text>().text = i;
            item.GetComponent<Button>().onClick.AddListener(() => { OnClick_Item(item); });
        }
        if (isNull) itemValues.Clear();
        prefab_item.SetActive(false);
    }

    private void OnDisable()
    {
        dropTog.isOn = false;
    }

    private void OnEnable()
    {
        dropTog.isOn = false;
        panel_选项框.SetActive(false);
    }

    public int GetValue()
    {
        for(int i = 0;i<itemValues.Count;i++)
        {
            if (valueStr == itemValues[i]) return i;
        }
        return -1;
    }
}
