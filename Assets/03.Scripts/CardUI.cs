using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 이 스크립트를 붙일 때 Unity가 같은 오브젝트에 Button 컴포넌트도 갖추도록 한다.
[RequireComponent(typeof(Button))]
public class CardUI : MonoBehaviour, IPointerClickHandler
{
    // Inspector에서 이 카드가 사용할 CardData 에셋을 연결한다.
    [Header("카드 데이터")]
    public CardData cardData;

    // 손패 카드를 선택했을 때 중앙에 표시할 확대 배율이다.
    [Header("확대 설정")]
    public float zoomScale = 1.5f;

    // static 필드 하나를 모든 카드가 공유한다. 새 카드를 선택하면 여기에 기록된 이전 카드를 손패로 돌려보낸다.
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

    private void Awake()  // 카드 클릭과 확대에 필요한 버튼, 위치 정보, Canvas를 처음 한 번 찾는다.
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

    public void OnPointerClick(PointerEventData eventData)  // Unity가 클릭 정보를 전달하면 마우스 버튼과 선택 상태에 따라 행동을 고른다.
    {
        if (!enabled || !button.IsInteractable())
            return;

        RewardCardManager rewardManager = RewardCardManager.Instance;  // 전투 보상 관리자를 찾아 손패 교체를 기다리는 중인지 확인한다.

        if (rewardManager != null && rewardManager.IsSelecting)  // 보상 선택 중이면 클릭한 손패를 교체 대상으로 처리한다.
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                rewardManager.SelectReplacement(this);  // 지금 누른 카드의 손패 자리를 보상 카드로 교체하도록 요청한다.

            return;
        }

        if (eventData.button == PointerEventData.InputButton.Right)  // 우클릭이면 현재 확대된 카드를 손으로 돌려보낸다.
        {
            CancelSelection();
            return;
        }

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (BattleResultManager.Instance != null &&
            BattleResultManager.Instance.IsBattleEnded)
            return;

        BattleTurnManager manager = BattleTurnManager.Instance;

        if (manager == null || !manager.CanPlayCard)
            return;

        if (isSelected)  // 이미 확대된 카드를 다시 눌렀다면 기술 사용으로 진행한다.
        {
            UseSkill();
            return;
        }

        CancelSelection();
        SelectCard();
    }

    private void SelectCard()  // 손패에서 원래 위치를 기억하고 카드를 화면 중앙에 크게 표시한다.
    {
        // 원래 위치와 크기 저장
        originalParent = rect.parent;  // 확대가 끝났을 때 돌아갈 손패의 부모 오브젝트를 기억한다.
        originalIndex = rect.GetSiblingIndex();  // 부모 안에서의 원래 순서를 저장해 복귀할 때 그 자리를 찾는다.

        originalAnchorMin = rect.anchorMin;
        originalAnchorMax = rect.anchorMax;
        originalPivot = rect.pivot;
        originalSize = rect.sizeDelta;
        originalPosition = rect.anchoredPosition3D;
        originalScale = rect.localScale;
        originalRotation = rect.localRotation;

        // Canvas 바로 아래로 이동하여 다른 UI 위에 표시
        rect.SetParent(rootCanvasRect, false);  // 손패에서 꺼내 최상위 Canvas를 기준으로 중앙에 배치할 준비를 한다.
        rect.SetAsLastSibling();

        // 화면 중앙으로 이동
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.sizeDelta = originalSize;
        rect.anchoredPosition3D = Vector3.zero;  // 중앙 앵커를 기준으로 위치 차이를 0으로 맞춘다.
        rect.localRotation = Quaternion.identity;
        rect.localScale = Vector3.one * zoomScale;

        selectedCard = this;  // 다른 카드를 클릭할 때 찾아올 수 있도록 현재 선택을 공유한다.
        isSelected = true;
    }

    private void ReturnToHand()  // 저장해 둔 부모, 순서, 위치, 크기로 되돌리고 선택을 해제한다.
    {
        // 원래 부모와 순서 복구
        rect.SetParent(originalParent, false);
        rect.SetSiblingIndex(originalIndex);  // 확대 전 손패에서 차지하던 순서로 돌려놓는다.

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

    private void UseSkill()  // 이 카드가 가진 데이터를 턴 관리자에게 넘겨 공격을 요청한다.
    {
        BattleTurnManager manager = BattleTurnManager.Instance;

        if (manager == null)
        {
            Debug.LogWarning("씬에 BattleTurnManager가 없어.", this);
            return;
        }

        if (manager.TryUsePlayerCard(cardData))  // 공격이 실제로 접수된 경우에만 카드를 원래 위치로 돌린다.
            ReturnToHand();
    }

    private void OnDisable()  // 카드가 비활성화될 때 남아 있는 선택 상태를 정리한다.
    {
        if (isSelected && originalParent != null)
            ReturnToHand();

        if (selectedCard == this)
            selectedCard = null;
    }

    public static void CancelSelection()  // 확대된 카드가 있으면 원래 손패 자리로 되돌리는 공용 함수다.
    {
        if (selectedCard != null)
            selectedCard.ReturnToHand();
    }
}