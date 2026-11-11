
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int enemiesPerSpawn = 3;
    [SerializeField] private float minSpawnDistance = 6f;
    [SerializeField] private float maxSpawnDistance = 10f;
    [SerializeField] private int maxEnemies = 100;

    [Header("Cleanup")]
    [SerializeField] private float despawnDistance = 30f;

    private List<GameObject> activeEnemies = new();
    private float spawnTimer;

    void Update()
    {
        if (enemyPrefab == null || player == null)
            return;

        CleanupEnemies();

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        int availableSlots =
            Mathf.Max(0, maxEnemies - activeEnemies.Count);

        int spawnCount =
            Mathf.Min(enemiesPerSpawn, availableSlots);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector2 direction =
                Random.insideUnitCircle.normalized;

            if (direction == Vector2.zero)
                direction = Vector2.right;

            float minDistance = Mathf.Min(
                minSpawnDistance, maxSpawnDistance
            );

            float maxDistance = Mathf.Max(
                minSpawnDistance, maxSpawnDistance
            );

            float distance = Random.Range(
                minDistance,
                maxDistance
            );

            Vector2 offset = direction * distance;

            Vector3 spawnPosition = new Vector3(
                player.position.x + offset.x,
                player.position.y + offset.y,
                enemyPrefab.transform.position.z
            );

            GameObject enemy = Instantiate(
                enemyPrefab,
                spawnPosition,
                Quaternion.identity
            );

            NPCFollowPatrol movement =
                enemy.GetComponent<NPCFollowPatrol>();

            if (movement != null)
                movement.SetPlayer(player);

            activeEnemies.Add(enemy);
        }
    }

    void CleanupEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemies[i];

            if (enemy == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            if (despawnDistance > 0f &&
                Vector2.Distance(
                    enemy.transform.position,
                    player.position
                ) > despawnDistance)
            {
                activeEnemies.RemoveAt(i);
                Destroy(enemy);
            }
        }
    }
}
