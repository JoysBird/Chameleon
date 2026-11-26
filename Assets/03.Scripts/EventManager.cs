using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    // Inspector에서 등장할 이벤트 데이터 목록을 등록한다.
    [Header("이벤트 목록")]
    public EventData[] eventList;

    // 왼쪽에 표시할 이벤트 이미지와 설명을 연결한다.
    [Header("이벤트 정보 UI")]
    public Image eventImage;
    public TMP_Text eventDescription;

    // 오른쪽에 배치된 선택지 버튼을 연결한다.
    [Header("선택지 UI")]
    public Button[] choiceButtons;

    // 실제 이벤트 효과를 실행할 매니저를 연결한다.
    [Header("이벤트 효과")]
    public EventEffectManager effectManager;

    // 이벤트가 종료되면 돌아갈 Scene 이름을 지정한다.
    [Header("복귀 Scene")]
    public string mapSceneName = "03MapScene";

    // 현재 등장한 이벤트와 이벤트 종료 여부를 저장한다.
    private EventData currentEvent;
    private bool eventFinished;

    // EventScene에 진입하면 버튼 기능을 연결하고 랜덤 이벤트를 불러온다.
    private void Start()
    {
        // 필수 UI와 매니저가 연결되어 있는지 검사한다.
        if (eventImage == null || eventDescription == null ||
            choiceButtons == null || effectManager == null)
        {
            Debug.LogWarning("EventManager의 Inspector 연결을 확인해줘.");
            return;
        }

        // 각 버튼에 해당하는 선택지 실행 기능을 연결한다.
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (choiceButtons[i] == null)
                continue;

            int choiceIndex = i;

            choiceButtons[i].onClick.AddListener(
                () => SelectChoice(choiceIndex)
            );
        }

        LoadRandomEvent();
    }

    // 등록된 이벤트 목록에서 하나를 무작위로 선택한다.
    private void LoadRandomEvent()
    {
        List<EventData> validEvents = new List<EventData>();

        // 연결되지 않은 이벤트 데이터는 제외한다.
        if (eventList != null)
        {
            foreach (EventData data in eventList)
            {
                if (data != null)
                    validEvents.Add(data);
            }
        }

        // 등록된 이벤트가 없다면 실행을 중단한다.
        if (validEvents.Count == 0)
        {
            Debug.LogWarning("등록된 이벤트가 없습니다.");
            return;
        }

        // 이벤트를 무작위로 선택하고 화면에 표시한다.
        int randomIndex = Random.Range(0, validEvents.Count);

        currentEvent = validEvents[randomIndex];

        UpdateEventUI();
    }

    // 현재 이벤트의 이미지, 설명, 선택지를 화면에 표시한다.
    private void UpdateEventUI()
    {
        // 왼쪽 이벤트 이미지와 설명을 변경한다.
        eventImage.sprite = currentEvent.eventImage;
        eventDescription.text = currentEvent.description;

        // 오른쪽 선택지 버튼의 내용을 변경한다.
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (choiceButtons[i] == null)
                continue;

            // 현재 이벤트에 해당 선택지가 있는지 검사한다.
            bool hasChoice = currentEvent.choices != null
                && i < currentEvent.choices.Length
                && currentEvent.choices[i] != null;

            choiceButtons[i].gameObject.SetActive(hasChoice);

            if (!hasChoice)
                continue;

            // 해당 버튼에 표시할 선택지 데이터를 가져온다.
            EventChoice choice = currentEvent.choices[i];

            TMP_Text buttonText =
                choiceButtons[i].GetComponentInChildren<TMP_Text>(true);

            // 선택지 이름과 효과 설명을 두 줄로 표시한다.
            if (buttonText != null)
            {
                buttonText.text =
                    choice.choiceText + "\n" + choice.effectDescription;
            }
        }

        Debug.Log("현재 이벤트: " + currentEvent.eventName);
    }

    // 플레이어가 선택지 버튼을 클릭했을 때 실행한다.
    public void SelectChoice(int choiceIndex)
    {
        // 이미 종료된 이벤트이거나 잘못된 선택지는 실행하지 않는다.
        if (eventFinished || currentEvent == null ||
            currentEvent.choices == null)
            return;

        if (choiceIndex < 0 || choiceIndex >= currentEvent.choices.Length)
            return;

        EventChoice selectedChoice = currentEvent.choices[choiceIndex];

        if (selectedChoice == null)
            return;

        // 복귀할 Scene이 등록되어 있는지 먼저 확인한다.
        if (!Application.CanStreamedLevelBeLoaded(mapSceneName))
        {
            Debug.LogWarning("복귀할 Scene을 Build Profiles에 등록해줘.");
            return;
        }

        // 선택지에 등록된 효과를 실행한다.
        bool success = effectManager.ApplyEffects(selectedChoice.effects);

        // 효과 적용에 실패하면 현재 이벤트 화면을 유지한다.
        if (!success)
            return;

        // 이벤트가 중복 실행되지 않도록 종료 상태를 기록한다.
        eventFinished = true;

        // 모든 선택지 버튼을 비활성화한다.
        foreach (Button button in choiceButtons)
        {
            if (button != null)
                button.interactable = false;
        }

        // 이벤트를 종료하고 노드맵으로 돌아간다.
        SceneManager.LoadScene(03MapScene);
    }
}