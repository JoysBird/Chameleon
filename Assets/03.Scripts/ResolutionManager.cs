using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }
}

//이것은 해상도를 고정하기 위한 scene입니다.