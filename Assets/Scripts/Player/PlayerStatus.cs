using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerStatus : NetworkBehaviour
{
    public NetworkVariable<int> gold = new NetworkVariable<int>(0);
    private NetworkVariable<int> ore = new NetworkVariable<int>(0);
    public NetworkList<int> cards = new NetworkList<int>();

    public CardDatabase cardDatabase;

    public NetworkVariable<int> miningBonus = new NetworkVariable<int>(0);
    public NetworkVariable<int> fishingBonus = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        gold.OnValueChanged += ChangeGold;
        ore.OnValueChanged += ChangeOre;
        cards.OnListChanged += ChangeCard;
    }

    public void RemoveGold(int amount)
    {
        if (!IsServer) return;
        gold.Value -= amount;
    }

    private void ChangeCard(NetworkListEvent<int> changeEvent)
    {
        CardData cardData = cardDatabase.GetCardData(changeEvent.Value);
        if (cardData == null) return;
        Debug.Log("카드 이름은? " + cardData.cardName);
    }

    public void RemoveCards(int cardId)
    {
        if (!IsServer) return;
        cards.Remove(cardId);
    }

    public void AddFishingBonus(int mount)
    {
        if (!IsServer) return;
        fishingBonus.Value += mount;
    }
    public void AddMiningBonus(int mount)
    {
        if (!IsServer) return;
        miningBonus.Value += mount;
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
