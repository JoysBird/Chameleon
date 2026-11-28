using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour
{
    // Project 창에 있는 노드 버튼 프리팹 5개를 연결한다.
    [Header("노드 버튼 프리팹")]
    [SerializeField] private Button startPrefab;
    [SerializeField] private Button battlePrefab;
    [SerializeField] private Button eventPrefab;
    [SerializeField] private Button hpPrefab;
    [SerializeField] private Button bossPrefab;

    // 지도에서 방과 방을 잇는 선의 모양을 설정한다.
    [Header("연결선")]
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField, Min(1f)] private float lineWidth = 3f;

    // MapRoot가 사용할 수 있는 화면 영역을 가져온다.
    private RectTransform mapRoot;

    // 지도 씬이 열리면 방을 생성하고, 선을 먼저 그린 뒤 버튼을 올린다.
    private void Start()
    {
        mapRoot = GetComponent<RectTransform>();

        if (mapRoot == null || startPrefab == null || battlePrefab == null ||
            eventPrefab == null || hpPrefab == null || bossPrefab == null)
        {
            Debug.LogWarning("MapRoot의 RectTransform과 노드 프리팹 5개를 확인해줘.", this);
            return;
        }

        Canvas.ForceUpdateCanvases();
        List<MapRoom> rooms = MapGenerator.Generate();

        // 선을 먼저 생성하면 버튼 뒤에 표시된다.
        foreach (MapRoom room in rooms)
        {
            foreach (MapRoom nextRoom in room.NextRooms)
            {
                DrawLine(GetRoomPosition(room), GetRoomPosition(nextRoom));
            }
        }

        // 생성된 방 종류에 맞는 프리팹을 해당 칸에 배치한다.
        foreach (MapRoom room in rooms)
        {
            Button button = Instantiate(GetPrefab(room.Type), mapRoot, false);
            RectTransform rect = (RectTransform)button.transform;

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = rect.anchorMin;
            rect.pivot = rect.anchorMin;
            rect.anchoredPosition = GetRoomPosition(room);

            button.name = $"{room.Type}_열{room.Column}_행{room.Row}";

            // 이번 단계는 배치 확인용이므로 기존 씬 이동 버튼의 입력을 잠근다.
            button.interactable = false;
        }
    }

    // 열은 왼쪽에서 오른쪽으로, 행은 위에서 아래로 배치한다.
    private Vector2 GetRoomPosition(MapRoom room)
    {
        int totalColumns = MapGenerator.MiddleColumnCount + 2;
        float cellWidth = mapRoot.rect.width / totalColumns;
        float cellHeight = mapRoot.rect.height / MapGenerator.RowCount;

        float x = -mapRoot.rect.width * 0.5f + cellWidth * (room.Column + 0.5f);
        float y = mapRoot.rect.height * 0.5f - cellHeight * (room.Row + 0.5f);

        return new Vector2(x, y);
    }

    // 두 방의 중심 사이에 얇은 UI Image를 배치해 통로를 그린다.
    private void DrawLine(Vector2 from, Vector2 to)
    {
        GameObject line = new GameObject("Line", typeof(RectTransform), typeof(Image));
        RectTransform rect = (RectTransform)line.transform;
        rect.SetParent(mapRoot, false);

        Vector2 direction = to - from;
        rect.anchoredPosition = (from + to) * 0.5f;
        rect.sizeDelta = new Vector2(direction.magnitude, lineWidth);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rect.localRotation = Quaternion.Euler(0f, 0f, angle);

        Image image = line.GetComponent<Image>();
        image.color = lineColor;
        image.raycastTarget = false;
    }

    // 방 종류에 해당하는 버튼 프리팹을 반환한다.
    private Button GetPrefab(MapRoomType type)
    {
        switch (type)
        {
            case MapRoomType.Start: return startPrefab;
            case MapRoomType.Battle: return battlePrefab;
            case MapRoomType.Event: return eventPrefab;
            case MapRoomType.HP: return hpPrefab;
            case MapRoomType.Boss: return bossPrefab;
            default: throw new System.ArgumentOutOfRangeException(nameof(type));
        }
    }
}