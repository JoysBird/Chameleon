using UnityEngine;
using UnityEngine.SceneManagement;

public class EventNodeButton : MonoBehaviour
{
    // 지도에서 EVENT 버튼을 누르면 이벤트 씬으로 이동한다.
    public void EnterEvent()
    {
        SceneManager.LoadScene("06EventScene");
    }
}