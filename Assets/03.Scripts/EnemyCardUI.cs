using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCardUI : MonoBehaviour
{
    // 카드 데이터와 표시용 프리팹을 한 쌍으로 묶어 Inspector 배열에서 편집할 수 있게 한다.
    [Serializable]
    public class CardPreview
    {
        public CardData cardData;
        public GameObject cardPrefab;
    }

    // 적의 다음 카드 미리보기를 생성할 UI 위치다.
    [Header("카드를 표시할 위치")]
    public RectTransform cardParent;

    // 각 CardData에 대응하는 그림과 텍스트가 들어 있는 프리팹을 연결한다.
    [Header("데이터와 카드 프리팹 연결")]
    public CardPreview[] cardPreviews;

    // 적 카드 미리보기에 적용할 크기 배율이다.
    [Header("표시 크기")]
    public float cardScale = 1f;

    private GameObject currentCard;
    private RectTransform previewRoot;

    public void ShowCard(CardData card)  // 기존 미리보기를 지우고 받은 데이터에 맞는 카드 그림을 표시한다.
    {
        ClearCard();  // 이전에 예고한 카드가 새 카드와 겹치지 않게 정리한다.

        if (card == null)
            return;

        if (cardParent == null)
        {
            Debug.LogWarning("Card Parent를 연결해줘.", this);
            return;
        }

        GameObject prefab = FindCardPrefab(card);

        if (prefab == null)
        {
            Debug.LogWarning(
                $"{card.name}에 연결된 카드 프리팹이 없어.", this);
            return;
        }

        CreatePreviewRoot();

        // 클릭 스크립트를 끈 뒤 화면에 표시한다.
        previewRoot.gameObject.SetActive(false);

        currentCard = Instantiate(prefab, previewRoot, false);

        foreach (CardUI cardUI in
                 currentCard.GetComponentsInChildren<CardUI>(true))
        {
            cardUI.enabled = false;  // 미리보기 카드가 손패 카드처럼 클릭되어 공격하는 일을 막는다.
        }

        foreach (Button button in
                 currentCard.GetComponentsInChildren<Button>(true))
        {
            button.interactable = false;
        }

        RectTransform rect =
            currentCard.GetComponent<RectTransform>();

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition3D = Vector3.zero;  // 중앙 앵커를 기준으로 위치 차이를 0으로 맞춘다.
        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one * cardScale;

        currentCard.SetActive(true);
        previewRoot.gameObject.SetActive(true);
    }

    private GameObject FindCardPrefab(CardData card)  // Inspector에 등록한 데이터와 프리팹 쌍에서 이 카드 그림을 찾는다.
    {
        if (cardPreviews == null)
            return null;

        foreach (CardPreview preview in cardPreviews)
        {
            if (preview != null && preview.cardData == card)
                return preview.cardPrefab;
        }

        return null;
    }

    private void CreatePreviewRoot()  // 적 카드 미리보기를 담는 UI 오브젝트를 처음 한 번 만든다.
    {
        if (previewRoot != null)
            return;

        GameObject root = new GameObject(
            "EnemyCardPreview",
            typeof(RectTransform),
            typeof(CanvasGroup));

        previewRoot = root.GetComponent<RectTransform>();
        previewRoot.SetParent(cardParent, false);
        previewRoot.anchorMin = Vector2.zero;
        previewRoot.anchorMax = Vector2.one;
        previewRoot.offsetMin = Vector2.zero;
        previewRoot.offsetMax = Vector2.zero;

        CanvasGroup group = root.GetComponent<CanvasGroup>();
        group.interactable = false;
        group.blocksRaycasts = false;  // 미리보기가 뒤에 있는 UI로 향하는 마우스 입력을 가로채지 않게 한다.
    }

    private void ClearCard()  // 현재 표시 중인 미리보기를 숨기고 복제한 오브젝트를 제거한다.
    {
        if (currentCard == null)
            return;

        currentCard.SetActive(false);
        Destroy(currentCard);
        currentCard = null;
    }
}