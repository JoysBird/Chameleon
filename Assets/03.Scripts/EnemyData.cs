using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game/Enemy")] //이것은 적에 관한 스크립트테이블을만들기 위함입니다. 

public class EnemyData : ScriptableObject //이것은 랜덤으로 등장하는 적의 이름, 모습, HP를 설정하기 위한 데이터 스크립트입니다.
{
    // 이 적 데이터 에셋에 이름과 프리팹을 기록한다.
    [Header("적 정보")]
    public string enemyName;  // 이 적 데이터 에셋이 어떤 적을 나타내는지 기록하는 이름이다.
    public GameObject enemyPrefab;

    // 에셋에 저장할 최대 HP다. 현재 SpawnPoint는 프리팹의 EnemyHealth 값을 사용한다.
    [Header("능력치")]
    public int maxHP;
}