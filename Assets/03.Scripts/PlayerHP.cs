using TMPro;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    public int maxHP = 30;
    public TMP_Text hpText;

    public int CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    private void Awake()
    {
        CurrentHP = maxHP;
        UpdateHPText();
    }

    public void TakeDamage(int damage)
    {
        CurrentHP = Mathf.Clamp(
            CurrentHP - Mathf.Max(0, damage), 0, maxHP);

        UpdateHPText();
    }

    private void UpdateHPText()
    {
        if (hpText != null)
            hpText.text = $"{CurrentHP}/{maxHP}";
    }
}
