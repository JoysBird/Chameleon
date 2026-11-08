using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [Header("HP 설정")]
    public int maxHP; // 적의 최대 HP inspector에서 설정
    private int currentHP; // 적의 현재 HP 데미지를 받던..이런거

    [Header("HP UI")]
    public TMP_Text hpText; // HP를 표시할 TextMeshPro UI

    void Start()
    {
        currentHP = maxHP; // 현재 HP를 최대 HP로 초기화
        UpdateHPText(); // 초기 HP 텍스트 갱신
    }

    private void UpdateHPText()
    {
        hpText.text = $"{currentHP}/{maxHP}"; // 현재 HP와 최대 HP를 텍스트로 표시
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage; // 받은 피해만큼 현재 HP 감소
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // 현재 HP가 0 미만 또는 최대 HP 초과가 되지 않도록 제한

        UpdateHPText(); // 피해 적용 후 HP 텍스트 갱신
    }
}
