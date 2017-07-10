// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/veloShader"
{
	SubShader{
		Pass{
		// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members pos,col)
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma target 3.0
#include "UnityCG.cginc"

		struct Point {
		float3 pos;
		float3 vel;
		float mass;
	};
	struct v2f {
		float4 pos : SV_POSITION;
		float3 col : COLOR0;
	};
	float x, y, z;
	StructuredBuffer<Point> value;

	v2f vert(uint id : SV_VertexID) {
		v2f o;
		float3 worldPos = value[id].pos;
		o.pos = UnityObjectToClipPos(worldPos);
		o.col = abs(normalize(value[id].vel));
		return o;
	}
	float4 frag(v2f i) : COLOR{
		return float4(i.col,1.0f);
	}
		ENDCG
	}
	}
		Fallback Off
}
