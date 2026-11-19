using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class RewardCardManager : MonoBehaviour
{
    public static RewardCardManager Instance { get; private set; }

    [Header("보상 패널")]
    [SerializeField] private GameObject rewardPanel;

    [Header("현재 손패를 왼쪽부터 연결")]
    [SerializeField] private CardUI[] handCards = new CardUI[4];

    [Header("기존 적 카드 표시 컴포넌트")]
    [SerializeField] private EnemyCardUI enemyCardUI;

    [Header("교체 안내 텍스트")]
    [SerializeField] private TMP_Text selectionText;

    private static CardData[] savedCards = new CardData[4];
    private static GameObject[] savedPrefabs = new GameObject[4];

    private CardData rewardCard;
    private GameObject rewardPrefab;
    private bool rewardStarted;

    public bool IsSelecting { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ResetSavedCards()
    {
        savedCards = new CardData[4];
        savedPrefabs = new GameObject[4];
    }

    private void Awake()
    {
        Instance = this;

        if (selectionText != null)
            selectionText.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (!ValidateHand())
            return;

        for (int i = 0; i < handCards.Length; i++)
        {
            if (savedCards[i] != null && savedPrefabs[i] != null)
                ReplaceCard(i, savedPrefabs[i], savedCards[i]);
        }
    }

    private bool ValidateHand()
    {
        if (handCards == null || handCards.Length != 4)
        {
            Debug.LogWarning("손패 4장을 연결해줘.", this);
            return false;
        }

        foreach (CardUI card in handCards)
        {
            if (card == null)
            {
                Debug.LogWarning("손패에 비어 있는 칸이 있어.", this);
                return false;
            }
        }

        return true;
    }

    public void BeginCardReward()
    {
        if (rewardStarted || !ValidateHand())
            return;

        BattleResultManager result = BattleResultManager.Instance;

        if (result == null || !result.IsBattleEnded)
            return;

        EnemyHealth enemy = FindFirstObjectByType<EnemyHealth>();

        if (enemy == null || enemy.CurrentHP > 0)
            return;

        EnemyCard enemyCards = enemy.GetComponent<EnemyCard>();

        if (enemyCards == null || enemyCardUI == null || rewardPanel == null)
        {
            Debug.LogWarning("적 카드와 보상 UI 연결을 확인해줘.", this);
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded("03MapScene"))
        {
            Debug.LogWarning("03MapScene을 빌드 씬 목록에 등록해줘.", this);
            return;
        }

        rewardCard = enemyCards.ChooseCard();

        if (rewardCard == null)
        {
            Debug.LogWarning("적의 카드 풀이 비어 있어.", this);
            return;
        }

        rewardPrefab = FindPrefab(rewardCard);

        if (rewardPrefab == null ||
            rewardPrefab.GetComponent<CardUI>() == null ||
            rewardPrefab.GetComponent<RectTransform>() == null ||
            rewardPrefab.GetComponent<Button>() == null)
        {
            Debug.LogWarning("보상 카드에 연결된 프리팹을 확인해줘.", this);
            return;
        }

        CardUI.CancelSelection();

        rewardStarted = true;
        IsSelecting = true;

        rewardPanel.SetActive(false);
        enemyCardUI.ShowCard(rewardCard);

        if (selectionText != null)
        {
            selectionText.text =
                $"{rewardCard.name} 획득\n교체할 손패 카드를 클릭하세요.";

            selectionText.gameObject.SetActive(true);
        }
    }

    private GameObject FindPrefab(CardData data)
    {
        if (enemyCardUI.cardPreviews == null)
            return null;

        foreach (EnemyCardUI.CardPreview preview in enemyCardUI.cardPreviews)
        {
            if (preview != null && preview.cardData == data)
                return preview.cardPrefab;
        }

        return null;
    }

    public void SelectReplacement(CardUI selectedCard)
    {
        if (!IsSelecting)
            return;

        int index = System.Array.IndexOf(handCards, selectedCard);

        if (index < 0)
            return;

        IsSelecting = false;

        ReplaceCard(index, rewardPrefab, rewardCard);

        savedCards[index] = rewardCard;
        savedPrefabs[index] = rewardPrefab;

        SceneManager.LoadScene("03MapScene");
    }

    private void ReplaceCard(int index, GameObject prefab, CardData data)
    {
        CardUI oldCard = handCards[index];
        RectTransform oldRect = oldCard.GetComponent<RectTransform>();

        GameObject newObject = Instantiate(prefab, oldRect.parent, false);
        RectTransform newRect = newObject.GetComponent<RectTransform>();
        CardUI newCard = newObject.GetComponent<CardUI>();

        newCard.cardData = data;
        newCard.zoomScale = oldCard.zoomScale;
        newCard.enabled = true;

        newRect.anchorMin = oldRect.anchorMin;
        newRect.anchorMax = oldRect.anchorMax;
        newRect.pivot = oldRect.pivot;
        newRect.sizeDelta = oldRect.sizeDelta;
        newRect.anchoredPosition3D = oldRect.anchoredPosition3D;
        newRect.localScale = oldRect.localScale;
        newRect.localRotation = oldRect.localRotation;
        newRect.SetSiblingIndex(oldRect.GetSiblingIndex());

        newObject.GetComponent<Button>().interactable = true;
        newObject.SetActive(true);

        handCards[index] = newCard;

        oldCard.gameObject.SetActive(false);
        Destroy(oldCard.gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}