using UnityEngine;

// 이 컴포넌트가 초기화될 때 게임 창의 해상도와 전체 화면 모드를 설정한다.
public class ResolutionManager : MonoBehaviour
{
    void Awake()  // 이 컴포넌트가 처음 활성화될 때 게임 창의 표시 방식을 설정한다.
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);  // 1920×1080 해상도와 테두리 없는 전체 화면 모드를 요청한다.
    }
}

//이것은 해상도를 고정하기 위한 scene입니다.