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
                    break;
                }
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void SendToGoldRpc(ulong targetClientId, int amount)
    {
        if (GameStateManager.Instance.gameState.Value != GameStateManager.GameState.Playing) return; // 게임중이아니면 리턴
        if (targetClientId == OwnerClientId) return;
        if (amount <= 0) return;
        if (playerStatus.gold.Value < amount) return; // 돈부족
        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(targetClientId, out NetworkClient targetClient)) return; // 타겟없음

        NetworkObject targetPlayer = targetClient.PlayerObject;
        if (targetPlayer == null) return;
        float distance = Vector3.Distance(transform.position, targetPlayer.transform.position);
        if (distance > 2) return;
        PlayerStatus targetStatus = targetPlayer.GetComponent<PlayerStatus>();
        if (targetStatus == null) return;
        playerStatus.RemoveGold(amount);
        targetStatus.AddGold(amount);
    }
}
