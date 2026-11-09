using System.Collections;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    public static BattleTurnManager Instance { get; private set; }

    public PlayerHealth player;
    public float enemyActionDelay = 0.7f;

    public bool CanPlayCard { get; private set; } = true;

    private void Awake()
    {
        Instance = this;
    }

    public bool TryUsePlayerCard(CardData card)
    {
        if (!CanPlayCard || card == null || player == null)
            return false;

        if (player.IsDead)
            return false;

        EnemyHealth enemy = FindFirstObjectByType<EnemyHealth>();

        if (enemy == null || enemy.CurrentHP <= 0)
            return false;

        EnemySkills skills = enemy.GetComponent<EnemySkills>();

        if (skills == null)
        {
            Debug.LogWarning("적 프리팹에 EnemySkills를 붙여줘.");
            return false;
        }

        // 즉시 잠가서 연속 클릭으로 중복 사용하지 못하게 한다.
        CanPlayCard = false;

        enemy.TakeDamage(card.damage);

        StartCoroutine(EnemyTurn(enemy, skills));

        return true;
    }

    private IEnumerator EnemyTurn(
        EnemyHealth enemy, EnemyCard skills)
    {
        if (enemy == null || enemy.CurrentHP <= 0)
        {
            Debug.Log("승리!");
            yield break;
        }

        yield return new WaitForSeconds(enemyActionDelay);

        if (enemy == null || enemy.CurrentHP <= 0)
        {
            Debug.Log("승리!");
            yield break;
        }

        if (player == null || player.IsDead)
            yield break;

        CardData enemyCard =
            skills != null ? skills.ChooseCard() : null;

        if (enemyCard != null)
        {
            Debug.Log($"적 기술 사용: {enemyCard.name}");
            player.TakeDamage(enemyCard.damage);
        }
        else
        {
            Debug.LogWarning("적에게 등록된 기술이 없어 턴을 넘겨.");
        }

        if (player.IsDead)
        {
            Debug.Log("패배!");
            yield break;
        }

        CanPlayCard = true;
        Debug.Log("플레이어 차례");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}