using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Card/CardData")]
public class CardData : ScriptableObject
{
    public enum CardEffectType
    {
        Mining,
        Fishing,
        MoveSpeed,
        Gold,
    }
    
    public int cardId;
    public string cardName;
    public int effectValue;
    public CardEffectType cardEffectType;


}
