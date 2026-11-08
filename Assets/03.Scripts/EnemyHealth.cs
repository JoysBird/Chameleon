using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealth : MonoBehaviour //이것은 프리팹의 HP를 설정하고 조정하는 ui를 만들기위함입니다.
{
    [Header("HP 설정")]
    public int maxHP;
    private int currentHP;

    [Header("HP UI")]
    public Slider hpSlider;
    public TMP_Text hpText;

    // 적 데이터 적용
    public void Initialize(EnemyData data)
    {
        maxHP = data.maxHP;
        currentHP = maxHP;
    }
}