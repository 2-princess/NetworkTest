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
        if (GameStateManager.Instance.gameState.Value != GameStateManager.GameState.Playing) return;
        Collider[] hits = Physics.OverlapSphere(handPos.position, 2f);
        foreach (Collider item in hits)
        {
            if (item.CompareTag("Item"))
            {
                NetworkObject netObj = item.GetComponent<NetworkObject>();
                if (netObj == null) continue;
                ItemData itemData = item.GetComponent<ItemData>();
                if (itemData == null) continue;
                switch (itemData.itemType)
                {
                    case ItemData.ItemType.Gold:
                        playerStatus.AddGold(itemData.value);
                        break;
                    case ItemData.ItemType.Ore:
                        playerStatus.AddOre(itemData.value);
                        break;
                    case ItemData.ItemType.Card:
                        playerStatus.AddCard(itemData.itemId);
                        break;
                }
                netObj.Despawn();
                break;
            }
        }
    }
}
