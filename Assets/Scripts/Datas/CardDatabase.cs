using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Datas/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> cardDatas = new List<CardData>();

    public CardData GetCardData(int cardId)
    {
        return cardDatas.Find(card => card.cardId == cardId);
    }
}
