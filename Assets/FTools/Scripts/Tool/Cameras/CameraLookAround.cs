using CustomInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAround : MonoBehaviour
{
    public static bool enableRoteGlobal;

    public bool resetSelf;
    public bool enableRote = true;
    public float rotationSpeed = 200.0f;

    public bool doLimitHorizontal;
    [ShowIf(nameof(doLimitHorizontal))]
    public Vector2 limitHorizontal;
    public bool doLimitVertical;
    [ShowIf(nameof(doLimitVertical))]
    public Vector2 limitVertical = new Vector2(-90.0f,90.0f);

    public bool canChangeCursor;
    [ShowIf(nameof(canChangeCursor))] 
        public Texture2D cursorSprite;

    private float rotationX;
    private float rotationY;
    bool isPress;

    Vector3 origPos;
    Quaternion origRot;
    float curHorizontal;

    private void Awake()
    {
        origPos = transform.position;
        origRot = transform.rotation;
        rotationX = transform.eulerAngles.x;
        rotationY = transform.eulerAngles.y;
        curHorizontal = rotationY;
    }

    void Update()
    {
        if (enableRote && enableRoteGlobal)
        {
            if (Input.GetMouseButtonDown(1))
            {
                isPress = true;
            }
            if (isPress)
            {
                if (canChangeCursor) 
                {
                    if (cursorSprite != null)
                    {
                        Cursor.SetCursor(cursorSprite, Vector2.zero, CursorMode.Auto);
                    }
                    else
                    {
                        Debug.LogWarning("未找到" + name + "鼠标样式");
                    }

                }
                rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
                rotationY += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
                //rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);
                rotationX = Mathf.Clamp(rotationX, limitVertical[0], limitVertical[1]);
                if(doLimitHorizontal)rotationY = Mathf.Clamp(rotationY, curHorizontal - limitHorizontal[0], curHorizontal + limitHorizontal[1]);
                transform.rotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
            }
            else
            {
                if (canChangeCursor) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
            if (Input.GetMouseButtonUp(1))
            {
                isPress = false;
            }
        }
    }

    public void ResetTrans(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        rotationX = transform.eulerAngles.x;
        rotationY = transform.eulerAngles.y;
    }

    private void OnEnable()
    {
        if (resetSelf)ResetTrans(origPos,origRot);
    }
}
