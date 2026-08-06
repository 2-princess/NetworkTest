using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    private NetworkVariable<int> hp = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        hp.OnValueChanged += ChangeHp;
    }

    private void ChangeHp(int oldHp, int newHp)
    {
        Debug.Log($"{name} 체력: {oldHp} → {newHp}");
    }
    
    public void TakeDamage(int damage)
    {
        if (!IsServer) return;

        hp.Value -= damage;
    }

}

