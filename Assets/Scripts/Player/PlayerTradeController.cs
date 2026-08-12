using Unity.Netcode;
using UnityEngine;

public class PlayerTradeController : NetworkBehaviour
{
    public PlayerStatus playerStatus;
    public Transform tradeTrans;

    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.T))
        {
            Collider[] targets = Physics.OverlapSphere(tradeTrans.position, 1f);
            foreach (Collider target in targets)
            {
                if (target.CompareTag("Player"))
                {
                    NetworkObject targetObj = target.GetComponent<NetworkObject>();
                    if (targetObj == null) continue;
                    ulong targetId = targetObj.OwnerClientId;
                    SendToGoldRpc(targetId, 10);
                }
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void SendToGoldRpc(ulong targetClientId, int amount)
    {
        if (targetClientId == OwnerClientId) return;
        if (amount <= 0) return;
        if (playerStatus.gold.Value < amount) return; // 돈부족
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(targetClientId, out NetworkClient targetClient)) return; // 타겟없음

        NetworkObject targetPlayer = targetClient.PlayerObject;
        if (targetPlayer == null) return;
        PlayerStatus targetStatus = targetPlayer.GetComponent<PlayerStatus>();
        if (targetStatus == null) return;
        playerStatus.RemoveGold(amount);
        targetStatus.AddGold(amount);
    }
}
