using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("슬롯 01~04 순서로 연결")]
    public RectTransform[] slots = new RectTransform[4];

    private GameObject[] items = new GameObject[4];

    public bool TryAddItem(GameObject itemPrefab)
    {
        if (itemPrefab == null)
            return false;

        if (itemPrefab.GetComponent<RectTransform>() == null)
        {
            Debug.LogWarning("UI 버튼 프리팹을 넣어줘.", this);
            return false;
        }

        if (slots == null || slots.Length != 4)
        {
            Debug.LogWarning("슬롯은 정확히 4개를 연결해줘.", this);
            return false;
        }

        // 생성 전에 네 슬롯의 연결 상태 확인
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogWarning($"{i + 1}번 슬롯이 비어 있어.", this);
                return false;
            }
        }

        // 01부터 순서대로 빈칸 찾기
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                continue;

            GameObject item = Instantiate(itemPrefab, slots[i], false);
            items[i] = item;

            // 아이템 버튼을 슬롯 크기에 맞추기
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
            return true;
        }

        Debug.Log("인벤토리가 가득 찼어.", this);
        return false;
    }

    // 보상 버튼의 On Click에서도 연결할 수 있는 함수
    public void AddItem(GameObject itemPrefab)
    {
        TryAddItem(itemPrefab);
    }

    // 아이템 효과 적용에 성공한 후 호출
    public void RemoveItem(GameObject itemObject)
    {
        if (itemObject == null)
            return;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != itemObject)
                continue;

            items[i] = null;
            itemObject.SetActive(false);
            Destroy(itemObject);
            return;
        }
    }
}