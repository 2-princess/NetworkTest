using UnityEngine;


public class ItemData : MonoBehaviour
{
    public enum ItemType
    {
        Gold,
        Ore,
        Card
    }
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public int value;

}
