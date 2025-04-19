using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public float speed = 2f;
    public float lifetime = 10f;

    private void Start()
    {
        //ʵ�ֳ�����Χ�����ٶ�����ϰ���
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        MoveLeft();
    }

    private void MoveLeft()
    {
        //�����ƶ��ϰ���
        // Debug.Log("move left....");
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
