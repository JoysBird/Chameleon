using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject Crusader;
    public EnemyData enemyData;

    void Start()
    {
        GameObject enemy = Instantiate(Crusader, transform.position, transform.rotation);

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.Initialize(enemyData);
    }
}