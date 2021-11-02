// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/YeastShader"
{
    Properties 
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("Color (RGBA)", Color) = (1, 1, 1, 1) // add _Color property
    }

    SubShader 
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100

        Pass 
        {
            CGPROGRAM

            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct appdata_t 
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f 
            {
                float4 vertex  : SV_POSITION;
                half2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            uniform sampler2D _MainTex;
            half4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); 
                
                o.vertex     = UnityObjectToClipPos(v.vertex);
                v.texcoord.x = 1 - v.texcoord.x;
                o.texcoord   = TRANSFORM_TEX(v.texcoord, _MainTex);
                

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); //Insert
    
                //fixed4 col = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_MainTex, i.texcoord) * _Color; //Insert
                fixed4 col = tex2D(_MainTex, i.texcoord) * _Color; // multiply by _Color
                //fixed4 myTex = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST)) * _Color;
                return col;
            }

            ENDCG
        }
    }
}