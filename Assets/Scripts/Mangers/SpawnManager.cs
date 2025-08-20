// Assets/_Project/Scripts/Managers/SpawnManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameConfig config;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private BoxCollider roomBounds; // isTrigger = true, bounds of playable area
    [SerializeField] private Transform enemyParent;

    private readonly List<EnemyController> pool = new();
    private readonly List<EnemyController> activeEnemies = new();
    private Coroutine spawnRoutine;
    private bool spawning = true;

    private void OnEnable()
    {
        GameManager.OnKeyCollected += HandleKeyCollected;
        GameManager.OnGameLost += StopAll;
        GameManager.OnGameWon += StopAll;
    }

    private void OnDisable()
    {
        GameManager.OnKeyCollected -= HandleKeyCollected;
        GameManager.OnGameLost -= StopAll;
        GameManager.OnGameWon -= StopAll;
    }

    private void Start()
    {
        // Prewarm pool
        for (int i = 0; i < config.enemyPoolSize; i++)
        {
            var go = Instantiate(enemyPrefab, enemyParent);
            go.SetActive(false);
            var ec = go.GetComponent<EnemyController>();
            pool.Add(ec);
        }
        spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        var wait = new WaitForSeconds(config.enemySpawnInterval);
        while (spawning)
        {
            TrySpawnEnemy();
            yield return wait;
        }
    }

    private void TrySpawnEnemy()
    {
        if (!spawning) return;
        if (player == null) return;

        if (!RandomPointAroundPlayer(out Vector3 spawnPos)) return;

        var enemy = GetFromPool();
        enemy.transform.position = spawnPos;
        enemy.gameObject.SetActive(true);
        enemy.Setup(player, config);
        activeEnemies.Add(enemy);
    }

    private EnemyController GetFromPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeSelf) return pool[i];
        }
        // Optional expand
        var go = Instantiate(enemyPrefab, enemyParent);
        go.SetActive(false);
        var ec = go.GetComponent<EnemyController>();
        pool.Add(ec);
        return ec;
    }

    public void DespawnAll()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            activeEnemies[i].Deactivate();
        }
        activeEnemies.Clear();
    }

    private void HandleKeyCollected()
    {
        spawning = false;
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
        DespawnAll();
    }

    private void StopAll()
    {
        spawning = false;
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
    }

    // --- Utility: random point around player within bounds ---
    private bool RandomPointAroundPlayer(out Vector3 result)
    {
        Bounds b = roomBounds.bounds;
        const int maxAttempts = 30; 
        for (int i = 0; i < maxAttempts; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            float radius = Random.Range(config.minSpawnDistance, config.spawnRadius);

            Vector3 candidate = player.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            candidate.y = b.min.y + 0.1f; 

            if (b.Contains(candidate) && Vector3.Distance(candidate, player.position) >= config.minSpawnDistance)
            {
                result = candidate;
                return true;
            }
        }

        Vector3 fallback = player.position + Vector3.back * config.minSpawnDistance;
        fallback.y = b.min.y + 0.1f;
        if (b.Contains(fallback))
        {
            result = fallback;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
