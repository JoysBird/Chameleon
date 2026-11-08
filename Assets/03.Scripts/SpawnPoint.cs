using UnityEngine;

[System.Serializable]
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
}