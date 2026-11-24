using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    // 다른 스크립트에서도 플레이어 데이터에 접근할 수 있도록 한다.
    public static PlayerDataManager Instance { get; private set; }

    // 플레이어의 최대 HP와 현재 HP를 저장한다.
    public int MaxHP { get; private set; }
    public int CurrentHP { get; private set; }

    // 플레이어의 사망 여부와 HP 초기화 여부를 확인한다.
    public bool IsDead => CurrentHP <= 0;
    private bool isInitialized;

    // 데이터 관리 오브젝트가 Scene 이동 후에도 유지되도록 설정한다.
    private void Awake()
    {
        // 이미 데이터 관리 오브젝트가 존재하면 중복 오브젝트를 제거한다.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Scene이 변경되어도 해당 오브젝트를 삭제하지 않는다.
        DontDestroyOnLoad(gameObject);
    }

    // 데이터 관리 오브젝트가 없다면 자동으로 생성한다.
    public static PlayerDataManager GetOrCreate(int initialMaxHP = 30)
    {
        if (Instance == null)
        {
            GameObject manager = new GameObject("PlayerDataManager");
            manager.AddComponent<PlayerDataManager>();
        }

        // HP가 한 번도 초기화되지 않았을 때만 초기화한다.
        if (!Instance.isInitialized)
        {
            Instance.ResetHP(initialMaxHP);
        }

        return Instance;
    }

    // 새로운 게임을 시작할 때 플레이어 HP를 초기화한다.
    public void ResetHP(int maxHP)
    {
        MaxHP = Mathf.Max(1, maxHP);
        CurrentHP = MaxHP;
        isInitialized = true;
    }

    // 플레이어에게 피해를 적용하고 HP가 0 미만으로 내려가지 않도록 한다.
    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        CurrentHP -= Mathf.Max(0, damage);
        CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);
    }

    // 플레이어의 HP를 회복하고 회복 적용 여부를 반환한다.
    public bool Heal(int amount)
    {
        // 사망 상태이거나 이미 최대 HP라면 회복하지 않는다.
        if (IsDead || CurrentHP >= MaxHP || amount <= 0)
            return false;

        // 최대 HP를 초과하지 않도록 회복량을 적용한다.
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);

        return true;
    }
}