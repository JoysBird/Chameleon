using UnityEngine;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    // Inspector에서 플레이어의 최대 HP를 지정한다. 전투 시작 시 현재 HP를 이 값으로 채운다.
    [Header("HP 설정")]
    public int maxHP = 30;
    // 다른 스크립트는 현재 체력을 읽을 수 있고, 값 변경은 PlayerHP 안에서만 가능하다.
    public int CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    // 피해나 회복 후 현재 HP/최대 HP를 표시할 텍스트를 연결한다.
    [Header("HP UI")]
    public TMP_Text hpText;

    private void Awake()  // 플레이어가 전투 씬에 생성될 때 현재 체력과 HP 텍스트를 초기화한다.
    {
        CurrentHP = maxHP;
        UpdateHPText();  // 방금 설정한 체력 수치로 화면 표시도 갱신한다.
    }

    private void UpdateHPText()  // 현재 체력과 최대 체력을 화면의 HP 텍스트에 반영한다.
    {
        if (hpText != null)
            hpText.text = $"{CurrentHP}/{maxHP}";
    }

    public void TakeDamage(int damage)  // 받은 피해를 체력에 적용하고 HP가 0이 되면 전투 결과를 알린다.
    {
        if (IsDead)
            return;

        CurrentHP -= Mathf.Max(0, damage);  // 음수 피해는 무시하고 실제 받은 양만 플레이어 HP에서 뺀다.
        CurrentHP = Mathf.Clamp(CurrentHP, 0, maxHP);

        UpdateHPText();  // 방금 설정한 체력 수치로 화면 표시도 갱신한다.

        if (IsDead && BattleResultManager.Instance != null)
        {
            BattleResultManager.Instance.ShowDefeat();  // 플레이어 사망을 결과 관리자에게 알려 패배 처리를 시작한다.
        }
    }

    public bool Heal(int amount)  // 회복이 적용되었는지 반환해 포션을 소모할지 결정하게 한다.
    {
        if (IsDead || CurrentHP >= maxHP || amount <= 0)
            return false;

        CurrentHP = Mathf.Min(CurrentHP + amount, maxHP);  // 회복량을 더하되 최대 HP를 넘지 않게 한다.
        UpdateHPText();  // 방금 설정한 체력 수치로 화면 표시도 갱신한다.

        return true;
    }
}