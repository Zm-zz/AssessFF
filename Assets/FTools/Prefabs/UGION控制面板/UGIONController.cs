using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ScriptExecutionOrder(3)]
public class UGIONController : MonoBehaviour
{
    public bool showLogo;
    public bool show��������;
    public bool showLicense = true;
    public GameObject[] logos;
    public GameObject obj_��������;
    public GameObject license;
    public Text text_�汾��;
    public Text text_���½�;
    public Text text_���½�;

    public GameObject panel_�������;
    public GameObject panel_�����ֲ�;

    private void Awake()
    {
        UGIONPanelOperation operation = UGIONPanelOperationPanel.Instance.LoadOperation();
        if (operation != null)
        {
            showLogo = operation.active_Logo;
            show�������� = operation.active_��������;
            showLicense = operation.active_License;
            text_�汾��.text = operation.str_�汾��;
            text_���½�.text = operation.str_���½�;
            text_���½�.text = operation.str_���½�;
        }
        else
        {
            Debug.Log("�½�UGION����̨");
            operation = new UGIONPanelOperation();
            operation.active_Logo = showLogo;
            operation.active_�������� = show��������;
            operation.active_License = showLicense;
            operation.str_�汾�� = text_�汾��.text;
            operation.str_���½� = text_���½�.text;
            operation.str_���½� = text_���½�.text;
            UGIONPanelOperationPanel.Instance.SaveOperation(operation);
        }

        foreach (var logo in logos)
        {
            logo.SetActive(showLogo);
        }
        obj_��������.SetActive(show��������);
        // license.SetActive(showLicense);
    }

    private void Update()
    {
        if (CheatingInstructions.CompareInstruction("UgionOperation"))
        {
            panel_�������.SetActive(true);
            UGIONPanelOperationPanel.Instance.SetOperationPanelData();
        }
        if (CheatingInstructions.CompareInstruction("UgionDictionary"))
        {
            panel_�����ֲ�.SetActive(true);
        }
    }

    [InspectorButton("�ֶ�����")]
    void SaveInUnity()
    {
        UGIONPanelOperation operation = new UGIONPanelOperation();
        operation.active_Logo = showLogo;
        operation.active_�������� = show��������;
        operation.active_License = showLicense;
        operation.str_�汾�� = text_�汾��.text;
        operation.str_���½� = text_���½�.text;
        operation.str_���½� = text_���½�.text;
        UGIONPanelOperationPanel.Instance.SaveOperation(operation);
    }
}
