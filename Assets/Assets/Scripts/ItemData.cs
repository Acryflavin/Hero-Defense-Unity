using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public int maxStackSize = 99;
    public ItemType itemType;
}

public enum ItemType
{
    Resource,
    Tool,
    Weapon,
    Consumable
}
