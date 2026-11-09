using UnityEngine;

public class EnemyCard : MonoBehaviour
{
    public CardData[] cards;

    public CardData ChooseCard()
    {
        if (cards == null || cards.Length == 0)
            return null;

        // 비어 있는 칸은 제외하고 무작위 선택
        int validCount = 0;

        foreach (CardData card in cards)
        {
            if (card != null)
                validCount++;
        }

        if (validCount == 0)
            return null;

        int selectedIndex = Random.Range(0, validCount);

        foreach (CardData card in cards)
        {
            if (card == null)
                continue;

            if (selectedIndex == 0)
                return card;

            selectedIndex--;
        }

        return null;
    }
}