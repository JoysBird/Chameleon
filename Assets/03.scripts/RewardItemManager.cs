using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardItemManager : MonoBehaviour
{
    [Header("인벤토리")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("아이템 보상 버튼")]
    [SerializeField] private Button rewardItemButton;

    [Header("획득 가능한 아이템 프리팹")]
    [SerializeField] private GameObject[] itemPrefabs;

    private GameObject selectedItemPrefab;
    private bool rewardClaimed;

    private void Start()
    {
        List<GameObject> validItems = new List<GameObject>();

        if (itemPrefabs != null)
        {
            foreach (GameObject prefab in itemPrefabs)
            {
                if (prefab != null &&
                    prefab.GetComponent<RectTransform>() != null)
                {
                    validItems.Add(prefab);
                }
            }
        }

        if (validItems.Count > 0)
        {
            int index = Random.Range(0, validItems.Count);
            selectedItemPrefab = validItems[index];
        }

        if (rewardItemButton != null)
        {
            rewardItemButton.interactable =
                inventoryManager != null && selectedItemPrefab != null;
        }
    }

    public void ClaimItemReward()
    {
        if (rewardClaimed)
            return;

        if (inventoryManager == null || selectedItemPrefab == null)
        {
            Debug.LogWarning("인벤토리와 아이템 보상 풀을 연결해줘.", this);
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded("03MapScene"))
        {
            Debug.LogWarning(
                "NodemapScene을 Build Profiles의 Scene List에 등록해줘.",
                this
            );
            return;
        }

        if (!inventoryManager.TryAddItem(selectedItemPrefab))
            return;

        rewardClaimed = true;

        if (rewardItemButton != null)
            rewardItemButton.interactable = false;

        SceneManager.LoadScene("03MapScene");
    }
}