using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveWithRay : MonoBehaviour
{
    public bool isCopy = false;
    private static CameraMoveWithRay instance;

    [SerializeField]
    private Camera m_camera;
    public Camera M_Camera
    {
        get { return instance.m_camera; }
        private set { instance.m_camera = value; }
    }

    /// <summary>
    /// x - 前后 y - 上下 z - 左右 (输入)
    /// </summary>
    private Vector3 m_input;
    private Vector2 m_last_mouse_pos;
    private Vector2 m_screen_move_delta;
    [Header("Sensitive")]
    [Range(0, 0.1f)]
    public float rotate_sensitive = 0.05f;

    [Header("Move Sensitive")] public float move_sensitive = 0.05f;

    [Header("Shift加速倍率")] public float shift_up_speed_rate = 2f;

    [Header("碰撞距离")] public float collider_distance = 0.1f;

    private void Awake()
    {
        if (!isCopy)
        {
            instance = this;
            M_Camera = GetComponent<Camera>();
        }
        else
        {
#pragma warning disable CS1717 // 对同一变量进行了赋值
            instance = CameraMoveWithRay.instance;
#pragma warning restore CS1717 // 对同一变量进行了赋值

            rotate_sensitive = instance.rotate_sensitive;
            move_sensitive = instance.move_sensitive;
            shift_up_speed_rate = instance.shift_up_speed_rate;
            collider_distance = instance.collider_distance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isCopy)
            return;

        Vector3 input = GetInput();

        UpdateScreenMove();
        if (Input.GetMouseButton(1))
            CameraRotate(m_screen_move_delta);

        Vector3 delta = CameraMove(input);
        DetectColliderAndMove(delta);
    }

    private Vector3 GetInput()
    {
        m_input = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            m_input += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_input += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_input += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            m_input += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            m_input += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            m_input += new Vector3(0, -1, 0);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_input *= shift_up_speed_rate;
        }

        return m_input;
    }

    public void UpdateScreenMove()
    {
        m_screen_move_delta = Vector2.zero;

        Vector2 now_mouse_pos = Input.mousePosition;

        if (m_last_mouse_pos == Vector2.zero)
        {
            m_last_mouse_pos = now_mouse_pos;
            return;
        }

        m_screen_move_delta = now_mouse_pos - m_last_mouse_pos;
        m_last_mouse_pos = now_mouse_pos;
    }

    public void CameraRotate(Vector2 mouse_move_delta)
    {
        Vector2 delta = mouse_move_delta * rotate_sensitive;

        M_Camera.transform.Rotate(new Vector3(0, 1, 0), delta.x, Space.World);

        M_Camera.transform.Rotate(M_Camera.transform.right, -delta.y, Space.World);
    }

    /// <summary>
    /// DeltaMove
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private Vector3 CameraMove(Vector3 input)
    {
        Vector3 delta = new Vector3(0, 0, 0);

        Vector3 hor_forward = M_Camera.transform.forward;
        hor_forward.y = 0;
        hor_forward.Normalize();

        delta += hor_forward * input.z;
        delta += Vector3.up * input.y;
        delta += M_Camera.transform.right * input.x;

        delta *= Time.deltaTime * move_sensitive;

        //Debug.Log($"Delta:{delta}");
        return delta;
    }

    /// <summary>
    /// 检测碰撞
    /// </summary>
    /// <param name="delta"></param>
    private void DetectColliderAndMove(Vector3 delta)
    {
        Ray ray = new Ray(M_Camera.transform.position, delta);
        Debug.DrawLine(M_Camera.transform.position, M_Camera.transform.position + delta * 100, Color.blue);

        bool isHit = Physics.Raycast(ray, out RaycastHit hit, collider_distance);
        if (isHit)
        {
            //m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, hit.point, 0.8f);
        }
        else
        {
            M_Camera.transform.position += delta;
        }
    }

    public void SetEnable(bool active)
    {
        instance.enabled = active;
    }
}
