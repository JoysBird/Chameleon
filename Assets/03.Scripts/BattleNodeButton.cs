using UnityEngine;
using UnityEngine.SceneManagement;

// 맵의 전투 노드 버튼이 On Click()에서 EnterBattle을 호출하면 전투 씬을 연다.
public class BattleNodeButton : MonoBehaviour
{
    public void EnterBattle()  // 노드맵에서 BattleNode 버튼을 눌렀을 때 호출하는 함수다.
    {
        SceneManager.LoadScene("04BattleScene");  // 노드맵에서 전투 씬으로 이동해 다음 전투를 시작한다.
    }
}