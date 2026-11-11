using UnityEngine;

public enum ItemEffectType
{
    HealHP
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [TextArea]
    public string description;

    public ItemEffectType effectType;

    [Min(0)]
    public int effectValue;
}