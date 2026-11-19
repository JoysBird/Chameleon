using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleResultManager : MonoBehaviour
{
    // 다른 스크립트는 BattleResultManager.Instance로 현재 전투의 결과 관리자에 접근한다. private set이므로 참조는 이 클래스 안에서만 바꾼다.
    public static BattleResultManager Instance { get; private set; }

    // Header는 Inspector에 제목을 표시한다. 아래 세 패널은 SerializeField 덕분에 private이어도 Inspector에서 연결할 수 있다.
    [Header("결과 화면")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject rewardPanel;

    // 승리·패배 패널을 띄운 뒤 다음 화면으로 넘어가기까지 기다릴 시간(초)이다.
    [Header("대기 시간")]
    [SerializeField] private float transitionDelay = 1.5f;

    // 다른 스크립트는 전투 종료 여부를 읽고, 승패 확정은 이 관리자에서 처리한다.
    public bool IsBattleEnded { get; private set; }

    private void Awake()  // 이 컴포넌트가 처음 활성화될 때 Unity가 호출한다. 결과 관리자를 등록하고 전투 시작 화면을 준비한다.
    {
        Instance = this;  // this는 지금 실행 중인 컴포넌트 자신이다. 다른 스크립트가 Instance로 이 관리자에 접근할 수 있게 한다.

        if (victoryPanel != null)  // Inspector에 승리 패널이 연결되어 있을 때만 바로 아래 줄을 실행한다.
            victoryPanel.SetActive(false);  // 승리 패널을 비활성화해 화면에서 숨긴다.

        if (defeatPanel != null)  // 패배 패널이 연결되어 있는지 확인해 빈 참조에서 함수를 호출하는 오류를 막는다.
            defeatPanel.SetActive(false);  // 전투가 시작될 때 패배 화면이 보이지 않게 한다.

        if (rewardPanel != null)  // 보상 패널이 연결되어 있을 때만 초기 상태를 설정한다.
            rewardPanel.SetActive(false);  // 전투가 시작되면 보상 선택 버튼도 패널과 함께 숨긴다.
    }

    public void ShowVictory()  // 적 HP가 0이 되었을 때 EnemyHealth가 호출하는 승리 처리 함수다.
    {
        if (IsBattleEnded)  // 이미 승패가 정해졌다면 아래의 return으로 두 번째 결과 처리를 막는다.
            return;

        IsBattleEnded = true;  // 이번 전투가 끝났다고 기록해 이후의 승패 요청을 막는다.

        ShowPanel(victoryPanel);  // 적이 쓰러졌을 때 승리 패널을 화면 맨 앞으로 가져온다.
        StartCoroutine(OpenRewardPanel());  // 승리 화면을 잠시 보여준 뒤 보상 패널을 여는 작업을 시작한다.
    }

    public void ShowDefeat()  // 플레이어 HP가 0이 되었을 때 PlayerHP가 호출하는 패배 처리 함수다.
    {
        if (IsBattleEnded)  // 이미 승패가 정해졌다면 아래의 return으로 두 번째 결과 처리를 막는다.
            return;

        IsBattleEnded = true;  // 이번 전투가 끝났다고 기록해 이후의 승패 요청을 막는다.

        ShowPanel(defeatPanel);  // 플레이어가 쓰러지면 패배 패널을 표시한다.
        StartCoroutine(MoveToDefeatScene());  // 패배 패널을 보여준 뒤 패배 씬으로 이동하는 작업을 시작한다.
    }

    private void ShowPanel(GameObject panel)  // 전달받은 패널을 켜고 같은 부모 아래 다른 UI보다 앞에 표시한다.
    {
        if (panel == null)  // 표시할 패널이 연결되지 않았다면 함수를 끝낸다.
            return;

        panel.SetActive(true);  // 해당 패널과 그 아래의 활성 자식 UI를 화면에 표시한다.
        panel.transform.SetAsLastSibling();  // 부모 아래의 마지막 순서로 옮겨 같은 Canvas에서 나중에 그려지게 한다.
    }

    private IEnumerator OpenRewardPanel()  // 승리 화면을 보상 선택 화면으로 바꾸는 코루틴이다.
    {
        yield return new WaitForSecondsRealtime(transitionDelay);  // 게임 속도와 관계없이 지정한 실제 시간만큼 이 코루틴만 기다린다.

        if (victoryPanel != null)  // Inspector에 승리 패널이 연결되어 있을 때만 바로 아래 줄을 실행한다.
            victoryPanel.SetActive(false);  // 승리 패널을 비활성화해 화면에서 숨긴다.

        ShowPanel(rewardPanel);  // 대기가 끝나면 플레이어가 보상을 고를 수 있도록 패널을 연다.
    }

    private IEnumerator MoveToDefeatScene()  // 패배 화면을 잠시 유지한 뒤 씬을 전환하는 코루틴이다.
    {
        yield return new WaitForSecondsRealtime(transitionDelay);  // 게임 속도와 관계없이 지정한 실제 시간만큼 이 코루틴만 기다린다.

        SceneManager.LoadScene("08DefeatScene");  // 현재 전투 씬을 닫고 패배 씬을 불러온다.
    }

    private void OnDestroy()  // 이 컴포넌트가 파괴될 때 Unity가 호출하는 정리 함수다.
    {
        if (Instance == this)  // 공유 참조가 아직 이 관리자를 가리킬 때만 비운다.
            Instance = null;  // 다른 스크립트가 사라진 관리자를 참조하지 않도록 연결을 해제한다.
    }
}