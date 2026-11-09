using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class CardUI : MonoBehaviour, IPointerClickHandler
{
    [Header("카드 데이터")]
    public CardData cardData;

    [Header("확대 설정")]
    public float zoomScale = 1.5f;

    private static CardUI selectedCard;

    private RectTransform rect;
    private RectTransform rootCanvasRect;
    private Button button;

    // 선택 전 카드 상태
    private Transform originalParent;
    private int originalIndex;
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalPivot;
    private Vector2 originalSize;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Quaternion originalRotation;

    private bool isSelected;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("카드는 Canvas 아래에 있어야 해.", this);
            enabled = false;
            return;
        }

        rootCanvasRect =
            canvas.rootCanvas.GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("카드 클릭 감지!", this);

        if (!enabled || !button.IsInteractable())
            return;

        // 우클릭하면 선택 취소
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (selectedCard != null)
                selectedCard.ReturnToHand();

            return;
        }

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        BattleTurnManager manager = BattleTurnManager.Instance;

        if (manager == null || !manager.CanPlayCard)
            return;
        // 이미 선택된 카드를 다시 클릭하면 기술 사용
        if (isSelected)
        {
            UseSkill();
            return;
        }

        // 다른 카드가 선택되어 있다면 원위치로 복귀
        if (selectedCard != null)
            selectedCard.ReturnToHand();

        SelectCard();
    }

    private void SelectCard()
    {
        // 원래 위치와 크기 저장
        originalParent = rect.parent;
        originalIndex = rect.GetSiblingIndex();

        originalAnchorMin = rect.anchorMin;
        originalAnchorMax = rect.anchorMax;
        originalPivot = rect.pivot;
        originalSize = rect.sizeDelta;
        originalPosition = rect.anchoredPosition3D;
        originalScale = rect.localScale;
        originalRotation = rect.localRotation;

        // Canvas 바로 아래로 이동하여 다른 UI 위에 표시
        rect.SetParent(rootCanvasRect, false);
        rect.SetAsLastSibling();

        // 화면 중앙으로 이동
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.sizeDelta = originalSize;
        rect.anchoredPosition3D = Vector3.zero;
        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one * zoomScale;

        selectedCard = this;
        isSelected = true;
    }

    private void ReturnToHand()
    {
        // 원래 부모와 순서 복구
        rect.SetParent(originalParent, false);
        rect.SetSiblingIndex(originalIndex);

        // 원래 위치와 크기 복구
        rect.anchorMin = originalAnchorMin;
        rect.anchorMax = originalAnchorMax;
        rect.pivot = originalPivot;
        rect.sizeDelta = originalSize;
        rect.anchoredPosition3D = originalPosition;
        rect.localScale = originalScale;
        rect.localRotation = originalRotation;

        isSelected = false;

        if (selectedCard == this)
            selectedCard = null;
    }

    private void UseSkill()
    {
        BattleTurnManager manager = BattleTurnManager.Instance;

        if (manager == null)
        {
            Debug.LogWarning("씬에 BattleTurnManager가 없어.", this);
            return;
        }

        if (manager.TryUsePlayerCard(cardData))
            ReturnToHand();
    }

    private void OnDisable()
    {
        if (isSelected && originalParent != null)
            ReturnToHand();

        if (selectedCard == this)
            selectedCard = null;
    }
}