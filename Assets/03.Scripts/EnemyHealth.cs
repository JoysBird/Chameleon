using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    [Header("HP 설정")]
    public int maxHP;
    public int CurrentHP => currentHP;
    private int currentHP;

    [Header("HP UI")]
    public TMP_Text hpText;

    void Awake()
    {
        currentHP = maxHP;
        UpdateHPText();
    }

    private void UpdateHPText()
    {
        if (hpText != null)
            hpText.text = $"{currentHP}/{maxHP}";
    }

    public void TakeDamage(int damage)
    {
        if (currentHP <= 0)
            return;

        currentHP -= Mathf.Max(0, damage);
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        UpdateHPText();

        if (currentHP <= 0 && BattleResultManager.Instance != null)
        {
            BattleResultManager.Instance.ShowVictory();
        }
    }
}