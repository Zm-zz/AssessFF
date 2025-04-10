using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//                 _ooOoo_
//                o8888888o
//                88" . "88
//                (| -_- |)
//                O\  =  /O
//             ____/`---'\____
//           .'  \\|     |//  `.
//          /  \\|||  :  |||//  \
//         / _ ||||| -:- ||||| - \
//         |   | \\\  -  /// |   |
//         | \_|  ''\---/ '' |_/ |
//         \  .-\__  `-`  __/-.  /
//       ___`. .'  /--.--\  `. . ___
//    ."" '<  `.___\_<|>_/___.'  > '"".
//   | | :  `- \`.;`\ _ /`;.`/ -`  : | |
//   \  \ `-.   \_ __\ /__ _/   .-` /  /
//====`-.____`-.___\_____/___.-`____.-'======
//                 `=---='
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//          ����������       �����ӹ���

public class MenuManager : MonoBehaviour
{
    private Transform trans_Parent;

    [BoxGroup("����Ԥ��")] public GameObject pre_ParMenu;
    [BoxGroup("����Ԥ��")] public GameObject pre_SubPar;
    [BoxGroup("��ҪԤ��")] public GameObject pre_SpreadOption;
    [BoxGroup("��ҪԤ��")] public GameObject pre_SubMenu;

    [BoxGroup("������")] public ProcedureData procedureData;
    [BoxGroup("������")][SerializeField] private bool _IsDefaultEnter;

    [ReadOnly][ShowInInspector][BoxGroup("ʵʱ׷������")] private ProcedureConfig currentProcedure;
    [ReadOnly][ShowInInspector][BoxGroup("ʵʱ׷������")] private int currentMainIndex = -1;
    [ReadOnly][ShowInInspector][BoxGroup("ʵʱ׷������")] private int currentSubIndex = -1;

    [ReadOnly][ShowInInspector][BoxGroup("��̬����")] private List<GameObject> spreadOptions = new List<GameObject>();
    [ReadOnly][ShowInInspector][BoxGroup("��̬����")] private Dictionary<OptionBase, List<OptionBase>> options = new Dictionary<OptionBase, List<OptionBase>>();

    // ����������ʶ
    private int mainIndex = 0;
    private int subIndex = 0;

    public void Initialize()
    {
        trans_Parent = GameObject.Find("MainCanvas/Panel_Menu/Scroll Vertical/Viewport/Content").transform;

        InitOptions(procedureData);
    }

    private void InitOptions(ProcedureData procedureData)
    {
        currentMainIndex = -1;
        currentSubIndex = -1;

        UnLoadAllOptions();
        LoadAllOptions(procedureData);

        if (_IsDefaultEnter)
        {
            EnterProcedure(options.Keys.First());
        }
    }

    private void LoadAllOptions(ProcedureData procedureData)
    {
        mainIndex = 0;
        subIndex = 0;

        procedureData.Procedures.ForEach(i =>
        {
            CreateOption(i, trans_Parent);
        });
    }

    /*  private void CreateOption(ProcedureInfo procedureInfo, Transform parent, UnityAction clickAction = null)
      {
          OptionBase option = Instantiate(pre_ParMenu, parent).GetComponent<OptionBase>();
          option.gameObject.name = $"Main_{procedureInfo.ProcedureConfig.procedureTitle}";
          option.Initialize(this, procedureInfo, mainIndex);
          mainIndex++;

          options.Add(option, new List<OptionBase>());

          if (procedureInfo.hasExtension)
          {
              GameObject subPar = Instantiate(pre_SubPar, parent);
              subPar.name = $"SubParent_{procedureInfo.ProcedureConfig.procedureTitle}";
              option.extensionMenu = subPar.transform;

              foreach (var config in option.procedureInfo.extendedProcedures)
              {
                  OptionBase subOption = Instantiate(pre_SubMenu, subPar.transform).GetComponent<OptionBase>();
                  Debug.Log(config);
                  Debug.Log(subOption);
                  subOption.gameObject.name = $"Sub_{config.procedureTitle}";
                  ProcedureInfo info = new ProcedureInfo(config, false, null);
                  subOption.Initialize(this, info, subIndex);
                  subIndex++;

                  if (options.ContainsKey(option))
                  {
                      options[option].Add(subOption);
                  }
                  else
                  {
                      options.Add(option, new List<OptionBase>() { subOption });
                  }
              }

              subPar.SetActive(false);
          }
      }*/

    private void CreateOption(ProcedureInfo procedureInfo, Transform parent)
    {
        GameObject spreadOption = Instantiate(pre_SpreadOption, parent);
        spreadOption.name = $"Option_{procedureInfo.ProcedureConfig.procedureTitle}";
        spreadOptions.Add(spreadOption);

        OptionBase option = spreadOption.GetComponentInChildren<OptionBase>();
        option.gameObject.name = $"Main_{procedureInfo.ProcedureConfig.procedureTitle}";
        option.Initialize(this, procedureInfo, mainIndex++);

        options.Add(option, new List<OptionBase>());

        Transform subSpread = spreadOption.transform.Find("Sub Spread");

        if (procedureInfo.hasExtension)
        {
            subSpread.GetComponent<FlexSubOptions>().preferredHeight = option.procedureInfo.extendedProcedures.Count * pre_SubMenu.GetComponent<RectTransform>().rect.height; ;

            foreach (var config in option.procedureInfo.extendedProcedures)
            {
                OptionBase subOption = Instantiate(pre_SubMenu, subSpread).GetComponent<OptionBase>();
                RectTransform subRect = subOption.GetComponent<RectTransform>();
                subOption.gameObject.name = $"Sub_{config.procedureTitle}";

                if (subSpread.childCount > 1)
                {
                    Vector3 lastPos = subSpread.GetChild(subSpread.childCount - 2).GetComponent<RectTransform>().position;
                    Vector3 newPos = new Vector3(lastPos.x, lastPos.y - subRect.rect.height, 0);
                    subRect.position = newPos;
                }

                ProcedureInfo info = new ProcedureInfo(config, false, null);
                subOption.Initialize(this, info, subIndex++);

                if (options.ContainsKey(option))
                {
                    options[option].Add(subOption);
                }
                else
                {
                    options.Add(option, new List<OptionBase>() { subOption });
                }
            }
        }
    }

    public void UnLoadAllOptions()
    {
        // ʹ��HashSet�����ظ�����
        var allOptions = new HashSet<GameObject>();

        // ������ѡ��
        foreach (var subList in options.Values)
        {
            if (subList == null) continue;

            foreach (var option in subList)
            {
                if (option != null) allOptions.Add(option.gameObject);
            }
        }

        // ������ѡ��
        foreach (var mainOption in options.Keys)
        {
            if (mainOption != null) allOptions.Add(mainOption.gameObject);
        }

        // ȫ����
        foreach (var spreadOption in spreadOptions)
        {
            if (spreadOption != null) allOptions.Add(spreadOption);
        }

        // ͳһ����
        foreach (var option in allOptions)
        {
            if (option.gameObject == null) continue;

            if (Application.isPlaying)
            {
                Destroy(option.gameObject);
            }
            else
            {
                DestroyImmediate(option.gameObject);
            }
        }

        options.Clear();
        spreadOptions.Clear();
    }

    private bool JudgeMainProcedure(OptionBase option, out OptionBase mainProcedure)
    {
        // ����Ƿ���������(�ֵ��)
        if (options.TryGetValue(option, out _))
        {
            mainProcedure = option;
            return true;
        }

        // �������̣�out�����̵�������
        mainProcedure = options.FirstOrDefault(kv => kv.Value?.Contains(option) == true).Key;
        return false;
    }

    public void ChangeState(OptionBase currOption)
    {
        bool isMain = JudgeMainProcedure(currOption, out OptionBase mainProcedure);

        // �ر�������ѡ�е�����
        if (isMain)
        {
            // �����ظ�ѡ
            foreach (var option in options.Keys.Where(o => o == currOption && o.Bool_IsOn))
            {
                return;
            }

            // ģʽ������
            if (GlobalManager.Instance.GameMode == GameMode.Exam && currOption.index < currentMainIndex) return;

            // �ر����������̼���������
            foreach (var option in options.Keys.Where(o => o != currOption && o.Bool_IsOn))
            {
                CloseOptionWithSubs(option);
            }
        }
        else
        {
            // �����ظ�ѡ
            foreach (var option in options[mainProcedure].Where(o => o == currOption && o.Bool_IsOn))
            {
                return;
            }

            if (GlobalManager.Instance.GameMode == GameMode.Exam && currOption.index < currentSubIndex) return;

            // �رյ�ǰ�������µ�����������
            foreach (var option in options[mainProcedure].Where(o => o != currOption && o.Bool_IsOn))
            {
                CloseOption(option);
            }
        }

        // ���õ�ǰѡ��
        currOption.ChangeState(true);
        // ������չ��һ��
        if (currOption.procedureInfo.hasExtension)
        {
            options[mainProcedure].First().ChangeState(true);
            currentSubIndex = options[mainProcedure].First().index;
        }

        if (isMain)
            currentMainIndex = currOption.index;
        else
            currentSubIndex = currOption.index;
    }

    private void CloseOptionWithSubs(OptionBase option)
    {
        if (option.procedureInfo.hasExtension)
        {
            options[option].ForEach(o =>
            {
                if (o.Bool_IsOn)
                {
                    CloseOption(o);
                }
            });
        }

        CloseOption(option);
    }

    private void CloseOption(OptionBase option)
    {
        option.ChangeState(false);
    }

    /// <summary>
    /// �Զ��忪��ѡ��
    /// </summary>
    private void EnterProcedure(OptionBase option)
    {
        bool isMain = JudgeMainProcedure(option, out OptionBase _Main);

        if (isMain && option.procedureInfo.hasExtension)
        {
            ChangeState(option);
            ChangeState(options[option].First());
        }
        else if (isMain && !option.procedureInfo.hasExtension)
        {
            ChangeState(option);
        }
        else if (!isMain)
        {
            ChangeState(_Main);
            ChangeState(option);
        }
    }

    /// <summary>
    /// ��ܽ�������
    /// </summary>
    public void ChangeProcedure(ProcedureConfig config)
    {
        GlobalManager.Instance.ChangeProcedure(config.procedureName);
        currentProcedure = config;

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
