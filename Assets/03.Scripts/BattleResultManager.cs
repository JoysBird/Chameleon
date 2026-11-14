using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleResultManager : MonoBehaviour
{
    public static BattleResultManager Instance { get; private set; }

    [Header("결과 화면")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject rewardPanel;

    [Header("대기 시간")]
    [SerializeField] private float transitionDelay = 1.5f;

    public bool IsBattleEnded { get; private set; }

    private void Awake()
    {
        Instance = this;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (defeatPanel != null)
            defeatPanel.SetActive(false);

        if (rewardPanel != null)
            rewardPanel.SetActive(false);
    }

    public void ShowVictory()
    {
        if (IsBattleEnded)
            return;

        IsBattleEnded = true;

        ShowPanel(victoryPanel);
        StartCoroutine(OpenRewardPanel());
    }

    public void ShowDefeat()
    {
        if (IsBattleEnded)
            return;

        IsBattleEnded = true;

        ShowPanel(defeatPanel);
        StartCoroutine(MoveToDefeatScene());
    }

    private void ShowPanel(GameObject panel)
    {
        if (panel == null)
            return;

        panel.SetActive(true);
        panel.transform.SetAsLastSibling();
    }

    private IEnumerator OpenRewardPanel()
    {
        yield return new WaitForSecondsRealtime(transitionDelay);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        ShowPanel(rewardPanel);
    }

    private IEnumerator MoveToDefeatScene()
    {
        yield return new WaitForSecondsRealtime(transitionDelay);

        SceneManager.LoadScene("08DefeatScene");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}