using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemUseButton : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(UseItem);
    }

    public void UseItem()
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
            if (!player.Heal(itemData.effectValue))
                return;

            button.interactable = false;
            inventory.RemoveItem(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(UseItem);
    }
}