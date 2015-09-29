using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    public GameObject DefaultSpawnPrefab;

    private void Awake()
    {
        SpawnerManager.Spawners.Add(this);
    }

    public void Trigger()
    {
        Trigger(DefaultSpawnPrefab);
    }

    public void Trigger(GameObject spawnPrefab)
    {
        Instantiate(spawnPrefab, transform.position, transform.rotation);
    }
}
