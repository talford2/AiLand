using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    public GameObject SpawnPrefab;

    private void Awake()
    {
        SpawnerManager.Spawners.Add(this);
    }

    public void Trigger()
    {
        Instantiate(SpawnPrefab, transform.position, transform.rotation);
    }
}
