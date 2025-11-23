using UnityEngine;
using System.Collections.Generic;

public class InfiniteGridSpawner : MonoBehaviour
{
    public GameObject tilePrefab;
    public SphereCollider sphere;
    public float cellSize = 10f;   // grid cell size
    private Dictionary<Vector2Int, GameObject> spawnedTiles = new Dictionary<Vector2Int, GameObject>();

    void Update()
    {
        Vector3 center = sphere.transform.position;
        float radius = sphere.radius * Mathf.Max(
            sphere.transform.lossyScale.x,
            sphere.transform.lossyScale.y,
            sphere.transform.lossyScale.z
        );

        int minX = Mathf.FloorToInt((center.x - radius) / cellSize);
        int maxX = Mathf.FloorToInt((center.x + radius) / cellSize);
        int minZ = Mathf.FloorToInt((center.z - radius) / cellSize);
        int maxZ = Mathf.FloorToInt((center.z + radius) / cellSize);

        // Spawn needed tiles
        for (int x = minX; x <= maxX; x++)
        {
            for (int z = minZ; z <= maxZ; z++)
            {
                Vector3 tileCenter = new Vector3(x * cellSize, 0, z * cellSize);

                if (Vector3.Distance(tileCenter, center) <= radius)
                {
                    Vector2Int key = new Vector2Int(x, z);
                    if (!spawnedTiles.ContainsKey(key))
                    {
                        GameObject tile = Instantiate(tilePrefab, tileCenter, Quaternion.identity);
                        spawnedTiles.Add(key, tile);
                    }
                }
            }
        }

        // Remove tiles outside sphere
        List<Vector2Int> toRemove = new List<Vector2Int>();

        foreach (var kvp in spawnedTiles)
        {
            Vector3 pos = kvp.Value.transform.position;
            if (Vector3.Distance(pos, center) > radius)
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var key in toRemove)
            spawnedTiles.Remove(key);
    }
}

