using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public List<SpawnPoint> spawnPoints;
    void Awake()
    {
        Instance = this;
    }

    public SpawnPoint GetEmptySpawnPoint()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            if (!spawnPoints[i].isUsed)
            {
                spawnPoints[i].isUsed = true;
                return spawnPoints[i];
            }
        }
        return null;
    }
}
