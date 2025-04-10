using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �����϶�
/// </summary>
public class PanelMoveController : MonoBehaviour, IDragHandler
{
    public RectTransform panel;
    Vector3 origPos;

    private void Awake()
    {
        origPos = panel.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        panel.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }

    private void OnEnable()
    {
        panel.transform.position = origPos;
    }
}