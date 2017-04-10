Shader "Unlit/ballShader"
{
	SubShader{
			Pass{
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members pos,col)
			CGPROGRAM

			#pragma exclude_renderers d3d11_9x d3d11 xbox360

			#pragma target 5.0  
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct Point {
				float3 pos;
				float3 vel;
			};
			struct v2f {
				float4 pos : SV_POSITION;
				float3 col : COLOR0;
			};
			float x, y, z;
			float4x4 ModelViewProjection;
			StructuredBuffer<Point> value;

			v2f vert(uint id : SV_VertexID) {
				v2f o;
				float3 worldPos = value[id].pos;
				o.pos = mul(UNITY_MATRIX_VP, worldPos);
				//o.pos = mul(ModelViewProjection,float4(worldPos,1.0f);
				//o.pos = float4(worldPos, 1.0f);
				o.col.x = (sin(float((id)*(3*float(x)))) * 127 + 128) / 255.0f;
				o.col.y = (sin(float((id)*(3*float(y)) + 2)) * 127 + 128) / 255.0f;
				o.col.z = (sin(float((id)*(3*float(z)) + 4)) * 127 + 128) / 255.0f;
				return o;
			}
			float4 frag(v2f i) : COLOR{
				return float4(i.col,1.0f);
			}
			ENDCG
		}
	}
	Fallback OFF
}
