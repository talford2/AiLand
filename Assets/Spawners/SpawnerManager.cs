using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager
{
    private static List<NpcSpawner> spawners;

    public static List<NpcSpawner> Spawners
    {
        get
        {
            if (spawners==null)
                spawners = new List<NpcSpawner>();
            return spawners;
        }
    }

    public static void TriggerRandom()
    {
        var randomSpawner = Spawners[Random.Range(0, Spawners.Count)];
        randomSpawner.Trigger();
    }

    public static void TriggerRandom(GameObject spawnPrefab)
    {
        var randomSpawner = Spawners[Random.Range(0, Spawners.Count)];
        randomSpawner.Trigger(spawnPrefab);
    }

}
