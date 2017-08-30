Shader "Hidden/Vintage"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Perlin ("Perlin", 2D) = "white" {}
		_Degrade ("Degrade", 2D) = "white" {}
		_Under ("Undertexture", Color) = (0,0,0,0)
		_Darker ("Darkertexture", Color) = (0,0,0,0)

		_Size ("Size", Range(0,2)) = 1 
		_Vig ("Vig", Range(0,1)) = 1 
		_VigIntensity ("Vignette Intensity", Float) = 15
		_DegradeSize ("Degrade", Float) = 1 

		_NoRed ("No Red", Range(0,1)) = 1 

	}
	SubShader
	{
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always
		Tags {"Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGBA

		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			float4  _MainTex_ST;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			sampler2D _MainTex, _Perlin, _Degrade;

			float _Size, _DegradeSize, _Vig, _NoRed, _VigIntensity;
			fixed4 _Under, _Darker;

			fixed4 frag (v2f i) : SV_Target
			{
				//float2 newUV = TRANSFORM_TEX(i.uv, _Perlin);
				fixed4 per = tex2D(_Perlin, i.uv);
				fixed4 deg = tex2D(_Degrade, i.uv * _DegradeSize);
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 tCol = tex2D(_MainTex, i.uv);

				float2 sp = (i.screenPos.xy/i.screenPos.w);
   
				sp *=  1.0 - sp.yx;   //vec2(1.0)- uv.yx; -> 1.-u.yx; Thanks FabriceNeyret !
    
				float vig = sp.x*sp.y * _VigIntensity; // multiply with sth for intensity
    
				vig = pow(vig, _Vig); // change pow for modifying the extend of the  vignette

				vig = vig;

				fixed3 BColor = fixed3(1,1,1);

				if(tCol.r > 0.01 && tCol.g < 0.2 && tCol.b < 0.2 && _NoRed > 0)
					col.rgb =  _Under.rgb * (col.r);

				fixed4 OUT = col;

				OUT.a = (vig * (1-per.b) * _Size) / _DegradeSize;

				//fixed4 BColor = fixed4(0,0,0,1);

				//OUT.a = lerp(OUT.a, BColor, vig/2);

				OUT.rgb = OUT.rgb * (OUT.a) + _Darker.rgb * (1-OUT.a);

				OUT.a = 1;

				if(tCol.r > 0.01 && tCol.g < 0.2 && tCol.b < 0.2 && _NoRed > 0)
					tCol.rgb = 0;

				OUT.rgb += tCol.rgb * 0.4;

				return OUT;
			}
			ENDCG
		}
	}
}

