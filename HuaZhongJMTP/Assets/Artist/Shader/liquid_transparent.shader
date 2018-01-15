Shader "Custom/liquid_transparent" {  
    Properties {  
        _Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}  
        _TransVal ("Transparency Value", Range(0,1)) = 0.5  
    }  
    SubShader {  
        Tags { "RenderType"="Opaque" "Queue"="Transparent+50"}  
        LOD 200  
          
        CGPROGRAM  
        #pragma surface surf Lambert alpha  
  
        sampler2D _MainTex;  
        float _TransVal;  
		fixed4 _Color;
        struct Input {  
            float2 uv_MainTex;  
        };  
  
        void surf (Input IN, inout SurfaceOutput o) {  
            half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;  
            o.Albedo = c.rgb;  
            o.Alpha = c.a * _TransVal;  
        }  
        ENDCG  
    }   
    FallBack "Diffuse"  
} 