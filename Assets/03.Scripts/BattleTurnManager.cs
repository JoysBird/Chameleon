using System.Collections;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    // public static이므로 카드마다 참조를 연결하지 않아도 BattleTurnManager.Instance로 현재 전투의 턴 관리자를 찾을 수 있다.
    public static BattleTurnManager Instance { get; private set; }

    public PlayerHP player;
    public float enemyActionDelay = 0.7f;

    // CardUI가 이 값을 읽어 클릭을 허용한다. 턴 관리자가 private set으로 적 행동 중 입력을 잠근다.
    public bool CanPlayCard { get; private set; } = true;

    private void Awake()  // 전투가 시작될 때 카드가 찾아 쓸 턴 관리자를 등록한다.
    {
        Instance = this;  // this는 지금 실행 중인 컴포넌트 자신이다. 다른 스크립트가 Instance로 이 관리자에 접근할 수 있게 한다.
    }

    public bool TryUsePlayerCard(CardData card)  // 카드 사용 조건을 검사하고 공격이 시작되면 true를 돌려준다.
    {
        if (!CanPlayCard || card == null || player == null)
            return false;

        if (player.IsDead)
            return false;

        EnemyHealth enemy = FindFirstObjectByType<EnemyHealth>();

        if (enemy == null || enemy.CurrentHP <= 0)
            return false;

        EnemyCard skills = enemy.GetComponent<EnemyCard>();

        if (skills == null)
        {
            Debug.LogWarning("적 프리팹에 EnemyCard를 붙여줘.");
            return false;
        }

        // 즉시 잠가서 연속 클릭으로 중복 사용하지 못하게 한다.
        CanPlayCard = false;  // 적 행동이 끝날 때까지 추가 카드 입력을 잠가 중복 공격을 막는다.

        enemy.TakeDamage(card.damage);  // 선택한 카드 데이터의 피해량만큼 현재 적의 HP를 줄인다.

        StartCoroutine(EnemyTurn(enemy, skills));  // 방금 공격한 적과 그 적의 기술 목록을 넘겨 반격 순서를 시작한다.

        return true;
    }

    private IEnumerator EnemyTurn(  // 플레이어의 공격 결과를 확인하고 살아 있는 적의 행동을 진행하는 코루틴이다.
        EnemyHealth enemy, EnemyCard skills)
    {
        if (enemy == null || enemy.CurrentHP <= 0)
        {
            EnemyCardUI ui = FindFirstObjectByType<EnemyCardUI>();
            if (ui != null)
                ui.ShowCard(null);  // 적이 쓰러졌으므로 이전에 표시한 다음 행동 카드를 지운다.

            Debug.Log("승리!");
            yield break;
        }

        yield return new WaitForSeconds(enemyActionDelay);  // 플레이어 공격과 적 반격이 구분되어 보이도록 설정한 시간만큼 기다린다.

        if (enemy == null || enemy.CurrentHP <= 0)
        {
            EnemyCardUI ui = FindFirstObjectByType<EnemyCardUI>();
            if (ui != null)
                ui.ShowCard(null);  // 적이 쓰러졌으므로 이전에 표시한 다음 행동 카드를 지운다.

            Debug.Log("승리!");
            yield break;
        }

        if (player == null || player.IsDead)
            yield break;

        CardData enemyCard =
            skills != null ? skills.NextCard : null;

        if (enemyCard != null)
        {
            Debug.Log($"적 기술 사용: {enemyCard.name}");
            player.TakeDamage(enemyCard.damage);  // 적이 예고한 카드의 피해량을 플레이어에게 적용한다.
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

        if (skills != null)
            skills.PrepareNextCard();  // 살아 있는 적이 다음 차례에 쓸 카드를 다시 뽑아 예고한다.

        CanPlayCard = true;  // 적 행동을 마쳤으므로 플레이어가 다시 카드를 누를 수 있다.
        Debug.Log("플레이어 차례");
    }

    private void OnDestroy()  // 이 컴포넌트가 파괴될 때 Unity가 호출하는 정리 함수다.
    {
        if (Instance == this)  // 공유 참조가 아직 이 관리자를 가리킬 때만 비운다.
            Instance = null;  // 다른 스크립트가 사라진 관리자를 참조하지 않도록 연결을 해제한다.
    }
}
