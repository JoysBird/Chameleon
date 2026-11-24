using UnityEngine;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    // Inspector에서 최초 플레이어의 최대 HP를 지정한다.
    [Header("HP 설정")]
    public int maxHP = 30;

    // 플레이어의 실제 HP 데이터를 관리하는 매니저를 참조한다.
    private PlayerDataManager playerData;

    // 다른 스크립트에서 현재 HP와 사망 여부를 확인할 수 있도록 한다.
    public int CurrentHP => playerData != null ? playerData.CurrentHP : 0;
    public bool IsDead => CurrentHP <= 0;

    // 현재 HP와 최대 HP를 표시할 텍스트를 연결한다.
    [Header("HP UI")]
    public TMP_Text hpText;

    // 전투 Scene에 진입하면 기존 플레이어 HP 데이터를 불러온다.
    private void Awake()
    {
        // 데이터가 없다면 생성하고, 존재한다면 기존 데이터를 가져온다.
        playerData = PlayerDataManager.GetOrCreate(maxHP);

        // 저장된 최대 HP를 현재 플레이어에게 반영한다.
        maxHP = playerData.MaxHP;

        // 불러온 HP를 화면에 표시한다.
        UpdateHPText();
    }

    // 현재 HP와 최대 HP를 화면의 HP 텍스트에 반영한다.
    private void UpdateHPText()
    {
        if (hpText != null)
            hpText.text = $"{CurrentHP}/{playerData.MaxHP}";
    }

    // 플레이어가 받은 피해를 저장된 HP 데이터에 적용한다.
    public void TakeDamage(int damage)
    {
        // 이미 사망했다면 추가 피해를 처리하지 않는다.
        if (IsDead)
            return;

        // 데이터 매니저에 저장된 현재 HP를 감소시킨다.
        playerData.TakeDamage(damage);

        // 변경된 HP를 화면에 표시한다.
        UpdateHPText();

        // 플레이어가 사망하면 기존 전투 패배 처리를 실행한다.
        if (IsDead && BattleResultManager.Instance != null)
        {
            BattleResultManager.Instance.ShowDefeat();
        }
    }

    // 플레이어의 HP를 회복하고 회복 적용 여부를 반환한다.
    public bool Heal(int amount)
    {
        // 데이터 매니저를 통해 HP 회복을 시도한다.
        bool isHealed = playerData.Heal(amount);

        // 회복되지 않았다면 포션이 소모되지 않도록 false를 반환한다.
        if (!isHealed)
            return false;

        // 회복된 HP를 화면에 표시한다.
        UpdateHPText();

        return true;
    }
}