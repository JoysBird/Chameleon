using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game/Enemy")] //이것은 적에 관한 스크립트테이블을만들기 위함입니다. 

public class EnemyData : ScriptableObject //이것은 랜덤으로 등장하는 적의 이름, 모습, HP를 설정하기 위한 데이터 스크립트입니다.
{
    [Header("적 정보")]
    public string enemyName;
    public GameObject enemyPrefab;

    [Header("능력치")]
    public int maxHP;
}