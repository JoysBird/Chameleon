using UnityEngine;
using UnityEngine.UI;

// 포션 프리팹에 사용 스크립트를 붙이면 클릭을 받을 Button도 함께 준비된다.
[RequireComponent(typeof(Button))]
public class ItemUseButton : MonoBehaviour
{
    // private 필드를 Inspector에 노출해 이 버튼이 사용할 아이템 데이터 에셋을 연결한다.
    [SerializeField] private ItemData itemData;

    private Button button;

    private void Awake()  // 인벤토리에 만들어진 아이템 버튼에 사용 함수를 연결한다.
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(UseItem);  // 플레이어가 이 버튼을 누르면 UseItem이 호출되도록 등록한다.
    }

    public void UseItem()  // 사용 가능한 상태인지 확인하고 아이템 효과가 적용되면 해당 슬롯을 비운다.
    {
        if (itemData == null)
            return;

        PlayerHP player = FindFirstObjectByType<PlayerHP>();
        InventoryManager inventory = GetComponentInParent<InventoryManager>();

        if (player == null || inventory == null)
            return;

        if (BattleResultManager.Instance != null &&
            BattleResultManager.Instance.IsBattleEnded)
            return;

        if (itemData.effectType == ItemEffectType.HealHP)
        {
            if (!player.Heal(itemData.effectValue))  // 최대 체력 등으로 회복에 실패하면 포션을 그대로 둔다.
                return;

            button.interactable = false;
            inventory.RemoveItem(gameObject);  // 회복에 성공했으므로 클릭한 포션을 슬롯과 저장 목록에서 제거한다.
        }
    }

    private void OnDestroy()  // 이 컴포넌트가 파괴될 때 Unity가 호출하는 정리 함수다.
    {
        if (button != null)
            button.onClick.RemoveListener(UseItem);  // 버튼에 등록했던 아이템 사용 함수를 해제한다.
    }
}