// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:1,cusa:True,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:True,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:True,atwp:True,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:1873,x:33229,y:32719,varname:node_1873,prsc:2|emission-1749-OUT,alpha-603-OUT;n:type:ShaderForge.SFN_Tex2d,id:4805,x:32551,y:32729,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:True,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-4444-OUT;n:type:ShaderForge.SFN_Multiply,id:1086,x:32812,y:32818,cmnt:RGB,varname:node_1086,prsc:2|A-4805-RGB,B-5983-RGB,C-5376-RGB;n:type:ShaderForge.SFN_Color,id:5983,x:32551,y:32915,ptovrint:False,ptlb:Color,ptin:_Color,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_VertexColor,id:5376,x:32551,y:33079,varname:node_5376,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1749,x:33025,y:32818,cmnt:Premultiply Alpha,varname:node_1749,prsc:2|A-1086-OUT,B-603-OUT;n:type:ShaderForge.SFN_Multiply,id:603,x:32812,y:32992,cmnt:A,varname:node_603,prsc:2|A-4805-A,B-5983-A,C-5376-A;n:type:ShaderForge.SFN_TexCoord,id:2567,x:32112,y:32594,varname:node_2567,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:5796,x:31732,y:32853,ptovrint:False,ptlb:Displacement Texture,ptin:_DisplacementTexture,varname:node_5796,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-587-OUT;n:type:ShaderForge.SFN_ComponentMask,id:3864,x:31915,y:32735,varname:node_3864,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-4586-OUT;n:type:ShaderForge.SFN_Multiply,id:997,x:32083,y:32880,varname:node_997,prsc:2|A-3864-OUT,B-3902-OUT,C-2249-A;n:type:ShaderForge.SFN_Time,id:3362,x:30729,y:32842,varname:node_3362,prsc:2;n:type:ShaderForge.SFN_TexCoord,id:2307,x:31335,y:32944,varname:node_2307,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Add,id:587,x:31525,y:32853,varname:node_587,prsc:2|A-8164-OUT,B-2307-UVOUT;n:type:ShaderForge.SFN_ValueProperty,id:264,x:30547,y:32753,ptovrint:False,ptlb:Jitter Speed,ptin:_JitterSpeed,varname:node_264,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:2;n:type:ShaderForge.SFN_Round,id:2060,x:31128,y:32879,varname:node_2060,prsc:2|IN-7607-OUT;n:type:ShaderForge.SFN_Multiply,id:8164,x:31335,y:32737,varname:node_8164,prsc:2|A-2438-OUT,B-2060-OUT;n:type:ShaderForge.SFN_Multiply,id:7607,x:30930,y:32756,varname:node_7607,prsc:2|A-2438-OUT,B-3362-TTR;n:type:ShaderForge.SFN_Add,id:2438,x:30729,y:32632,varname:node_2438,prsc:2|A-7823-OUT,B-264-OUT;n:type:ShaderForge.SFN_Vector1,id:7823,x:30547,y:32534,varname:node_7823,prsc:2,v1:0.111;n:type:ShaderForge.SFN_Slider,id:3902,x:31697,y:33055,ptovrint:False,ptlb:Jitter Strength,ptin:_JitterStrength,varname:node_3902,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1,max:0.1;n:type:ShaderForge.SFN_Tex2d,id:2249,x:31794,y:33195,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_2249,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2ede061f33b6b714d94eb5c283685e60,ntxv:0,isnm:False|UVIN-2307-UVOUT;n:type:ShaderForge.SFN_Add,id:4444,x:32276,y:32728,varname:node_4444,prsc:2|A-2567-UVOUT,B-997-OUT;n:type:ShaderForge.SFN_Subtract,id:2021,x:32276,y:32880,varname:node_2021,prsc:2|A-2567-UVOUT,B-997-OUT;n:type:ShaderForge.SFN_Subtract,id:4586,x:31680,y:32658,varname:node_4586,prsc:2|A-5796-RGB,B-3211-OUT;n:type:ShaderForge.SFN_Vector1,id:3211,x:31790,y:32608,varname:node_3211,prsc:2,v1:0.5;proporder:4805-5983-5796-264-3902-2249;pass:END;sub:END;*/

Shader "Laundry/Wobble" {
    Properties {
        [PerRendererData]_MainTex ("MainTex", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _DisplacementTexture ("Displacement Texture", 2D) = "white" {}
        _JitterSpeed ("Jitter Speed", Float ) = 2
        _JitterStrength ("Jitter Strength", Range(0, 0.1)) = 0.1
        _Mask ("Mask", 2D) = "white" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _Stencil ("Stencil ID", Float) = 0
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilComp ("Stencil Comparison", Float) = 8
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilOpFail ("Stencil Fail Operation", Float) = 0
        _StencilOpZFail ("Stencil Z-Fail Operation", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
            "PreviewType"="Plane"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            Stencil {
                Ref [_Stencil]
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
                Comp [_StencilComp]
                Pass [_StencilOp]
                Fail [_StencilOpFail]
                ZFail [_StencilOpZFail]
            }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _DisplacementTexture; uniform float4 _DisplacementTexture_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP( float, _JitterSpeed)
                UNITY_DEFINE_INSTANCED_PROP( float, _JitterStrength)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float _JitterSpeed_var = UNITY_ACCESS_INSTANCED_PROP( Props, _JitterSpeed );
                float node_2438 = (0.111+_JitterSpeed_var);
                float4 node_3362 = _Time;
                float2 node_587 = ((node_2438*round((node_2438*node_3362.a)))+i.uv0);
                float4 _DisplacementTexture_var = tex2D(_DisplacementTexture,TRANSFORM_TEX(node_587, _DisplacementTexture));
                float _JitterStrength_var = UNITY_ACCESS_INSTANCED_PROP( Props, _JitterStrength );
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float2 node_997 = ((_DisplacementTexture_var.rgb-0.5).rg*_JitterStrength_var*_Mask_var.a);
                float2 node_4444 = (i.uv0+node_997);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_4444, _MainTex));
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                float node_603 = (_MainTex_var.a*_Color_var.a*i.vertexColor.a); // A
                float3 emissive = ((_MainTex_var.rgb*_Color_var.rgb*i.vertexColor.rgb)*node_603);
                float3 finalColor = emissive;
                return fixed4(finalColor,node_603);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                #ifdef PIXELSNAP_ON
                    o.pos = UnityPixelSnap(o.pos);
                #endif
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
