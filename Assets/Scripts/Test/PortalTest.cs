using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTest : MonoBehaviour
{
    public MeshRenderer screen;
    Camera mainCam;
    RenderTexture viewTexture;

    private void Awake() {
        mainCam = Camera.main;
        screen.material.SetInt("displayMask", 1);
    }
    private void CreateViewTexture() {
        if(viewTexture == null) {
            viewTexture = new RenderTexture(Screen.width, Screen.height, 100);
        }
        // 카메라가 해당 텍스쳐로 렌더링된다.
        mainCam.targetTexture = viewTexture;
        // _MainTex => 텍스쳐를 입력받는 변수(이름이 달라져도 상관없지만 첫번째 텍스쳐를 _MainTex 라고 함)
        screen.material.SetTexture("_MainTex", viewTexture);
    }
    public void Render() {
        CreateViewTexture();
    }
}
