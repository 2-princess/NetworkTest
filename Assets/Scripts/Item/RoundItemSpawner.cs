using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class RoundItemSpawner : NetworkBehaviour
{
    public NetworkObject itemPrefeb;

    public override void OnNetworkSpawn()
    {
        GameStateManager.Instance.gameState.OnValueChanged += OnGameStateChange;
    }
    public override void OnNetworkDespawn()
    {
        GameStateManager.Instance.gameState.OnValueChanged -= OnGameStateChange;
    }

    private void OnGameStateChange(GameStateManager.GameState oldState, GameStateManager.GameState newState)
    {
        if (newState != GameStateManager.GameState.Playing) return;
        if (!IsServer) return;
        NetworkObject item = Instantiate(itemPrefeb, transform.position, Quaternion.identity);
        item.Spawn();
    }


}
