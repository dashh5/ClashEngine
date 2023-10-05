// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon/Water"
{
	Properties
	{
		_Float0("Float 0", Float) = 0
		_Foam("Foam", 2D) = "white" {}
		[HDR]_FoamColor("FoamColor", Color) = (0.2091492,0.9433962,0.7685754,0)
		[HDR]_WaterColor("WaterColor", Color) = (0.4308473,0.4669504,0.8867924,0)
		_Noise("Noise", 2D) = "white" {}
		_Float3("Float 3", Float) = 0
		_Foam2Tiling("Foam2Tiling", Float) = 0
		_Foam1Tiling("Foam1Tiling", Float) = 0
		_NoiseTiling("NoiseTiling", Float) = 0
		_Float1("Float 1", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float4 screenPosition6;
		};

		uniform float4 _FoamColor;
		uniform float4 _WaterColor;
		uniform sampler2D _Foam;
		SamplerState sampler_Foam;
		uniform float _Foam1Tiling;
		uniform float _Foam2Tiling;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _Float0;
		uniform sampler2D _Noise;
		SamplerState sampler_Noise;
		uniform float _NoiseTiling;
		uniform float _Float3;
		uniform float _Float1;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 vertexPos6 = ase_vertex3Pos;
			float4 ase_screenPos6 = ComputeScreenPos( UnityObjectToClipPos( vertexPos6 ) );
			o.screenPosition6 = ase_screenPos6;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float2 appendResult56 = (float2(ase_worldPos.x , ase_worldPos.z));
			float2 panner21 = ( 1.0 * _Time.y * float2( 0.01,0.02 ) + ( _Foam1Tiling * appendResult56 ));
			float2 panner31 = ( 1.0 * _Time.y * float2( -0.02,0.01 ) + ( appendResult56 * _Foam2Tiling ));
			float4 ase_screenPos6 = i.screenPosition6;
			float4 ase_screenPosNorm6 = ase_screenPos6 / ase_screenPos6.w;
			ase_screenPosNorm6.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm6.z : ase_screenPosNorm6.z * 0.5 + 0.5;
			float screenDepth6 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm6.xy ));
			float distanceDepth6 = abs( ( screenDepth6 - LinearEyeDepth( ase_screenPosNorm6.z ) ) / ( _Float0 ) );
			float2 panner51 = ( 1.0 * _Time.y * float2( 0.05,-0.05 ) + ( appendResult56 * _NoiseTiling ));
			float clampResult17 = clamp( ( ( tex2D( _Foam, panner21 ).r * tex2D( _Foam, panner31 ).r * distanceDepth6 ) + ( distanceDepth6 * pow( tex2D( _Noise, panner51 ).r , _Float3 ) ) ) , 0.0 , 1.0 );
			float4 lerpResult13 = lerp( _FoamColor , _WaterColor , clampResult17);
			o.Emission = lerpResult13.rgb;
			o.Alpha = _Float1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
7;353;1502;658;753.2119;243.9482;1.19115;True;False
Node;AmplifyShaderEditor.WorldPosInputsNode;55;-2191.205,199.028;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;59;-1598.902,849.125;Inherit;False;Property;_NoiseTiling;NoiseTiling;8;0;Create;True;0;0;False;0;False;0;0.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;-1959.191,207.974;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-1531.803,293.8448;Inherit;False;Property;_Foam2Tiling;Foam2Tiling;6;0;Create;True;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-1581.189,-133.9529;Inherit;False;Property;_Foam1Tiling;Foam1Tiling;7;0;Create;True;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1344.448,774.4846;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-1360.278,-129.6161;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;51;-1089.271,774.6586;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,-0.05;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1319.025,203.181;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-974.2111,579.0156;Inherit;False;Property;_Float0;Float 0;0;0;Create;True;0;0;False;0;False;0;1.42;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-756.1178,963.2471;Inherit;False;Property;_Float3;Float 3;5;0;Create;True;0;0;False;0;False;0;-3.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;8;-1018.211,427.0157;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;31;-1037.055,204.4972;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.02,0.01;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;47;-817.6273,743.5661;Inherit;True;Property;_Noise;Noise;4;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;21;-1032.641,-130.1652;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.01,0.02;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PowerNode;49;-441.7379,773.1742;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;30;-741.7805,176.5718;Inherit;True;Property;_TextureSample3;Texture Sample 3;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;19;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;6;-680.2939,476.7406;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-743.7869,-160.4578;Inherit;True;Property;_Foam;Foam;1;0;Create;True;0;0;False;0;False;-1;None;fbe4a1b98373bf54788204db9bf04e2e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-233.7758,596.1895;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-226.0698,178.0826;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;25.30428,235.6217;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;16;153.2666,-5.283883;Inherit;False;Property;_WaterColor;WaterColor;3;1;[HDR];Create;True;0;0;False;0;False;0.4308473,0.4669504,0.8867924,0;0.4308473,0.4669504,0.8867924,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;17;248.0173,182.5345;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;15;152.5951,-195.3555;Inherit;False;Property;_FoamColor;FoamColor;2;1;[HDR];Create;True;0;0;False;0;False;0.2091492,0.9433962,0.7685754,0;0.2091492,0.9433962,0.7685754,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;13;513.1608,-23.71936;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;77;512.0524,365.1081;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;75;111.2982,552.8684;Inherit;False;Property;_VertexOffset;VertexOffset;9;0;Create;True;0;0;False;0;False;1;0.0005;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;309.2981,412.8683;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;73;114.2982,389.8683;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;81;532.4805,182.6284;Inherit;False;Property;_Float1;Float 1;10;0;Create;True;0;0;False;0;False;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;80;781.0912,-17.97194;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Toon/Water;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;56;0;55;1
WireConnection;56;1;55;3
WireConnection;58;0;56;0
WireConnection;58;1;59;0
WireConnection;63;0;62;0
WireConnection;63;1;56;0
WireConnection;51;0;58;0
WireConnection;60;0;56;0
WireConnection;60;1;61;0
WireConnection;31;0;60;0
WireConnection;47;1;51;0
WireConnection;21;0;63;0
WireConnection;49;0;47;1
WireConnection;49;1;48;0
WireConnection;30;1;31;0
WireConnection;6;1;8;0
WireConnection;6;0;7;0
WireConnection;19;1;21;0
WireConnection;46;0;6;0
WireConnection;46;1;49;0
WireConnection;38;0;19;1
WireConnection;38;1;30;1
WireConnection;38;2;6;0
WireConnection;18;0;38;0
WireConnection;18;1;46;0
WireConnection;17;0;18;0
WireConnection;13;0;15;0
WireConnection;13;1;16;0
WireConnection;13;2;17;0
WireConnection;77;2;74;0
WireConnection;74;0;73;2
WireConnection;74;1;75;0
WireConnection;80;2;13;0
WireConnection;80;9;81;0
ASEEND*/
//CHKSM=5B1B39998DF25C51D85D22FDC62BC491E486D8E6