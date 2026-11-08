using UnityEngine;

public class SpawnPoint : MonoBehaviour //이것은 배틀 scene에서, 랜덤한 적을 스폰하기 위한 스크립트 입니다.
{
    public GameObject Crusader;

    void Start()
    {
        Instantiate(Crusader, transform.position, transform.rotation);
    }
}