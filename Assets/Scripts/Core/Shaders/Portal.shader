// OpenGL & Unity => {
//                      (0, 1), (1, 1)
//                      (0, 0), (1, 0)
//                   }

// Unreal & DirectX => {
//                      (0, 0), (1, 0)
//                      (0, 1), (1, 1)
//                   }

Shader "Custom/Portal"
{
    Properties
    {
        _InactiveColour ("Inactive Colour", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100 // 통상 거리에 따른 3D 모델의 디테일을 달리 처리 -> 셰이더 퀄리티를 제한하는 용도로 사용
        Cull Off

        Pass // 각 SubShader는 여러 Pass 로 구성
        {
            CGPROGRAM // 셰이더에 사용되는 언어를 정의 ex) unity = CG / GLSL
            // 전처리문 (메인 함수로 사용할 함수명을 지정해주는 역할)
            // 정점 셰이더 함수 = vert , 프레그먼트 셰이더 함수 = frag 사용하겠다고 선언
            // 프레그먼트 셰이더는 픽셀 셰이더에 대응되는 셰이더
            #pragma vertex vert // 정점 함수의 이름을 정의, 각 꼭짓점에 대해 실행 (프로젝션, 색상, 텍스처 및 기타 데이터의 좌표)
            #pragma fragment frag // 각 픽셀에 대해 실행 (색상정보)
            #include "UnityCG.cginc" // 셰이더 코드 안에 헤더파일 포함


            // 정점 셰이더의 입력
            // 직접 할당하는 것이 아닌 구조체를 짜면 유니티가 알아서 값을 넣어줌
            struct appdata
            {
                // 정점의 로컬 좌표계 위치
                float4 vertex : POSITION; // POSITION : 정점위치
            };

            // vertex to fragment 의 약자
            // 정점 셰이더에서 appdata의 정보를 가공하여 프레그먼트 셰이더에서 사용할 정보를 v2f 구조체에 채움
            // 프레그먼트 셰이더에서 이 정보를 열람하여 최종 색상을 결정
            // (데이터가 온전히 넘어가는 것이 아닌 해당 픽셀 위치에 맞추어 자연스럽게 보간된 데이터를 내려줌)
            struct v2f // 새로운 유형의 변수를 정의, 구조체는 네이티브 데이터보다 더 많은 데이터가 필요할 때 사용
            {
                // 공간 포지션을 출력해 GPU에 화면의 어느 부분을 어느 뎁스로 래스터화할지
                float4 vertex : SV_POSITION; // clip space position output
                // uv 를 받아오도록 정의
                float4 screenPos : TEXCOORD0; // TEXCOORD0 : 1번째 UV coordinate, texture coordinate input
            };
            // 입력부
            // 텍스쳐는 UV 를 만나기 전까지 그냥 메모리에 올라와있는 텍스쳐일 뿐
            sampler2D _MainTex;
            float4 _InactiveColour;
            int displayMask; // set to 1 to display texture, otherwise will draw test colour
            

            v2f vert (appdata v)
            {
                v2f o;
                // 정점의 로컬 좌표를 받아와서 투영 좌표로 변환해주는 작업
                // 월드, 카메라, 투영 좌표로 변환하는 과정을 하나의 작업으로 줄인 것
                o.vertex = UnityObjectToClipPos(v.vertex); // UnityCG.cginc 에서 정의
                // 
                o.screenPos = ComputeScreenPos(o.vertex); // UnityCG.cginc 에서 정의
                return o;
            }
            
            // frag 함수는 fixed4 타입(낮은 정밀도 RGBA 컬러)을 반환
            // 함수는 하나의 값만을 반환 -> 시맨틱을 : SV_Target 으로 명시
            // SV_Target -> 반환되는 fixed4 가 screen view 의 target 이 되는 픽셀 위치에 뿌려져야 되는 색상 값이라는 것을 의미
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.screenPos.xy / i.screenPos.w;
                // 텍스처에서 색상 추출
                // 첫번째 매개변수 : 샘플러, 두번째 매개변수 : UV
                fixed4 portalCol = tex2D(_MainTex, uv);
                return portalCol * displayMask + _InactiveColour * (1-displayMask);
            }
            ENDCG
        }
    }
    Fallback "Standard" // for shadows
}
