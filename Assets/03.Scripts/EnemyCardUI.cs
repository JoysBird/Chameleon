using TMPro;
using UnityEngine;

public class EnemyCardUI : MonoBehaviour
{
    public TMP_Text cardText;

    public void ShowCard(CardData card)
    {
        if (cardText == null)
            return;

        if (card == null)
        {
            cardText.text = "사용할 기술 없음";
            return;
        }

        cardText.text =
            $"다음 행동\n\n{card.name}\n\n피해량: {card.damage}";
    }
}