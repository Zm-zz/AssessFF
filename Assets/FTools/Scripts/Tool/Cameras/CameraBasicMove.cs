using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������ƶ���ֱ�ӹ���������ϣ�������Collider��rigidbody�������סY���ƶ�����������ת
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class CameraBasicMove : MonoBehaviour
{
    //����Ϊtrue
    private static bool canMove = true; 
    public bool canCameraMove;
    public float moveSpeed = 2.0f;      //�ƶ��ٶ�
    public float rotationSpeed = 2.0f;  //��ת�ٶ�
    private static Vector3 preLookPos;  //��һ֡��תλ��
    private static Vector3 lookDelta;   //��תʱ���ƫ����

    //����
    public bool canLifting;
    public KeyCode upCode = KeyCode.E;
    public KeyCode downCode = KeyCode.Q;
    public float maxLimitY = 3f;   //�������
    public float minLimitY = 0.5f;   //�������
    public float LiftingSpeed = 3;//�����ٶ�

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    Vector3 origPos;
    Quaternion origRot;

    public static bool CanMove
    {
        set { canMove = value; }
        get { return canMove; }
    }

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        origPos = transform.position;
        origRot = transform.rotation;
        rotationX = transform.eulerAngles.x;
        rotationY = transform.eulerAngles.y;
        if (transform.position.y > maxLimitY)
        {
            transform.position = new Vector3(transform.position.x, maxLimitY, transform.position.z);
        }
        else if (transform.position.y < minLimitY)
        {
            transform.position = new Vector3(transform.position.x, minLimitY, transform.position.z);
        }
    }
    private void Reset()
    {
        upCode = KeyCode.E;
        downCode = KeyCode.Q;
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<BoxCollider>().size = Vector3.one * 0.5f;
    }

    void Update()
    {
        if (canMove && canCameraMove)
        {
            LookAround();
        }
    }

    private void FixedUpdate()
    {
        if (canMove && canCameraMove)
        {
            Move();
            Lifting();
        }
    }

    private void Move()
    {
        // Camera Movement
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        Vector3 localMoveDirection = new Vector3(horizontalMove, 0.0f, verticalMove);
        Vector3 worldMoveDirection = transform.TransformDirection(localMoveDirection);
        worldMoveDirection = new Vector3(worldMoveDirection.x, 0, worldMoveDirection.z).normalized;

        rb.velocity = worldMoveDirection * moveSpeed * Time.deltaTime * 1000;
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D)) rb.velocity = Vector3.zero;
    }

    void LookAround()
    {
        // Camera Rotation
        if (Input.GetMouseButtonDown(1))
        {
            preLookPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            lookDelta = Vector3.zero;
        }
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            rotationX = transform.eulerAngles.x;
            rotationY = transform.eulerAngles.y;
            rotationX -= Input.GetAxis("Mouse Y") * rotationSpeed;
            rotationY += Input.GetAxis("Mouse X") * rotationSpeed;
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
            float x = Input.mousePosition.x - preLookPos.x;
            float y = Input.mousePosition.y - preLookPos.y;
            lookDelta = Input.mousePosition - preLookPos;
            preLookPos = Input.mousePosition;
        }
    }

    public static Vector3 GetLookDelta()
    {
        if (lookDelta == null) return Vector3.zero;
        return lookDelta;
    }

    void Lifting()
    {
        if (!canLifting || !canCameraMove || !canMove) return;
        if (Input.GetKey(upCode))
        {
            if (transform.position.y + LiftingSpeed * Time.deltaTime < maxLimitY)
            {
                transform.position += new Vector3(0, LiftingSpeed * Time.deltaTime, 0);
            }
        }
        if (Input.GetKey(downCode))
        {
            if (transform.position.y - LiftingSpeed * Time.deltaTime > minLimitY)
            {
                transform.position -= new Vector3(0, LiftingSpeed * Time.deltaTime, 0);
            }
        }
    }

    public void ResetTrans(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        rotationX = transform.eulerAngles.x;
        rotationY = transform.eulerAngles.y;
        if (transform.position.y > maxLimitY)
        {
            transform.position = new Vector3(transform.position.x, maxLimitY, transform.position.z);
        }
        else if (transform.position.y < minLimitY)
        {
            transform.position = new Vector3(transform.position.x, minLimitY, transform.position.z);
        }
    }

    private void OnEnable()
    {
        ResetOrig();
    }

    public void ResetOrig()
    {
        ResetTrans(origPos, origRot);
    }
}