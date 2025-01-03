using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] prefabsToSpawn;
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
        if (prefabsToSpawn.Length == 0)
        {
            Debug.LogWarning("Nessun prefab da spawnare!");
            return;
        }

        GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

        Instantiate(prefab, transform.position, Quaternion.identity);

        
    }
}