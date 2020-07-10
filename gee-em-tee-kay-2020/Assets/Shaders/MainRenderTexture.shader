// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33212,y:32713,varname:node_3138,prsc:2|emission-7248-OUT;n:type:ShaderForge.SFN_Tex2d,id:1536,x:31938,y:32811,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_1536,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:e809ca07124d5dd4c90e48bcdb2aae49,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Append,id:3017,x:32663,y:32874,varname:node_3017,prsc:2|A-1536-R,B-9905-OUT;n:type:ShaderForge.SFN_Vector1,id:9905,x:32485,y:33002,varname:node_9905,prsc:2,v1:0;n:type:ShaderForge.SFN_Tex2dAsset,id:3378,x:32663,y:33026,ptovrint:False,ptlb:ColorRamp,ptin:_ColorRamp,varname:node_3378,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:85a57b172c2c1a5428b628d615f92ce1,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:2831,x:32847,y:32874,varname:node_2831,prsc:2,tex:85a57b172c2c1a5428b628d615f92ce1,ntxv:0,isnm:False|UVIN-3017-OUT,TEX-3378-TEX;n:type:ShaderForge.SFN_Lerp,id:7248,x:33028,y:32813,varname:node_7248,prsc:2|A-6695-OUT,B-2831-RGB,T-8481-OUT;n:type:ShaderForge.SFN_Slider,id:8481,x:32823,y:33070,ptovrint:False,ptlb:ColorRampAmount,ptin:_ColorRampAmount,varname:node_8481,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:6695,x:32485,y:32810,varname:node_6695,prsc:2|IN-1536-RGB,IMIN-2682-OUT,IMAX-1645-OUT,OMIN-3687-OUT,OMAX-2091-OUT;n:type:ShaderForge.SFN_Vector1,id:2682,x:32092,y:32862,varname:node_2682,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:1645,x:32092,y:32927,varname:node_1645,prsc:2,v1:1;n:type:ShaderForge.SFN_Slider,id:1482,x:31950,y:33095,ptovrint:False,ptlb:Contrast,ptin:_Contrast,varname:node_1482,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:-0.5,cur:0.1171171,max:1;n:type:ShaderForge.SFN_Add,id:2091,x:32281,y:33002,varname:node_2091,prsc:2|A-1645-OUT,B-1482-OUT;n:type:ShaderForge.SFN_Subtract,id:3687,x:32304,y:33169,varname:node_3687,prsc:2|A-2682-OUT,B-1482-OUT;proporder:1536-3378-8481-1482;pass:END;sub:END;*/

Shader "Laundry/PostProcessing" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _ColorRamp ("ColorRamp", 2D) = "black" {}
        _ColorRampAmount ("ColorRampAmount", Range(0, 1)) = 0
        _Contrast ("Contrast", Range(-0.5, 1)) = 0.1171171
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _ColorRamp; uniform float4 _ColorRamp_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _ColorRampAmount)
                UNITY_DEFINE_INSTANCED_PROP( float, _Contrast)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
////// Lighting:
////// Emissive:
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float node_2682 = 0.0;
                float node_1645 = 1.0;
                float _Contrast_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Contrast );
                float node_3687 = (node_2682-_Contrast_var);
                float2 node_3017 = float2(_MainTex_var.r,0.0);
                float4 node_2831 = tex2D(_ColorRamp,TRANSFORM_TEX(node_3017, _ColorRamp));
                float _ColorRampAmount_var = UNITY_ACCESS_INSTANCED_PROP( Props, _ColorRampAmount );
                float3 emissive = lerp((node_3687 + ( (_MainTex_var.rgb - node_2682) * ((node_1645+_Contrast_var) - node_3687) ) / (node_1645 - node_2682)),node_2831.rgb,_ColorRampAmount_var);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
