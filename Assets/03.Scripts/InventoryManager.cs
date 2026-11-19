using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // 씬에 있는 아이템 슬롯 네 개를 표시 순서대로 연결한다.
    [Header("슬롯 01~04 순서로 연결")]
    public RectTransform[] slots = new RectTransform[4];

    private GameObject[] items = new GameObject[4];

    // static 배열은 씬이 바뀌어도 획득한 프리팹을 기억한다. 새 인벤토리는 이 배열로 슬롯을 복원한다.
    private static GameObject[] savedItemPrefabs = new GameObject[4];

    // 플레이가 새로 시작될 때 Unity가 아래 함수를 호출해 이전 플레이의 아이템 기록을 비운다.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeData()  // 새 플레이를 시작할 때 네 슬롯의 공유 아이템 기록을 비운다.
    {
        savedItemPrefabs = new GameObject[4];
    }

    private void Start()  // 씬의 슬롯 연결을 검사한 뒤 이전 전투에서 보관한 아이템을 다시 표시한다.
    {
        if (!ValidateSlots())
            return;

        for (int i = 0; i < savedItemPrefabs.Length; i++)
        {
            if (savedItemPrefabs[i] != null && items[i] == null)
            {
                CreateItem(savedItemPrefabs[i], i);
            }
        }
    }

    private bool ValidateSlots()  // 슬롯이 정확히 네 개이고 각 슬롯 오브젝트가 연결되어 있는지 검사한다.
    {
        if (slots == null || slots.Length != 4)
        {
            Debug.LogWarning("슬롯은 정확히 4개를 연결해줘.", this);
            return false;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogWarning($"{i + 1}번 슬롯을 연결해줘.", this);
                return false;
            }
        }

        return true;
    }

    public bool TryAddItem(GameObject itemPrefab)  // 첫 빈 슬롯에 아이템을 생성하고 실제로 추가했는지 반환한다.
    {
        if (itemPrefab == null)
            return false;

        if (itemPrefab.GetComponent<RectTransform>() == null)
        {
            Debug.LogWarning("UI 버튼 프리팹을 넣어줘.", this);
            return false;
        }

        if (!ValidateSlots())
            return false;

        for (int i = 0; i < savedItemPrefabs.Length; i++)
        {
            if (savedItemPrefabs[i] != null)
                continue;

            CreateItem(itemPrefab, i);
            savedItemPrefabs[i] = itemPrefab;  // 씬 이동 후에도 이 슬롯에 같은 아이템을 다시 만들도록 원본을 기록한다.
            return true;
        }

        Debug.Log("인벤토리가 가득 찼어.", this);
        return false;
    }

    private void CreateItem(GameObject itemPrefab, int slotIndex)  // 지정한 슬롯 아래에 아이템 UI를 복제하고 슬롯 크기에 맞춘다.
    {
        GameObject item = Instantiate(itemPrefab, slots[slotIndex], false);
        items[slotIndex] = item;  // 현재 씬에서 실제로 눌러 사용할 아이템 오브젝트를 기억한다.

        RectTransform rect = item.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        rect.localRotation = Quaternion.identity;

        Vector3 position = rect.localPosition;
        position.z = 0f;
        rect.localPosition = position;

        item.SetActive(true);
    }

    public void AddItem(GameObject itemPrefab)  // 성공 여부를 돌려받지 않아도 되는 호출에서 같은 추가 기능을 사용한다.
    {
        TryAddItem(itemPrefab);
    }

    public void RemoveItem(GameObject itemObject)  // 전달받은 아이템과 일치하는 슬롯을 찾아 비우고 화면에서도 제거한다.
    {
        if (itemObject == null)
            return;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != itemObject)
                continue;

            savedItemPrefabs[i] = null;  // 다음 씬에서 이 아이템이 다시 생성되지 않게 저장 기록을 지운다.
            items[i] = null;

            itemObject.SetActive(false);
            Destroy(itemObject);
            return;
        }
    }

    public void ClearInventory()  // 네 슬롯의 저장 기록과 현재 표시 중인 아이템을 모두 정리한다.
    {
        for (int i = 0; i < items.Length; i++)
        {
            savedItemPrefabs[i] = null;  // 다음 씬에서 이 아이템이 다시 생성되지 않게 저장 기록을 지운다.

            if (items[i] != null)
            {
                items[i].SetActive(false);
                Destroy(items[i]);
                items[i] = null;
            }
        }
    }
}