using UnityEngine;

public class ResolutionManager : MonoBehaviour //이것은 해상도를 고정하기 위한 scene입니다.
{
    void Awake(DontDestroyOnLoad(gameObject);)
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);//1920 해상도 고정, 풀스크린윈도우
    }
}