using UnityEngine;

public class BarrierSpawner : MonoBehaviour
{
    public GameObject barrierPrefab;
    public float spawnDelay = 2f;
    public float minSpawnHeight = -2f;
    public float maxSpawnHeight = 2f;

    private void Start()
    {
        //�ظ����ú�����ʵ����Ʒ�����ظ�����
        InvokeRepeating("SpawnBarrier", 0f, spawnDelay);
    }

    private void SpawnBarrier()
    {
        float randomHeight = Random.Range(minSpawnHeight, maxSpawnHeight);
        Vector2 spawnPosition = new Vector2(transform.position.x, randomHeight);
        Instantiate(barrierPrefab, spawnPosition, Quaternion.identity).transform.parent = transform;
    }
}
