using HighlightPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class HideAndTag : MonoBehaviour
{
    public static bool isOn;
    public Toggle tog_͸��;
    public BoxPop TagBox;
    Toggle tog;

    GameObject curMuscle;
    List<GameObject> hideMuscleList;
    Vector3 currentMousePos;

    private void Awake()
    {
        tog = GetComponent<Toggle>();
        tog.onValueChanged.AddListener(OnValueChange);
        hideMuscleList = new List<GameObject>();
        TagBox.HideImmediately();
    }

    void OnValueChange(bool b)
    {
        if (b)
        {
            isOn = true;
            tog_͸��.isOn = true;
            tog_͸��.enabled = false;
        }
        else
        {
            isOn = false;
            tog_͸��.isOn = false;
            tog_͸��.enabled = true;
            ShutHide();
        }
    }

    void CanHide()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitting = Physics.Raycast(ray, out hit, 9999, LayerMask.GetMask("Muscle"));

        //���������ʧ
        if (Input.GetMouseButtonDown(0))
        {
            if (curMuscle != null)
            {
                curMuscle.SetActive(false);
                hideMuscleList.Add(curMuscle);
                curMuscle.GetComponent<HighlightEffect>().highlighted = false;
                curMuscle = null;
            }
            TagBox.Hide();
        }

        //����Ƿ�����һ֡�ļ���
        if (hitting)
        {
            GameObject obj = hit.collider.gameObject;
            if (curMuscle != null && obj == curMuscle)
            {
                if (currentMousePos != Input.mousePosition)
                {
                    TagBox.SetBox(Input.mousePosition);
                }
                return;
            }
        }

        if (curMuscle != null)
        {
            curMuscle.GetComponent<HighlightEffect>().highlighted = false;
            curMuscle = null;
        }
        TagBox.Hide();

        //ˢ�¼�������Ч��
        if (hitting)
        {
            GameObject obj = hit.collider.gameObject;
            curMuscle = obj;
            curMuscle.GetComponent<HighlightEffect>().highlighted = true;

            TagBox.SetBox(Input.mousePosition, curMuscle.name.ToString());
            currentMousePos = Input.mousePosition;
        }
    }

    void ShutHide()
    {
        foreach (var o in hideMuscleList)
        {
            o.SetActive(true);
        }
        hideMuscleList.Clear();
        TagBox.HideImmediately();
        if (curMuscle != null)
        {
            curMuscle.GetComponent<HighlightEffect>().highlighted = false;
            curMuscle = null;
        }
    }

    private void Update()
    {
        if (tog.isOn)
        {
            CanHide();
        }
    }
}
