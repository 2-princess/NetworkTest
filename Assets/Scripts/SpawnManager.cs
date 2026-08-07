using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    public List<Transform> spawnPoint;
    void Awake()
    {
        Instance = this;
    }

    public Transform GetSpawnPoint(int index)
    {
        return spawnPoint[index];
    }
}
