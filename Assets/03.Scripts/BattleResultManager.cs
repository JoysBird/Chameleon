using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleResultManager : MonoBehaviour
{
    public static BattleResultManager Instance { get; private set; }

    [Header("결과 화면")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

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
    }

    public void ShowVictory()
    {
        ShowResult(victoryPanel, "05RewardScene");
    }

    public void ShowDefeat()
    {
        ShowResult(defeatPanel, "08DefeatScene");
    }

    private void ShowResult(GameObject panel, string sceneName)
    {
        if (IsBattleEnded)
            return;

        IsBattleEnded = true;

        if (panel != null)
        {
            panel.SetActive(true);
            panel.transform.SetAsLastSibling();
        }

        StartCoroutine(ChangeScene(sceneName));
    }

    private IEnumerator ChangeScene(string sceneName)
    {
        yield return new WaitForSecondsRealtime(transitionDelay);

        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}