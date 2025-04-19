using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CameraBasicMove))]
public class CameraControl : SingletonPatternMonoBase<CameraControl>
{
    [Header("点位父类")]
    public Transform pointsParent;

    private CameraBasicMove motion;

    public Dictionary<string, Transform> pointsDic = new Dictionary<string, Transform>();

    private void Awake()
    {
        InitializeCameraSet();

        motion = GetComponent<CameraBasicMove>();
    }

    private void InitializeCameraSet()
    {
        List<Transform> points = pointsParent.GetComponentsInChildren<Transform>(true).ToList();

        foreach (Transform t in points)
        {
            pointsDic.Add(t.name, t);
        }
    }

    /// <summary>
    /// 相机移动到目标位置
    /// </summary>
    public void MoveToTarget(string pointName, float motionTime = 0, UnityAction action = null, bool canMove = true, bool canLift = true)
    {
        if (pointsDic.ContainsKey(pointName))
        {
            MoveToTarget(pointsDic[pointName], motionTime, action, canMove, canLift);
        }
        else
        {
            Debug.Log($"<size=13><color=red>不存在视角：</color></size>{pointName}");
        }
    }

    /// <summary>
    /// 移动到目标为止，并在站定以后调用事件
    /// </summary>
    public void MoveToTarget(Transform target, float motionTime, UnityAction moveEvent = null, bool canMove = true, bool canLift = true)
    {
        // 终止自身正在运作的Tween
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
