using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObjectMove : MonoBehaviour
{
    Coroutine moveCor;
    GameObject origObj;
    public string posName = "init";

    private void Awake()
    {
        origObj = new GameObject();
        origObj.transform.position = transform.position;
        origObj.transform.rotation = transform.rotation;
        origObj.transform.localScale = transform.localScale;
        origObj.transform.SetParent(transform.parent);
        posName = "init";
    }

    public void MoveToOrigin(float timer = 1f, Action afterAction = null)
    {
        MoveToTarget(origObj.transform, timer, afterAction, "init");
    }

    public void MoveToTarget(GameObject obj, float timer = 1f, Action afterAction = null, string posName = "target")
    {
        MoveToTarget(obj.transform, timer, afterAction, posName);
    }
    public void MoveToTarget(Transform trans, float timer = 1f, Action afterAction = null, string posName = "target")
    {
        transform.SetParent(trans.parent);
        if (moveCor != null) StopCoroutine(moveCor);
        moveCor = StartCoroutine(Cor_MoveTo(trans, timer, afterAction, posName));
    }

    IEnumerator Cor_MoveTo(Transform trans, float timer = 1f, Action afterAction = null, string posName = "target")
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 startScale = transform.localScale;
        this.posName = posName;

        while (elapsedTime < timer)
        {
            elapsedTime += Time.deltaTime;
            // 使用 SmoothStep 进行平滑插值
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsedTime / timer));

            transform.position = Vector3.Lerp(startPos, trans.position, t);
            transform.rotation = Quaternion.Slerp(startRot, trans.rotation, t);
            transform.localScale = Vector3.Lerp(startScale, trans.localScale, t);

            yield return null; // Use null or WaitForFixedUpdate() depending on your needs
        }

        // Ensure final position and rotation are exactly what was requested
        transform.position = trans.position;
        transform.rotation = trans.rotation;
        transform.localScale = trans.localScale;
        afterAction?.Invoke();
    }
}
