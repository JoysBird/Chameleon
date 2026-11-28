using System.Collections.Generic;
using UnityEngine;

// 지도에 등장하는 방의 종류를 구분한다.
public enum MapRoomType
{
    Start,
    Battle,
    Event,
    HP,
    Boss
}

// 방 하나의 위치, 종류, 이동 가능한 다음 방을 저장한다.
public class MapRoom
{
    public int Column { get; }
    public int Row { get; }
    public MapRoomType Type { get; }
    public List<MapRoom> NextRooms { get; } = new List<MapRoom>();

    public MapRoom(int column, int row, MapRoomType type)
    {
        Column = column;
        Row = row;
        Type = type;
    }
}

public static class MapGenerator
{
    // 가운데 구간은 위·가운데·아래 3행과 진행 방향 8열로 구성한다.
    public const int RowCount = 3;
    public const int MiddleColumnCount = 8;

    // START부터 BOSS까지 방과 통로가 모두 연결된 지도 한 장을 만든다.
    public static List<MapRoom> Generate()
    {
        List<MapRoom> rooms = new List<MapRoom>();
        MapRoom[,] middle = new MapRoom[MiddleColumnCount, RowCount];

        // 0열 가운데에 시작 지점을 만든다.
        MapRoom start = new MapRoom(0, 1, MapRoomType.Start);
        rooms.Add(start);

        // 화면의 1~8열에 해당하는 24칸에 랜덤 방을 만든다.
        for (int column = 0; column < MiddleColumnCount; column++)
        {
            for (int row = 0; row < RowCount; row++)
            {
                MapRoom room = new MapRoom(column + 1, row, PickRoomType());
                middle[column, row] = room;
                rooms.Add(room);
            }
        }

        // 마지막 9열 가운데에 보스 방을 만든다.
        MapRoom boss = new MapRoom(MiddleColumnCount + 1, 1, MapRoomType.Boss);
        rooms.Add(boss);

        // START에서 첫 열의 세 방으로, 마지막 열의 세 방에서 BOSS로 연결한다.
        for (int row = 0; row < RowCount; row++)
        {
            start.NextRooms.Add(middle[0, row]);
            middle[MiddleColumnCount - 1, row].NextRooms.Add(boss);
        }

        // 이웃한 두 열 사이에 직선 통로와 대각선 갈림길을 만든다.
        for (int column = 0; column < MiddleColumnCount - 1; column++)
        {
            // 세 행 모두 오른쪽으로 전진할 수 있게 한다.
            for (int row = 0; row < RowCount; row++)
            {
                middle[column, row].NextRooms.Add(middle[column + 1, row]);
            }

            // 위↔가운데 또는 가운데↔아래 중 한 구간에 갈림길을 만든다.
            int firstPair = Random.Range(0, RowCount - 1);
            AddDiagonal(middle, column, firstPair);

            // 절반의 확률로 나머지 구간에도 갈림길을 하나 더 만든다.
            if (Random.value < 0.5f)
            {
                int otherPair = 1 - firstPair;
                AddDiagonal(middle, column, otherPair);
            }
        }

        return rooms;
    }

    // 인접한 두 행 사이에서 대각선 방향 하나를 뽑는다.
    // 같은 구간에는 한 방향만 그리므로 두 대각선이 X자로 겹치지 않는다.
    private static void AddDiagonal(MapRoom[,] middle, int column, int upperRow)
    {
        int lowerRow = upperRow + 1;

        if (Random.value < 0.5f)
        {
            middle[column, upperRow].NextRooms.Add(middle[column + 1, lowerRow]);
        }
        else
        {
            middle[column, lowerRow].NextRooms.Add(middle[column + 1, upperRow]);
        }
    }

    // 초기 테스트용 등장 확률은 전투 50%, 이벤트 35%, HP 15%로 둔다.
    private static MapRoomType PickRoomType()
    {
        int roll = Random.Range(0, 100);

        if (roll < 50)
            return MapRoomType.Battle;

        if (roll < 85)
            return MapRoomType.Event;

        return MapRoomType.HP;
    }
}