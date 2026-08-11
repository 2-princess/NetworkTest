using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public PlayerStatus playerStatus;

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (playerStatus.cards.Count == 0) return;
            int cardId = playerStatus.cards[0];
            playerStatus.UseCardRpc(cardId);
        }
    }
}
