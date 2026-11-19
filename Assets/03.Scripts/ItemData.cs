using UnityEngine;

public enum ItemEffectType
{
    HealHP
}

// Project 창의 Create 메뉴에서 포션 등 아이템별 효과와 수치를 담을 에셋을 만든다.
[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    [TextArea]
    public string description;

    public ItemEffectType effectType;

    // Inspector에서 효과 수치의 최솟값을 0으로 제한한다.
    [Min(0)]
    public int effectValue;  // 효과의 수치다. HealHP 포션이면 플레이어에게 전달할 회복량이 된다.
}