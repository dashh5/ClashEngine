// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Toon/ToonGrassTwoSides"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			half filler;
		};

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
976;114;1502;610;706.6116;246.538;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;56;-3086.546,198.8887;Inherit;False;507.201;385.7996;Comment;3;61;59;58;N . V;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;60;-2391.766,368.316;Inherit;False;1617.938;553.8222;;13;96;94;91;88;87;82;78;76;75;74;68;65;63;Rim Light;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;62;-3079.479,-362.4841;Inherit;False;540.401;320.6003;Comment;3;71;67;66;N . L;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;64;-2228.574,-675.1526;Inherit;False;723.599;290;Also know as Lambert Wrap or Half Lambert;3;77;73;70;Diffuse Wrap;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;69;-1943.479,-74.48412;Inherit;False;812;304;Comment;5;93;84;83;81;72;Attenuation and Ambient;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;71;-2679.479,-250.4841;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;91;-1143.767,496.316;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;96;-951.7668,496.316;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-872.8031,-464.3748;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-1120.583,613.2961;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;82;-1373.859,789.5391;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;87;-1415.51,609.5844;Float;False;Property;_RimColor;Rim Color;2;1;[HDR];Create;True;0;0;False;0;False;0,1,0.8758622,0;0,1,0.4360411,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-1437.479,113.5159;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-1335.767,496.316;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-1591.767,416.316;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScaleAndOffsetNode;73;-1939.576,-617.8185;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-1274.285,-23.75286;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-2331.602,632.6021;Float;False;Property;_RimOffset;Rim Offset;4;0;Create;True;0;0;False;0;False;0.24;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;83;-1843.479,-24.48411;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.DotProductOpNode;61;-2734.546,326.8887;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;76;-1595.602,520.6021;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;65;-2123.602,520.6021;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;81;-1684.479,64.51588;Inherit;False;Tangent;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;77;-1679.975,-618.3531;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;-175.1074,425.8469;Inherit;False;Constant;_AlfaClip;AlfaClip;8;0;Create;True;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-577.8792,-241.2841;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-2212.784,-599.4177;Float;False;Property;_WrapperValue;Wrapper Value;1;0;Create;True;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;74;-1787.602,520.6021;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1899.602,648.6021;Float;False;Property;_RimPower;Rim Power;3;0;Create;True;0;0;False;0;False;0.5;0.75;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;85;-1196.456,-444.3331;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;67;-2967.479,-314.4841;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SaturateNode;68;-1963.602,520.6021;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;58;-3038.546,246.8887;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LightAttenuation;72;-1863.079,137.5159;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;97;-231.4794,-10.48411;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;59;-2990.546,406.8887;Float;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;66;-3015.479,-154.4841;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;90;-1189.017,-269.9783;Inherit;False;Property;_AlbedoColor;AlbedoColor;6;0;Create;True;0;0;False;0;False;1,1,1,0;0.1726759,0.4150939,0.1625129,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;151;-618.6328,10.85598;Inherit;False;Property;_Float1;Float 1;7;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;89;-1306.542,-643.8537;Inherit;True;Property;_ToonRamp;Toon Ramp;0;0;Create;True;0;0;False;0;False;-1;None;2d6a94b398b4de44ba13ecab19e49877;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;79;-367.0458,190.8002;Inherit;True;Property;_Albedo;Albedo;5;0;Create;True;0;0;False;0;False;-1;None;165da17c658bc2f42bbbda9628781ebd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;152;9,-11;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Toon/ToonGrassTwoSides;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;71;0;67;0
WireConnection;71;1;66;0
WireConnection;91;0;88;0
WireConnection;96;0;91;0
WireConnection;96;1;94;0
WireConnection;92;0;89;0
WireConnection;92;1;85;0
WireConnection;92;2;90;0
WireConnection;94;0;87;0
WireConnection;94;1;82;0
WireConnection;84;0;81;0
WireConnection;84;1;72;0
WireConnection;88;0;78;0
WireConnection;88;1;76;0
WireConnection;78;0;72;0
WireConnection;78;1;71;0
WireConnection;73;0;71;0
WireConnection;73;1;70;0
WireConnection;73;2;70;0
WireConnection;93;0;83;0
WireConnection;93;1;84;0
WireConnection;61;0;58;0
WireConnection;61;1;59;0
WireConnection;76;0;74;0
WireConnection;76;1;75;0
WireConnection;65;0;61;0
WireConnection;65;1;63;0
WireConnection;77;0;73;0
WireConnection;95;0;92;0
WireConnection;95;1;93;0
WireConnection;74;0;68;0
WireConnection;68;0;65;0
WireConnection;97;0;95;0
WireConnection;97;1;96;0
WireConnection;89;1;77;0
ASEEND*/
//CHKSM=43566B11A03C1161F58252EDCAE97B2506685A71