using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public PlayerStatus playerStatus;

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (playerStatus.cards.Count == 0) return;
            int cardId = playerStatus.cards[0];
            UseCardRpc(cardId);
        }
    }

    //? 카드를 사용해도되는지 흐름
    [Rpc(SendTo.Server)]
    public void UseCardRpc(int cardId)
    {
        if (!playerStatus.cards.Contains(cardId)) return;
        CardData cardData = playerStatus.cardDatabase.GetCardData(cardId);
        if (cardData == null) return;
        ApplyCardEffect(cardData);
        playerStatus.RemoveCards(cardId);
    }

    //? 카드효과를 판단하는 흐름
    private void ApplyCardEffect(CardData cardData)
    {
        switch (cardData.cardEffectType)
        {
            case CardData.CardEffectType.Mining:
                playerStatus.AddMiningBonus(cardData.effectValue);
                break;
            case CardData.CardEffectType.Fishing:
                playerStatus.AddFishingBonus(cardData.effectValue);
                break;
        }
    }
}
