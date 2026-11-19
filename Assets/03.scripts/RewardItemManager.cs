using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardItemManager : MonoBehaviour
{
    // 보상 아이템을 빈 슬롯에 추가할 인벤토리를 연결한다. SerializeField로 private 참조를 씬에 저장한다.
    [Header("인벤토리")]
    [SerializeField] private InventoryManager inventoryManager;

    // 보상 수령 가능 여부에 따라 활성화할 버튼을 연결한다.
    [Header("아이템 보상 버튼")]
    [SerializeField] private Button rewardItemButton;

    // 전투 보상으로 추첨할 아이템 UI 프리팹 목록이다.
    [Header("획득 가능한 아이템 프리팹")]
    [SerializeField] private GameObject[] itemPrefabs;

    private GameObject selectedItemPrefab;
    private bool rewardClaimed;

    private void Start()  // 보상 패널이 처음 활성화되면 이번 전투에서 지급할 아이템을 뽑는다.
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
            selectedItemPrefab = validItems[index];  // 버튼을 누를 때까지 이번에 뽑힌 보상 프리팹을 기억한다.
        }

        if (rewardItemButton != null)
        {
            rewardItemButton.interactable =
                inventoryManager != null && selectedItemPrefab != null;
        }
    }

    public void ClaimItemReward()  // 보상 버튼 클릭 시 선택된 아이템을 인벤토리에 지급하고 맵으로 돌아간다.
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

        if (!inventoryManager.TryAddItem(selectedItemPrefab))  // 슬롯이 가득 차는 등 지급에 실패하면 맵으로 이동하기 전에 멈춘다.
            return;

        rewardClaimed = true;  // 이미 지급했다고 기록해 같은 전투에서 중복 수령을 막는다.

        if (rewardItemButton != null)
            rewardItemButton.interactable = false;

        SceneManager.LoadScene("03MapScene");  // 보상을 받은 뒤 노드맵 씬을 불러온다.
    }
}