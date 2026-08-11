using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    private NetworkVariable<int> gold = new NetworkVariable<int>(0);
    private NetworkVariable<int> ore = new NetworkVariable<int>(0);
    public NetworkList<int> cards = new NetworkList<int>();

    public CardDatabase cardDatabase;

    public NetworkVariable<int> miningBonus = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        gold.OnValueChanged += ChangeGold;
        ore.OnValueChanged += ChangeOre;
        cards.OnListChanged += ChangeCard;
    }

    private void ChangeCard(NetworkListEvent<int> changeEvent)
    {
        CardData cardData = cardDatabase.GetCardData(changeEvent.Value);
        if (cardData == null) return;
        Debug.Log("카드 이름은? " + cardData.cardName);
    }

    [Rpc(SendTo.Server)]
    public void UseCardRpc(int cardId)
    {
        if (!cards.Contains(cardId)) return;

        CardData cardData = cardDatabase.GetCardData(cardId);
        if (cardData == null) return;
        miningBonus.Value += cardData.effectValue;
        cards.Remove(cardId);
    }

    public void AddGold(int amount)
    {
        if (!IsServer) return;
        gold.Value += amount;
    }
    private void ChangeGold(int oldValue, int newValue)
    {
        Debug.Log("골드 증가 :" + newValue);
    }
    private void ChangeOre(int oldValue, int newValue)
    {
        Debug.Log("광석 증가 :" + newValue);
    }
    public void AddOre(int mount)
    {
        if (!IsServer) return;
        ore.Value += mount;
    }
    public void AddCard(int itemId)
    {
        if (!IsServer) return;
        cards.Add(itemId);
    }
}
