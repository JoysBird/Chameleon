using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour //이것은 프리팹의 HP를 설정하고 조정하는 텍스트와 ui를 만들기 위함입니다.
{
    [Header("HP 설정")]
    public int maxHP;
    private int currentHP;

    [Header("HP UI")]
    public TMP_Text hpText;

    public void Initialize(EnemyData data)
    {
        maxHP = data.maxHP;
        currentHP = maxHP;

        UpdateHPText();
    }

    private void UpdateHPText()
    {
        if (hpText == null)
        {
            Debug.LogError("HP Text가 연결되지 않았습니다!");
            return;
        }

        hpText.text = $"{currentHP}/{maxHP}";
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPText();
    }
}