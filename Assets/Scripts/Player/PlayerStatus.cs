using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    private NetworkVariable<int> gold = new NetworkVariable<int>(0);

    public void AddGold(int amount)
    {
        if (!IsServer) return;
        gold.Value += amount;
    }
}
