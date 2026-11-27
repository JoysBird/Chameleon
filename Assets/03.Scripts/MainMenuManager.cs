using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // 메인 메뉴에서 이동할 씬 이름을 Inspector에 표시한다.
    [Header("이동할 씬")]
    [SerializeField] private string mapSceneName = "03MapScene";
    [SerializeField] private string optionSceneName = "02OptionScene";

    // 저장 기능을 연결하기 전까지 비활성화할 Continue 버튼이다.
    [Header("이어하기 버튼")]
    [SerializeField] private Button continueButton;

    // 메인 메뉴가 열리면 Continue 버튼을 눌리지 않는 상태로 표시한다.
    private void Awake()
    {
        if (continueButton != null)
        {
            continueButton.interactable = false;
        }
    }

    // Start 버튼을 누르면 노드 맵으로 이동한다.
    public void StartGame()
    {
        SceneManager.LoadScene(mapSceneName);
    }

    // Option 버튼을 누르면 옵션 씬으로 이동한다.
    public void OpenOptions()
    {
        SceneManager.LoadScene(optionSceneName);
    }

    // Quit 버튼을 누르면 에디터에서는 Play 모드를 끝내고, 빌드에서는 게임을 종료한다.
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}