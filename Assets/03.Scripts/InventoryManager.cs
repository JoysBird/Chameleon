using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("½½·Ô 01~04 ¼ø¼­·Î ¿¬°á")]
    public RectTransform[] slots = new RectTransform[4];

    private GameObject[] items = new GameObject[4];

    private static GameObject[] savedItemPrefabs = new GameObject[4];

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetRuntimeData()
    {
        savedItemPrefabs = new GameObject[4];
    }

    private void Start()
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

    private bool ValidateSlots()
    {
        if (slots == null || slots.Length != 4)
        {
            Debug.LogWarning("½½·ÔÀº Á¤È®È÷ 4°³¸¦ ¿¬°áÇØÁà.", this);
            return false;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogWarning($"{i + 1}¹ø ½½·ÔÀ» ¿¬°áÇØÁà.", this);
                return false;
            }
        }

        return true;
    }

    public bool TryAddItem(GameObject itemPrefab)
    {
        if (itemPrefab == null)
            return false;

        if (itemPrefab.GetComponent<RectTransform>() == null)
        {
            Debug.LogWarning("UI ¹öÆ° ÇÁ¸®ÆÕÀ» ³Ö¾îÁà.", this);
            return false;
        }

        if (!ValidateSlots())
            return false;

        for (int i = 0; i < savedItemPrefabs.Length; i++)
        {
            if (savedItemPrefabs[i] != null)
                continue;

            CreateItem(itemPrefab, i);
            savedItemPrefabs[i] = itemPrefab;
            return true;
        }

        Debug.Log("ÀÎº¥Åä¸®°¡ °¡µæ Ã¡¾î.", this);
        return false;
    }

    private void CreateItem(GameObject itemPrefab, int slotIndex)
    {
        GameObject item = Instantiate(itemPrefab, slots[slotIndex], false);
        items[slotIndex] = item;

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

    public void AddItem(GameObject itemPrefab)
    {
        TryAddItem(itemPrefab);
    }

    public void RemoveItem(GameObject itemObject)
    {
        if (itemObject == null)
            return;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != itemObject)
                continue;

            savedItemPrefabs[i] = null;
            items[i] = null;

            itemObject.SetActive(false);
            Destroy(itemObject);
            return;
        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < items.Length; i++)
        {
            savedItemPrefabs[i] = null;

            if (items[i] != null)
            {
                items[i].SetActive(false);
                Destroy(items[i]);
                items[i] = null;
            }
        }
    }
}