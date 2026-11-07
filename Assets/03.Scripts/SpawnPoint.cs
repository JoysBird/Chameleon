using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject Crusader;

    void Start()
    {
        Instantiate(Crusader, transform.position, transform.rotation);
    }
}