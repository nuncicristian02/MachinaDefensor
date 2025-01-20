using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] BaseEnemyPrefabs;
    public GameObject[] IntermediateEnemyPrefabs;
    public GameObject[] BossEnemyPrefabs;
    public float SpawnCooldown = 2f;
    public bool IsActive = true;

    [Header("Spawn Area")]
    public Vector2 spawnAreaSize = new Vector2(5f, 5f); 

    private float timeSinceLastSpawn = 0f;
    
    private void Update()
    {
        if (!IsActive)
        {
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= SpawnCooldown)
        {
            SpawnObject();
            timeSinceLastSpawn = 0f;
        }
    }

    private void SpawnObject()
    {
        GameObject prefab = null;

        if (RoundsManager.Instance.SpawnedBaseEnemies < RoundsManager.Instance.RoundSpawnersParameters.BaseEnemiesCount)
        {
            if (BaseEnemyPrefabs.Length == 0)
            {
                Debug.LogWarning("Nessun prefab base da spawnare!");
                return;
            }

            prefab = BaseEnemyPrefabs[Random.Range(0, BaseEnemyPrefabs.Length)];

            RoundsManager.Instance.SpawnedBaseEnemies += 1;
        }
        else if (RoundsManager.Instance.SpawnedIntermediateEnemies < RoundsManager.Instance.RoundSpawnersParameters.IntermediateEnemiesCount)
        {
            if (IntermediateEnemyPrefabs.Length == 0)
            {
                Debug.LogWarning("Nessun prefab intermedio da spawnare!");
                return;
            }

            prefab = IntermediateEnemyPrefabs[Random.Range(0, IntermediateEnemyPrefabs.Length)];

            RoundsManager.Instance.SpawnedIntermediateEnemies += 1;
        }
        else if (RoundsManager.Instance.SpawnedBossEnemies < RoundsManager.Instance.RoundSpawnersParameters.BossEnemiesCount)
        {
            if (BossEnemyPrefabs.Length == 0)
            {
                Debug.LogWarning("Nessun prefab boss da spawnare!");
                return;
            }

            prefab = BossEnemyPrefabs[Random.Range(0, BossEnemyPrefabs.Length)];

            RoundsManager.Instance.SpawnedBossEnemies += 1;
        }

        if (!prefab)
        {
            Debug.LogError("Impossible to find prefab to spawn enemy");
        }

        Instantiate(prefab, transform.position, Quaternion.identity);

        RoundsManager.Instance.SpawnedEnemies += 1;
    }
}