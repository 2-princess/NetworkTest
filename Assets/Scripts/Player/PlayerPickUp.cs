using Unity.Netcode;
using UnityEngine;

public class PlayerPickUp : NetworkBehaviour
{
    public PlayerStatus playerStatus;
    public Transform handPos;
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUpRPC();
        }
    }

    [Rpc(SendTo.Server)]
    private void PickUpRPC()
    {
        Collider[] hits = Physics.OverlapSphere(handPos.position, 2f);
        foreach (Collider item in hits)
        {
            if (item.CompareTag("Item"))
            {
                NetworkObject netObj = item.GetComponent<NetworkObject>();
                if (netObj == null) continue;
                netObj.Despawn();
                playerStatus.AddGold(10);
                break;
            }
        }
    }
}
