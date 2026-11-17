using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleNodeButton : MonoBehaviour
{
    public void EnterBattle()
    {
        SceneManager.LoadScene("04BattleScene");
    }
}