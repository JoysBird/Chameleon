using UnityEngine;

// Project 창에서 새로운 이벤트 데이터 에셋을 생성할 수 있도록 설정한다.
[CreateAssetMenu(fileName = "EventData", menuName = "Game/Event Data")]
public class EventData : ScriptableObject
{
    // 이벤트의 이름과 일러스트를 저장한다.
    [Header("이벤트 정보")]
    public string eventName;
    public Sprite eventImage;

    // 이벤트 화면 왼쪽에 표시할 설명을 저장한다.
    [TextArea(3, 6)]
    public string description;

    // 이벤트 화면 오른쪽에 표시할 선택지 목록을 저장한다.
    [Header("이벤트 선택지")]
    public EventChoice[] choices;
}

// 각 선택지에 필요한 정보를 저장한다.
[System.Serializable]
public class EventChoice
{
    // 버튼에 표시할 선택지 이름을 저장한다.
    public string choiceText;

    // 버튼에 표시할 효과 설명을 저장한다.
    [TextArea(2, 4)]
    public string effectDescription;

    // 선택지를 클릭했을 때 실행할 효과 목록을 저장한다.
    public EventEffect[] effects;
}

// 선택지에 적용되는 효과의 종류와 수치를 저장한다.
[System.Serializable]
public class EventEffect
{
    // 적용할 효과의 종류를 지정한다.
    public EventEffectType effectType;

    // HP 감소량, 회복량, 획득할 아이템 개수 등을 지정한다.
    public int value;
}

// 이벤트에서 사용할 수 있는 효과의 종류를 정의한다.
public enum EventEffectType
{
    None,
    LoseHP,
    HealHP,
    GetRandomCard,
    GetRandomItem
}