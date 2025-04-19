using Sirenix.OdinInspector;
using UnityEditor.ShortcutManagement;
using UnityEngine;

/***
 **************************************************************
 *                                                            *
 *   .=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-.       *
 *    |                     ______                     |      *
 *    |                  .-"      "-.                  |      *
 *    |                 /            \                 |      *
 *    |     _          |              |          _     |      *
 *    |    ( \         |,  .-.  .-.  ,|         / )    |      *
 *    |     > "=._     | )(__/  \__)( |     _.=" <     |      *
 *    |    (_/"=._"=._ |/     /\     \| _.="_.="\_)    |      *
 *    |           "=._"(_     ^^     _)"_.="           |      *
 *    |               "=\__|IIIIII|__/="               |      *
 *    |              _.="| \IIIIII/ |"=._              |      *
 *    |    _     _.="_.="\          /"=._"=._     _    |      *
 *    |   ( \_.="_.="     `--------`     "=._"=._/ )   |      *
 *    |    > _.="                            "=._ <    |      *
 *    |   (_/                                    \_)   |      *
 *    |                                                |      *
 *    '-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-='      *
 *                                                            *
 *             LOOKING MY EYES    TELL ME WHY                 *
 **************************************************************
 */

public class GlobalManager : SingletonPatternMonoBase<GlobalManager>
{
    public GameMode GameMode = GameMode.Train;

    [ReadOnly][SerializeField] private FTaskManager _TaskManager;
    [ReadOnly][SerializeField] private MenuManager _MenuManager;

    private void Start()
    {
        Launch();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Robot.Instance.ShowTips("Çë×ÐÏ¸²éÔÄ²¡ÀýÐÅÏ¢");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Robot.Instance.HideAllDialog();
        }
    }

    public void Launch()
    {
        _TaskManager = FindObjectOfType<FTaskManager>();
        _MenuManager = FindObjectOfType<MenuManager>();

        InitManagers();
    }

    private void InitManagers()
    {
        _TaskManager.InitTask();
        _MenuManager.Initialize();
        Robot.Instance.Initialize();
    }

    public void ChangeProcedure(string procedureName)
    {
        FTaskManager.Instance.EnterTask(procedureName);
    }
}

public enum GameMode
{
    Train,
    Exam,
}
