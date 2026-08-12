using Unity.Netcode;
using UnityEngine;

public class PlayerTradeController : NetworkBehaviour
{
    public PlayerStatus playerStatus;
    [Rpc(SendTo.Server)]
    private void SendToGoldRpc(ulong targetClientId, int amount)
    {
        if (amount <= 0) return;
        if (playerStatus.gold.Value < amount) return; // 돈부족
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(targetClientId, out NetworkClient targetClient)) return; // 타겟없음
        
        NetworkObject targetPlayer = targetClient.PlayerObject;
        PlayerStatus targetStatus = targetPlayer.GetComponent<PlayerStatus>();
        playerStatus.RemoveGold(amount);
        targetStatus.AddGold(amount);
    }
}
