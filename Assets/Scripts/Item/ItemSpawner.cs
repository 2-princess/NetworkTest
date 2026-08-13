using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject itemPrefeb;
    public Transform[] spawnPoints;
    public HashSet<int> usedSpawnPoints = new HashSet<int>();
    private List<NetworkObject> spawnItems = new List<NetworkObject>();

    public override void OnNetworkSpawn()
    {
        GameStateManager.Instance.gameState.OnValueChanged += OnGameStateChange;
    }
    public override void OnNetworkDespawn()
    {
        GameStateManager.Instance.gameState.OnValueChanged -= OnGameStateChange;
    }

    public void SpawnItem()
    {
        if (!IsServer) return;
        if (usedSpawnPoints.Count >= spawnPoints.Length) return;

        int randInt = Random.Range(0, spawnPoints.Length);
        while (!usedSpawnPoints.Add(randInt))
        {
            randInt = Random.Range(0, spawnPoints.Length);
        }
        GameObject item = Instantiate(itemPrefeb, spawnPoints[randInt].position, Quaternion.identity);
        NetworkObject networkItem = item.GetComponent<NetworkObject>();
        networkItem.Spawn();
        spawnItems.Add(networkItem);
    }

    private void OnGameStateChange(GameStateManager.GameState oldState, GameStateManager.GameState newState)
    {
        if (!IsServer) return;
        if (newState == GameStateManager.GameState.Playing)
        {
            for (int i = 0; i < 3; i++)
            {
                SpawnItem();
            }
        }
        if (newState == GameStateManager.GameState.RoundEnd)
        {
            usedSpawnPoints.Clear();
            foreach (NetworkObject item in spawnItems)
            {
                if (item != null && item.IsSpawned) item.Despawn();
            }
            spawnItems.Clear();
        }
    }
}
