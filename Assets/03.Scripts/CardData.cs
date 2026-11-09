using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Game/Card Data")]
public class CardData : ScriptableObject
{
    [Header("카드 효과")]
    public int damage;
}