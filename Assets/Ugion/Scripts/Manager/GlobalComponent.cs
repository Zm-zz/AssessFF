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

public class GlobalComponent : SingletonPatternMonoBase<GlobalComponent>
{
    public GameMode GameMode = GameMode.Train;

    public ProcedureData procedureData;

    public FTaskManager TaskManager { get; private set; }
    public MenuManager MenuManager { get; private set; }

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
        TaskManager = FindObjectOfType<FTaskManager>();
        MenuManager = FindObjectOfType<MenuManager>();

        InitManagers();
    }

    private void InitManagers()
    {
        TaskManager.InitTask();
        MenuManager.Initialize(procedureData);
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
