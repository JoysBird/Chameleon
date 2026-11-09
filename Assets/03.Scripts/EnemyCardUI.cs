using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCardUI : MonoBehaviour
{
    [Serializable]
    public class CardPreview
    {
        public CardData cardData;
        public GameObject cardPrefab;
    }

    [Header("카드를 표시할 위치")]
    public RectTransform cardParent;

    [Header("데이터와 카드 프리팹 연결")]
    public CardPreview[] cardPreviews;

    [Header("표시 크기")]
    public float cardScale = 1f;

    private GameObject currentCard;
    private RectTransform previewRoot;

    public void ShowCard(CardData card)
    {
        ClearCard();

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
            cardUI.enabled = false;
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
        rect.anchoredPosition3D = Vector3.zero;
        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one * cardScale;

        currentCard.SetActive(true);
        previewRoot.gameObject.SetActive(true);
    }

    private GameObject FindCardPrefab(CardData card)
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

    private void CreatePreviewRoot()
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
        group.blocksRaycasts = false;
    }

    private void ClearCard()
    {
        if (currentCard == null)
            return;

        currentCard.SetActive(false);
        Destroy(currentCard);
        currentCard = null;
    }
}