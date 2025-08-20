using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameConfig config;
    public Collider arena;
    public Transform obstacles;
    public float spawnOffset = 2.5f; 

    private void Start()
    {
        SpawnObstacles();
    }

    public void SpawnObstacles()
    {
        Bounds b = arena.bounds;
        for (int i = 0; i < config.obstacleCount; i++)
        {
            GameObject prefab = config.obstaclePrefab[Random.Range(0, config.obstaclePrefab.Count)];

            Vector3 pos = new Vector3(
                Random.Range(b.min.x + spawnOffset, b.max.x - spawnOffset),
                b.min.y,
                Random.Range(b.min.z + spawnOffset, b.max.z - spawnOffset)
            );

            Quaternion rot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

            Instantiate(prefab, pos, rot, obstacles);
        }
    }
}
