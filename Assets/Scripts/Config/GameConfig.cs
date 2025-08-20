using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Player")]
    public float playerMoveSpeed = 5f;
    public float sprintMultiplier = 1.6f;
    public float jumpHeight = 1.5f; // CharacterController jump via velocity formula
    public float gravity = -9.81f;
    public float rotationSpeed = 5f;

    [Header("Enemy")]
    public float enemySpeed = 3.5f;

    [Header("Spawning")]
    public float enemySpawnInterval = 3f; // one every 3 seconds
    public float spawnRadius = 12f;       // around player
    public float minSpawnDistance = 6f;   // do not spawn too close
    public int enemyPoolSize = 24;        // perf: pooling

    [Header("Key")]
    public float keyRespawnTime = 12f;        // n seconds: one-time re-randomization
    public float keyMinDistanceFromStart = 15f; // "far" from player's starting point

    [Header("Arena Settings")]
    public int obstacleCount = 10;   // Count obstacles
    public List<GameObject> obstaclePrefab;
}
