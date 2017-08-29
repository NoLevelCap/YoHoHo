// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "WeatherShader"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_Texture0("Texture 0", 2D) = "white" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 texcoord_0;
		};

		uniform sampler2D _Texture0;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			o.texcoord_0.xy = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float4 tex2DNode74 = tex2D( _Texture0, ( ( ( ase_worldPos / 100.0 ) + ( _Time.y / 5.0 ) ) / 2.0 ).xy );
			o.Albedo = tex2DNode74.xyz;
			float lerpResult51 = lerp( 0.0 , 1.0 , ( i.texcoord_0.x / 0.05 ));
			float lerpResult60 = lerp( 0.0 , 1.0 , ( i.texcoord_0.y / 0.05 ));
			float lerpResult70 = lerp( 0.0 , 1.0 , ( ( 1.0 - i.texcoord_0.x ) / 0.05 ));
			float lerpResult71 = lerp( 0.0 , 1.0 , ( ( 1.0 - i.texcoord_0.y ) / 0.05 ));
			float FallOff57 = min( min( min( lerpResult51 , lerpResult60 ) , lerpResult70 ) , lerpResult71 );
			o.Alpha = ( pow( FallOff57 , -1.0 ) * tex2DNode74.r );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=12001
1214;122;706;936;740.6027;729.8954;1.6;True;False
Node;AmplifyShaderEditor.CommentaryNode;56;-642.1984,731.9006;Float;False;1200.8;724;;16;57;52;53;54;51;59;60;61;63;64;65;67;68;69;70;71;FallOff;0;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-571.6976,947.5012;Float;False;Constant;_Float3;Float 3;1;0;0.05;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.TextureCoordinatesNode;53;-584.8973,806.0001;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;54;-235.7982,815.5036;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;61;-247.1974,1028.999;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;63;-247.1978,930.1974;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;69;-51.79755,963.9993;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.OneMinusNode;64;-287.1971,1191.299;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;60;-51.1975,1083.3;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;51;-44.09858,827.2028;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;65;-35.9975,1237.401;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMinNode;59;235.3018,868.3999;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.LerpOp;70;169.2025,1033.999;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;48;-943.3989,-520.0994;Float;False;Constant;_Float1;Float 1;1;0;100;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;49;-611.0989,-440.199;Float;False;Constant;_Float2;Float 2;1;0;5;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleTimeNode;50;-621.8006,-517.3005;Float;False;1;0;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldPosInputsNode;79;-1009.696,-733.5975;Float;False;0;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.LerpOp;71;167.2025,1227.999;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;1.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMinNode;67;385.8026,972.298;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;80;-754.1966,-863.9984;Float;True;2;0;FLOAT3;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleDivideOpNode;87;-383.8035,-539.2016;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMinNode;68;420.6027,1149.999;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;47;-263.399,-386.5992;Float;False;Constant;_Float0;Float 0;1;0;2;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;86;-252.7003,-741.2012;Float;False;2;2;0;FLOAT3;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;358.5017,1351.302;Float;False;FallOff;-1;True;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.GetLocalVarNode;58;-378.5986,-234.9999;Float;True;57;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleDivideOpNode;88;-744.5016,-235.6998;Float;True;2;0;FLOAT3;0.0;False;1;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.TexturePropertyNode;46;-979.8974,2.099942;Float;True;Property;_Texture0;Texture 0;0;0;Assets/Res/Perlin/Noise.psd;False;white;Auto;0;1;SAMPLER2D
Node;AmplifyShaderEditor.PowerNode;93;-165.0027,-208.0957;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;-1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;74;-497.6956,-26.39985;Float;True;Property;_TextureSample0;Texture Sample 0;1;0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT4;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-152.7033,115.5047;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMinNode;55;-212.2986,426.6998;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;63,-6;Float;False;True;2;Float;ASEMaterialInspector;0;Standard;WeatherShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Transparent;0.5;True;False;0;False;Transparent;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;False;0;4;10;25;False;0.5;False;0;Zero;Zero;0;Zero;Zero;Add;Add;0;False;1.11;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;OBJECT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;54;0;53;1
WireConnection;54;1;52;0
WireConnection;61;0;53;2
WireConnection;61;1;52;0
WireConnection;63;0;53;1
WireConnection;69;0;63;0
WireConnection;69;1;52;0
WireConnection;64;0;53;2
WireConnection;60;2;61;0
WireConnection;51;2;54;0
WireConnection;65;0;64;0
WireConnection;65;1;52;0
WireConnection;59;0;51;0
WireConnection;59;1;60;0
WireConnection;70;2;69;0
WireConnection;71;2;65;0
WireConnection;67;0;59;0
WireConnection;67;1;70;0
WireConnection;80;0;79;0
WireConnection;80;1;48;0
WireConnection;87;0;50;0
WireConnection;87;1;49;0
WireConnection;68;0;67;0
WireConnection;68;1;71;0
WireConnection;86;0;80;0
WireConnection;86;1;87;0
WireConnection;57;0;68;0
WireConnection;88;0;86;0
WireConnection;88;1;47;0
WireConnection;93;0;58;0
WireConnection;74;0;46;0
WireConnection;74;1;88;0
WireConnection;92;0;93;0
WireConnection;92;1;74;1
WireConnection;0;0;74;0
WireConnection;0;9;92;0
ASEEND*/
//CHKSM=16C3A1AFC829BA04A1B9D78CC54A9D9E60908B9C