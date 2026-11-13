using UnityEngine;
using TMPro;

public class PlayerHP : MonoBehaviour
{
    [Header("HP ¼³Á¤")]
    public int maxHP = 30;
    public int CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    [Header("HP UI")]
    public TMP_Text hpText;

    private void Awake()
    {
        CurrentHP = maxHP;
        UpdateHPText();
    }

    private void UpdateHPText()
    {
        if (hpText != null)
            hpText.text = $"{CurrentHP}/{maxHP}";
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        CurrentHP -= Mathf.Max(0, damage);
        CurrentHP = Mathf.Clamp(CurrentHP, 0, maxHP);

        UpdateHPText();

        if (IsDead && BattleResultManager.Instance != null)
        {
            BattleResultManager.Instance.ShowDefeat();
        }
    }
}