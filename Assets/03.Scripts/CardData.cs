using UnityEngine;

// Project 창의 Create 메뉴에서 카드 데이터 에셋을 만든다. 여러 카드가 같은 에셋의 피해량을 참조할 수 있다.
[CreateAssetMenu(fileName = "CardData", menuName = "Game/Card Data")]
public class CardData : ScriptableObject
{
    // Inspector에서 전투에 적용할 카드 피해량을 설정하는 구역이다.
    [Header("카드 효과")]
    public int damage;  // 이 카드를 사용했을 때 상대 HP에서 뺄 피해량이다.
}