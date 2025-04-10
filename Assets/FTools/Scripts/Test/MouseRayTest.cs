using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseRayTest : MonoBehaviour
{
    public enum RaycastType { 全部物体, 首个物体, 目标物体, 顶层UI, 全部UI }
    public RaycastType raycastType;
    public bool showRay;
    public LayerMask layer;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.R))
        {
            if (showRay) Debug.DrawRay(Camera.main.transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100, Color.red);
            switch (raycastType)
            {
                case RaycastType.全部物体:
                    ShowAllCollider();
                    break;
                case RaycastType.首个物体:
                    ShowFirstCollider();
                    break;
                case RaycastType.目标物体:
                    ShowTargetLayerCollider();
                    break;
                case RaycastType.顶层UI:
                    ShowTopUI();
                    break;
                case RaycastType.全部UI:
                    ShowAllUI();
                    break;
                default:
                    break;
            }
        }
#endif
    }

    void ShowAllCollider()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (var h in hits)
        {
            Debug.Log(h.collider.name);
        }
    }

    void ShowFirstCollider()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitting = Physics.Raycast(ray, out hit, 9999);
        if (hitting)
        {
            Debug.Log(hit.collider.name);
        }
    }

    void ShowTargetLayerCollider()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitting = Physics.Raycast(ray, out hit, 9999, layer);
        if (hitting)
        {
            Debug.Log(hit.collider.name);
        }
    }

    void ShowTopUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, result);
            Debug.Log(result[0]);
        }
    }

    void ShowAllUI()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, result);
            Debug.Log("**********************************************");
            foreach (var i in result)
            {
                Debug.Log(i.gameObject.name);
            }
        }
    }
}
