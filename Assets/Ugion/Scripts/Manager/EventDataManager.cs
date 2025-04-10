//using UnityEngine;

//public class EventDataManager : MonoBehaviour
//{
//    public void Initialize()
//    {
//        Main.Event.Subscribe(Event_ChangeState.EventId, OnEvent_ChangeStateFire);
//    }

//    private void OnEvent_ChangeStateFire(object sender, GameEventArgs e)
//    {
//        Event_ChangeState ne = (Event_ChangeState)e;

//        if (ne != null)
//        {
//            GlobalManager.Instance.ChangeProcedure(ne.ProcedureName);
//            Debug.Log($"<color=orange>ChangeProcedure��</color>{ne.ProcedureTitle}");
//        }
//    }

//    public void UnInitialize()
//    {
//        Main.Event.Unsubscribe(Event_ChangeState.EventId, OnEvent_ChangeStateFire);
//    }
//}

//public class Event_ChangeState : GameEventArgs
//{
//    public static readonly int EventId = typeof(TestEvent).GetHashCode();

//    /// <summary>
//    /// ��ʼ������ȫ�����óɹ��¼���ŵ���ʵ����
//    /// </summary>
//    public Event_ChangeState()
//    {
//    }

//    public Event_ChangeState(ProcedureConfig config)
//    {
//        ProcedureName = config.procedureName;
//        ProcedureTitle = config.procedureTitle;
//    }

//    public override int Id
//    {
//        get { return EventId; }
//    }

//    public string ProcedureName
//    {
//        get;
//        private set;
//    }

//    public string ProcedureTitle
//    {
//        get;
//        private set;
//    }

//    /// <summary>
//    /// ��������ȫ�����óɹ��¼���
//    /// </summary>
//    /// <param name="e">�ڲ��¼���</param>
//    /// <returns>�����ļ���ȫ�����óɹ��¼���</returns>
//    public static Event_ChangeState Create(Event_ChangeState e)
//    {
//        Event_ChangeState changeStateEvent = ReferencePool.Acquire<Event_ChangeState>();
//        changeStateEvent.ProcedureName = e.ProcedureName;
//        changeStateEvent.ProcedureTitle = e.ProcedureTitle;
//        return changeStateEvent;
//    }

//    public override void Clear()
//    {

//    }
//}