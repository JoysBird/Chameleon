using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenuManager : MonoBehaviour
{
    // Save and Back 버튼을 누르면 메인 메뉴 씬으로 돌아간다.
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("01MainMenuScene");
    }
}