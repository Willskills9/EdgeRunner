using UnityEngine;
using System.Collections;

public class SpawnRoad : MonoBehaviour
{
    public GameObject prefabToSpawn;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.9f);
        }
    }
}