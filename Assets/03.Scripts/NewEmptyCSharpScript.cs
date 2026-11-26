using UnityEngine;
using System.Collections.Generic;

public class EventEffectManager : MonoBehaviour
{
    // 이벤트에서 무작위로 획득할 수 있는 아이템 프리팹 목록이다.
    [Header("획득 가능한 아이템")]
    public GameObject[] itemPrefabs;

    // 선택지에 등록된 모든 효과가 적용 가능한지 확인하고 실행한다.
    public bool ApplyEffects(EventEffect[] effects)
    {
        // 현재 플레이어의 저장된 HP 데이터를 가져온다.
        PlayerDataManager playerData = PlayerDataManager.GetOrCreate();

        if (playerData.IsDead)
            return false;

        if (effects == null)
            return true;

        // 실제 효과를 적용하기 전에 예상 HP와 지급할 아이템을 준비한다.
        int expectedHP = playerData.CurrentHP;
        List<GameObject> rewards = new List<GameObject>();

        // 모든 효과를 먼저 검사하여 실행 가능한지 확인한다.
        foreach (EventEffect effect in effects)
        {
            if (effect == null || effect.value < 0)
                return false;

            switch (effect.effectType)
            {
                case EventEffectType.None:
                    break;

                // HP 소모 후에도 플레이어가 살아 있는지 확인한다.
                case EventEffectType.LoseHP:
                    if (expectedHP <= effect.value)
                    {
                        Debug.LogWarning("HP가 부족합니다.");
                        return false;
                    }

                    expectedHP -= effect.value;
                    break;

                // 회복 이후의 예상 HP를 계산한다.
                case EventEffectType.HealHP:
                    expectedHP = Mathf.Min(
                        expectedHP + effect.value,
                        playerData.MaxHP
                    );
                    break;

                // 획득할 무작위 아이템을 미리 추첨한다.
                case EventEffectType.GetRandomItem:
                    if (effect.value > 4 - rewards.Count)
                        return false;

                    for (int i = 0; i < effect.value; i++)
                    {
                        GameObject item = GetRandomItem();

                        if (item == null)
                            return false;

                        rewards.Add(item);
                    }
                    break;

                // 카드 획득은 기존 카드 교체 시스템과 연결한 뒤 구현한다.
                case EventEffectType.GetRandomCard:
                    Debug.LogWarning("카드 획득 기능은 아직 연결되지 않았습니다.");
                    return false;

                default:
                    return false;
            }
        }

        // 지급할 아이템이 있다면 인벤토리에 저장한다.
        // 저장에 실패하면 HP도 감소시키지 않는다.
        if (rewards.Count > 0)
        {
            if (!InventoryManager.TryStoreItems(rewards.ToArray()))
                return false;
        }

        // 모든 검사가 완료되면 실제 HP 감소 및 회복 효과를 적용한다.
        foreach (EventEffect effect in effects)
        {
            switch (effect.effectType)
            {
                case EventEffectType.LoseHP:
                    playerData.TakeDamage(effect.value);
                    break;

                case EventEffectType.HealHP:
                    playerData.Heal(effect.value);
                    break;
            }
        }

        // 현재 HP와 획득한 아이템 개수를 Console에 출력한다.
        Debug.Log($"현재 HP: {playerData.CurrentHP}/{playerData.MaxHP}");
        Debug.Log($"획득한 아이템: {rewards.Count}개");

        return true;
    }

    // 등록된 아이템 목록에서 유효한 프리팹 하나를 무작위로 반환한다.
    private GameObject GetRandomItem()
    {
        List<GameObject> validItems = new List<GameObject>();

        // 연결되지 않았거나 UI 프리팹이 아닌 항목을 제외한다.
        if (itemPrefabs != null)
        {
            foreach (GameObject item in itemPrefabs)
            {
                if (item != null && item.GetComponent<RectTransform>() != null)
                    validItems.Add(item);
            }
        }

        // 등록된 아이템이 없다면 경고를 출력한다.
        if (validItems.Count == 0)
        {
            Debug.LogWarning("등록된 아이템이 없습니다.");
            return null;
        }

        // 유효한 아이템 중 하나를 무작위로 선택한다.
        int randomIndex = Random.Range(0, validItems.Count);

        return validItems[randomIndex];
    }
}