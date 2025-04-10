using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceExample : MonoBehaviour
{
    public ChoiceManager choiceManager;
    int choice = 1;

    public void SetChoice()
    {
        switch (choice)
        {
            case 1:
                SetChoice1();
                break;
            case 2:
                SetChoice2();
                break;
            case 3:
                SetChoice3();
                break;
            case 4:
                SetChoice4();
                break;
        }
        choice++;
        if(choice > 4)
        {
            choice = 1;
        }
    }

    public void SetChoice1()
    {
        choiceManager.SetChoice(new ChoiceInfo(
            "��ѡ������",
            new string[] {"ѡ��1","ѡ��2","ѡ��3"},
            2, true
            ),
            (bool b) => { Debug.Log($"��ȷ��2���ش�{b}"); },
            () => { choiceManager.SetActive(false); },
            true);
    }

    public void SetChoice2()
    {
        choiceManager.SetChoice(new ChoiceInfo(
            "��ѡ�ⲻ����",
            new string[] { "ѡ��1", "ѡ��2", "ѡ��3" },
            2, true
            ),
            (bool b) => { Debug.Log($"��ȷ��2���ش�{b}"); },
            () => { choiceManager.SetActive(false); },
            false);
    }

    public void SetChoice3()
    {
        choiceManager.SetChoice(new ChoiceInfo(
            "��ѡ������",
            new string[] { "ѡ��1", "ѡ��2", "ѡ��3" },
            23, false
            ),
            (bool b) => { Debug.Log($"��ȷ��23���ش�{b}"); },
            () => { choiceManager.SetActive(false); },
            true);
    }

    public void SetChoice4()
    {
        choiceManager.SetChoice(new ChoiceInfo(
            "��ѡ�ⲻ����",
            new string[] { "ѡ��1", "ѡ��2", "ѡ��3" },
            23, false
            ),
            (bool b) => { Debug.Log($"��ȷ��23���ش�{b}"); },
            () => { choiceManager.SetActive(false); },
            false);
    }
}
