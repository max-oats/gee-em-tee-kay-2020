// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33054,y:32715,varname:node_3138,prsc:2|emission-6352-RGB;n:type:ShaderForge.SFN_Tex2d,id:6352,x:32761,y:32826,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_6352,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False|UVIN-2095-OUT;n:type:ShaderForge.SFN_Panner,id:510,x:32010,y:32915,varname:node_510,prsc:2,spu:0,spv:0.1|UVIN-5064-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:5064,x:32010,y:32729,varname:node_5064,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Lerp,id:2095,x:32506,y:32826,varname:node_2095,prsc:2|A-5064-UVOUT,B-9534-R,T-5622-OUT;n:type:ShaderForge.SFN_Tex2d,id:9534,x:32211,y:32915,ptovrint:False,ptlb:Displacement,ptin:_Displacement,varname:node_9534,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:28c7aad1372ff114b90d330f8a2dd938,ntxv:0,isnm:False|UVIN-510-UVOUT;n:type:ShaderForge.SFN_Slider,id:5622,x:32036,y:33133,ptovrint:False,ptlb:Waveyness,ptin:_Waveyness,varname:node_5622,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1,max:0.2;proporder:6352-9534-5622;pass:END;sub:END;*/

Shader "Laundry/Underwater" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _Displacement ("Displacement", 2D) = "white" {}
        _Waveyness ("Waveyness", Range(0, 0.2)) = 0.1
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
            uniform sampler2D _Displacement; uniform float4 _Displacement_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _Waveyness)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
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
                float4 node_179 = _Time;
                float2 node_510 = (i.uv0+node_179.g*float2(0,0.1));
                float4 _Displacement_var = tex2D(_Displacement,TRANSFORM_TEX(node_510, _Displacement));
                float _Waveyness_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Waveyness );
                float2 node_2095 = lerp(i.uv0,float2(_Displacement_var.r,_Displacement_var.r),_Waveyness_var);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_2095, _MainTex));
                float3 emissive = _MainTex_var.rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
