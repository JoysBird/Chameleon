using UnityEngine;

public class EnemyCard : MonoBehaviour
{
    // Inspector에 등록한 이 적의 기술 목록이다. ChooseCard가 빈 칸을 제외하고 한 장을 뽑는다.
    public CardData[] cards;

    // 턴 관리자는 미리 뽑힌 카드를 읽고, 다음 카드 지정은 이 컴포넌트 안에서 처리한다.
    public CardData NextCard { get; private set; }
    private EnemyCardUI cardUI;

    private void Start()  // 적이 생성된 뒤 미리보기 UI를 찾아 첫 행동을 예고한다.
    {
        cardUI = FindFirstObjectByType<EnemyCardUI>();
        PrepareNextCard();
    }

    public void PrepareNextCard()  // 다음 적 턴의 카드를 정하고 미리보기에 같은 카드를 표시한다.
    {
        NextCard = ChooseCard();  // 지금 뽑은 결과를 보관해 실제 반격에서도 같은 카드를 사용한다.

        if (cardUI != null)
            cardUI.ShowCard(NextCard);
    }

    public CardData ChooseCard()  // 적의 카드 목록에서 비어 있는 항목을 제외하고 무작위 한 장을 반환한다.
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

        int selectedIndex = Random.Range(0, validCount);  // 유효한 카드 수를 기준으로 0부터 개수 미만까지의 번호를 뽑는다.

        foreach (CardData card in cards)
        {
            if (card == null)
                continue;

            if (selectedIndex == 0)
                return card;

            selectedIndex--;  // 빈 항목을 제외하고 한 장씩 지나가며 뽑은 순번을 찾는다.
        }

        return null;
    }
}
