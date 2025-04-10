using UnityEngine;

/// <summary>
/// �����ӽ����
/// </summary>
public class CameraFree : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 2.0f;

    private float RotationX
    {
        get { return transform.eulerAngles.x; }
        set
        {
            Vector3 vector = transform.eulerAngles;
            vector.x = value;
            transform.eulerAngles = vector;
        }
    }
    private float RotationY
    {
        get { return transform.eulerAngles.y; }
        set
        {
            Vector3 vector = transform.eulerAngles;
            vector.y = value;

            transform.eulerAngles = vector;
        }
    }


    void Update()
    {
        // Camera Movement
        Vector3 right = ((Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0)) * Camera.main.transform.right;
        Vector3 forward = ((Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0)) * Camera.main.transform.forward;

        Vector3 moveDirection = right + forward;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Camera Rotation
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            Cursor.lockState = CursorLockMode.Locked;
            RotationX -= Input.GetAxis("Mouse Y") * rotationSpeed;
            RotationY += Input.GetAxis("Mouse X") * rotationSpeed;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}