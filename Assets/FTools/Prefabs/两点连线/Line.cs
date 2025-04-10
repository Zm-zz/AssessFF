using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    private Material material;
    private Vector2 tiling;
    private int mainTexProperty;
    public Vector3 pointA;
    public Vector3 pointB;
    private Vector3 pointC;
    private float lineLen;
    private float density = 2f;
    private Canvas text_Canvas;

    private void Awake()
    {
        text_Canvas = GetComponentInChildren<Canvas>();
    }
    private void Start()
    {
        material = lineRenderer.material;
        mainTexProperty = Shader.PropertyToID("_MainTex");
        material.SetTextureScale(mainTexProperty, tiling);
    }
    private void Update()
    {
        //跟随
        lineRenderer.SetPosition(0, pointA);
        lineRenderer.SetPosition(1, pointB);
        //中点位置
        pointC = (pointA + pointB)/2;
        text_Canvas.transform.position = pointC;
        text_Canvas.transform.LookAt(Camera.main.transform.position);
        text_Canvas.transform.rotation = Quaternion.Slerp(text_Canvas.transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - text_Canvas.transform.position), 0);

        //自适应
        lineLen = (lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).magnitude;
        tiling = new Vector2(lineLen * density, 0);
        material.SetTextureScale(mainTexProperty, tiling);
    }
}
