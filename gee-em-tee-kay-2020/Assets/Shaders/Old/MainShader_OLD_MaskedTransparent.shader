    Shader "Laundry/Unlit/TransparentMasked" {
     
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Cutoff  ("Alpha cutoff", Range(0,1)) = 0.5
        _StencilMask("Stencil Mask", Range(0, 255)) = 1
    }
     
    Category {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Stencil{
            Ref [_StencilMask]
            Comp Equal
 
        }

     
        SubShader {Pass {
            GLSLPROGRAM
            varying mediump vec2 uv;
           
            #ifdef VERTEX
            uniform mediump vec4 _MainTex_ST;
            void main() {
                gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
                uv = gl_MultiTexCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
            }
            #endif
           
            #ifdef FRAGMENT
            uniform lowp sampler2D _MainTex;
            uniform lowp vec4 _Color;
            void main() {
                gl_FragColor = texture2D(_MainTex, uv) * _Color;
            }
            #endif     
            ENDGLSL
        }}
       
        SubShader {Pass {
            SetTexture[_MainTex] {Combine texture * constant ConstantColor[_Color]}
        }}
    }
     
    }
