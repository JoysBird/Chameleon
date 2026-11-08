using UnityEngine;

// 적 프리팹 정보를 저장하기 위한 클래스
[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;// 생성할 적 프리팹
}

public class SpawnPoint : MonoBehaviour
{
    public EnemySpawnData[] enemyList;// 생성할 적 프리팹 목록

    void Start()
    {
        SpawnRandomEnemy();// 게임 시작 시 랜덤 적 생성 함수
    }

    void SpawnRandomEnemy()
    {
        if (enemyList.Length == 0)
            return; // 적 목록에 등록된 프리팹이 없다면 함수 종료

        int randomIndex = Random.Range(0, enemyList.Length);// 적 목록 중 랜덤한 인덱스 선택

        EnemySpawnData selectedEnemy = enemyList[randomIndex];// 랜덤으로 선택된 적 정보 가져오기

        GameObject enemy = Instantiate( // 선택한 적 프리팹 생성
            selectedEnemy.enemyPrefab, // 생성할 적 프리팹 지정
            transform.position, // 현재 SpawnPoint의 위치에 생성
            transform.rotation // 현재 SpawnPoint의 회전값 적용
        );
    }
}


//이하는enemydata를 통해 스크립트 테이블 쓰는방식. 
/*
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public EnemyData enemyData;
}

public class SpawnPoint : MonoBehaviour
{
    public EnemySpawnData[] enemyList;

    void Start()
    {
        SpawnRandomEnemy();
    }

    void SpawnRandomEnemy()
    {
        if (enemyList.Length == 0)
            return;

        int randomIndex = Random.Range(0, enemyList.Length);

        EnemySpawnData selectedEnemy = enemyList[randomIndex];

        GameObject enemy = Instantiate(
            selectedEnemy.enemyPrefab,
            transform.position,
            transform.rotation
        );

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.Initialize(selectedEnemy.enemyData);
    }
*/