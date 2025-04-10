using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CameraControl : SingletonPatternMonoBase<CameraControl>
{
    [Header("��λ����")]
    public Transform pointsParent;

    private CameraBasicMove motion;

    public Dictionary<string, Transform> pointsDic = new Dictionary<string, Transform>();

    private void Awake()
    {
        List<Transform> points = pointsParent.GetComponentsInChildren<Transform>(true).ToList();

        foreach (Transform t in points)
        {
            pointsDic.Add(t.name, t);
        }

        motion = GetComponent<CameraBasicMove>();
    }



    /// <summary>
    /// ����ƶ���Ŀ��λ��
    /// </summary>
    /// <param name="pointName"></param>
    /// <param name="motionTime"></param>
    /// <param name="action"></param>
    /// <param name="canMove"></param>
    /// <param name="canLift"></param>
    public void MoveToTarget(string pointName, float motionTime = 0, UnityAction action = null, bool canMove = true, bool canLift = true)
    {
        if (pointsDic.ContainsKey(pointName))
        {
            MoveToTarget(pointsDic[pointName], motionTime, action, canMove, canLift);
        }
        else
        {
            Debug.Log($"<color=red>�ӽ���û�У�</color>{pointName}");
        }
    }

    /// <summary>
    /// �ƶ���Ŀ��Ϊֹ������վ���Ժ�����¼�
    /// </summary>
    /// <param name="target"></param>
    /// <param name="moveEvent"></param>
    public void MoveToTarget(Transform target, float motionTime, UnityAction moveEvent = null, bool canMove = true, bool canLift = true)
    {
        // ��ֹ��������������Tween
        int tweenNumber = DOTween.Kill(transform);

        motion.canCameraMove = false;
        motion.canLifting = false;

        transform.DORotate(target.rotation.eulerAngles, motionTime);

        transform.DOMove(target.position, motionTime).OnComplete(() =>
        {
            motion.canCameraMove = canMove;
            motion.canLifting = canLift;

            moveEvent?.Invoke();
        });
    }

}
