using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    // 적이 생성될 때 현재 체력으로 사용할 최대 HP를 지정한다.
    [Header("HP 설정")]
    public int maxHP;
    // 전투 스크립트가 남은 체력을 읽는 속성이다. 실제 값은 TakeDamage에서 갱신한다.
    public int CurrentHP => currentHP;
    private int currentHP;

    // 현재 HP/최대 HP를 표시할 적 프리팹의 텍스트를 연결한다.
    [Header("HP UI")]
    public TMP_Text hpText;

    void Awake()  // 적 프리팹이 생성될 때 Inspector의 최대 HP로 전투를 시작한다.
    {
        currentHP = maxHP;
        UpdateHPText();  // 방금 설정한 체력 수치로 화면 표시도 갱신한다.
    }

    private void UpdateHPText()  // 현재 체력과 최대 체력을 화면의 HP 텍스트에 반영한다.
    {
        if (hpText != null)
            hpText.text = $"{currentHP}/{maxHP}";
    }

    public void TakeDamage(int damage)  // 받은 피해를 체력에 적용하고 HP가 0이 되면 전투 결과를 알린다.
    {
        if (currentHP <= 0)
            return;

        currentHP -= Mathf.Max(0, damage);  // 음수 피해는 0으로 처리해 공격이 적의 체력을 회복시키지 않게 한다.
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPText();  // 방금 설정한 체력 수치로 화면 표시도 갱신한다.

        if (currentHP <= 0 && BattleResultManager.Instance != null)
        {
            BattleResultManager.Instance.ShowVictory();  // 적의 HP가 0이 되었음을 알려 승리 처리를 시작한다.
        }
    }
}