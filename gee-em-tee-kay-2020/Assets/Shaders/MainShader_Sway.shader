// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:1,stmr:255,stmw:255,stcp:5,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9361,x:33347,y:32751,varname:node_9361,prsc:2|normal-6127-RGB,custl-1941-OUT,clip-5645-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:8068,x:32727,y:33314,varname:node_8068,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1941,x:32972,y:33032,cmnt:Diffuse Contribution,varname:node_1941,prsc:2|A-2821-OUT,B-9655-RGB,C-8174-OUT,D-8068-OUT;n:type:ShaderForge.SFN_Tex2d,id:4094,x:32639,y:32800,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_4094,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:5b9330a4c4406bf47b066b6f1b477197,ntxv:0,isnm:False|UVIN-7425-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:7425,x:32465,y:32800,varname:node_7425,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_LightColor,id:9655,x:32522,y:32990,varname:node_9655,prsc:2;n:type:ShaderForge.SFN_Color,id:393,x:32639,y:32637,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_393,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.6603774,c2:0.6603774,c3:0.6603774,c4:1;n:type:ShaderForge.SFN_Multiply,id:2821,x:32860,y:32727,varname:node_2821,prsc:2|A-393-RGB,B-4094-RGB;n:type:ShaderForge.SFN_Tex2d,id:1125,x:32727,y:33463,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_1125,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5645,x:32972,y:33174,varname:node_5645,prsc:2|A-4094-A,B-1125-A,C-393-A;n:type:ShaderForge.SFN_Tex2d,id:6127,x:33091,y:32727,ptovrint:False,ptlb:Normal,ptin:_Normal,varname:node_6127,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:True;n:type:ShaderForge.SFN_AmbientLight,id:7778,x:31940,y:33156,varname:node_7778,prsc:2;n:type:ShaderForge.SFN_OneMinus,id:1493,x:32135,y:33368,varname:node_1493,prsc:2|IN-6433-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:6433,x:31917,y:33312,varname:node_6433,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7877,x:32309,y:33368,varname:node_7877,prsc:2|A-1493-OUT,B-8769-OUT;n:type:ShaderForge.SFN_Vector1,id:8769,x:32173,y:33502,varname:node_8769,prsc:2,v1:10;n:type:ShaderForge.SFN_OneMinus,id:5259,x:32478,y:33368,varname:node_5259,prsc:2|IN-7877-OUT;n:type:ShaderForge.SFN_Lerp,id:8174,x:32512,y:33211,varname:node_8174,prsc:2|A-7778-RGB,B-393-RGB,T-6433-OUT;proporder:4094-393-1125-6127;pass:END;sub:END;*/

Shader "Laundry/MainShader_Sway" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        [PerRendererData]_Color ("Color", Color) = (0.6603774,0.6603774,0.6603774,1)
        _Mask ("Mask", 2D) = "white" {}
        _Normal ("Normal", 2D) = "bump" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5

        _wind_dir ("Wind Direction", Vector) = (0.5,0.05,0.5,0)
        _wind_size ("Wind Wave Size", range(5,50)) = 15

        _tree_sway_stutter_influence("Tree Sway Stutter Influence", range(0,1)) = 0.2
        _tree_sway_stutter ("Tree Sway Stutter", range(0,10)) = 1.5
        _tree_sway_speed ("Tree Sway Speed", range(0,10)) = 1
        _tree_sway_disp ("Tree Sway Displacement", range(0,1)) = 0.3

        _branches_disp ("Branches Displacement", range(0,0.5)) = 0.3

        _leaves_wiggle_disp ("Leaves Wiggle Displacement", float) = 0.07
        _leaves_wiggle_speed ("Leaves Wiggle Speed", float) = 0.01

        _r_influence ("Red Vertex Influence", range(0,1)) = 1
        _b_influence ("Blue Vertex Influence", range(0,1)) = 1
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
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
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;

            //Declared Variables
            float4 _wind_dir;
            float _wind_size;
            float _tree_sway_speed;
            float _tree_sway_disp;
            float _leaves_wiggle_disp;
            float _leaves_wiggle_speed;
            float _branches_disp;
            float _tree_sway_stutter;
            float _tree_sway_stutter_influence;
            float _r_influence;
            float _b_influence;

            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                fixed4 color : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                fixed4 color : COLOR;
                LIGHTING_COORDS(4,5)
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                //Gets the vertex's World Position 
                float3 worldPos = mul (unity_ObjectToWorld, o.pos).xyz;

                //Tree Movement and Wiggle
                o.pos.x += (cos(_Time.z * _tree_sway_speed + (worldPos.x/_wind_size) + (sin(_Time.z * _tree_sway_stutter * _tree_sway_speed + (worldPos.x/_wind_size)) * _tree_sway_stutter_influence) ) + 1)/2 * _tree_sway_disp * _wind_dir.x * (o.pos.y / 10) + 
                cos(_Time.w * o.pos.x * _leaves_wiggle_speed + (worldPos.x/_wind_size)) * _leaves_wiggle_disp * _wind_dir.x * lightColor.b * _b_influence;

                o.pos.z += (cos(_Time.z * _tree_sway_speed + (worldPos.z/_wind_size) + (sin(_Time.z * _tree_sway_stutter * _tree_sway_speed + (worldPos.z/_wind_size)) * _tree_sway_stutter_influence) ) + 1)/2 * _tree_sway_disp * _wind_dir.z * (o.pos.y / 10) + 
                cos(_Time.w * o.pos.z * _leaves_wiggle_speed + (worldPos.x/_wind_size)) * _leaves_wiggle_disp * _wind_dir.z * lightColor.b * _b_influence;

                o.pos.y += cos(_Time.z * _tree_sway_speed + (worldPos.z/_wind_size)) * _tree_sway_disp * _wind_dir.y * (o.pos.y / 10);

                //Branches Movement
                o.pos.y += sin(_Time.w * _tree_sway_speed + _wind_dir.x + (worldPos.z/_wind_size)) * _branches_disp  * lightColor.r * _r_influence;
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                clip((_MainTex_var.a*_Mask_var.a*_Color_var.a) - 0.5);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 finalColor = ((_Color_var.rgb*_MainTex_var.rgb)*_LightColor0.rgb*lerp(UNITY_LIGHTMODEL_AMBIENT.rgb,_Color_var.rgb,attenuation)*attenuation);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            uniform sampler2D _Normal; uniform float4 _Normal_ST;

            
            //Declared Variables
            float4 _wind_dir;
            float _wind_size;
            float _tree_sway_speed;
            float _tree_sway_disp;
            float _leaves_wiggle_disp;
            float _leaves_wiggle_speed;
            float _branches_disp;
            float _tree_sway_stutter;
            float _tree_sway_stutter_influence;
            float _r_influence;
            float _b_influence;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                fixed4 color : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                float3 tangentDir : TEXCOORD2;
                float3 bitangentDir : TEXCOORD3;
                fixed4 color : COLOR;
                LIGHTING_COORDS(4,5)
                UNITY_FOG_COORDS(6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos( v.vertex );
                //Gets the vertex's World Position 
                float3 worldPos = mul (unity_ObjectToWorld, o.pos).xyz;

                //Tree Movement and Wiggle
                o.pos.x += (cos(_Time.z * _tree_sway_speed + (worldPos.x/_wind_size) + (sin(_Time.z * _tree_sway_stutter * _tree_sway_speed + (worldPos.x/_wind_size)) * _tree_sway_stutter_influence) ) + 1)/2 * _tree_sway_disp * _wind_dir.x * (o.pos.y / 10) + 
                cos(_Time.w * o.pos.x * _leaves_wiggle_speed + (worldPos.x/_wind_size)) * _leaves_wiggle_disp * _wind_dir.x * v.color.b * _b_influence;

                o.pos.z += (cos(_Time.z * _tree_sway_speed + (worldPos.z/_wind_size) + (sin(_Time.z * _tree_sway_stutter * _tree_sway_speed + (worldPos.z/_wind_size)) * _tree_sway_stutter_influence) ) + 1)/2 * _tree_sway_disp * _wind_dir.z * (o.pos.y / 10) + 
                cos(_Time.w * o.pos.z * _leaves_wiggle_speed + (worldPos.x/_wind_size)) * _leaves_wiggle_disp * _wind_dir.z * v.color.b * _b_influence;

                o.pos.y += cos(_Time.z * _tree_sway_speed + (worldPos.z/_wind_size)) * _tree_sway_disp * _wind_dir.y * (o.pos.y / 10);

                //Branches Movement
                o.pos.y += sin(_Time.w * _tree_sway_speed + _wind_dir.x + (worldPos.z/_wind_size)) * _branches_disp * v.color.r * _r_influence;
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 _Normal_var = UnpackNormal(tex2D(_Normal,TRANSFORM_TEX(i.uv0, _Normal)));
                float3 normalLocal = _Normal_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                clip((_MainTex_var.a*_Mask_var.a*_Color_var.a) - 0.5);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 finalColor = ((_Color_var.rgb*_MainTex_var.rgb)*_LightColor0.rgb*lerp(UNITY_LIGHTMODEL_AMBIENT.rgb,_Color_var.rgb,attenuation)*attenuation);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
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
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma target 3.0
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                clip((_MainTex_var.a*_Mask_var.a*_Color_var.a) - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
