Shader "Hidden/ZoneShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //_ZoneThreshold("ZoneThreshold", Float) = 0.5
        _InColor ("InColor", Color) = (0,0,0,0)
        _BlobRadius("BlobRadius", float) = 1000
        _BlobThreshold("BlobThreshold", int) = 500
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : POSITION;

            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            uniform int _NodeCount;
            uniform float4 _NodePositions[256];
            float4 _InColor;
            float _BlobRadius;
            int _BlobThreshold;
            
            fixed4 frag (v2f i) : SV_Target
            {
            
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 screenPos = i.vertex;
                
                float pixelHeat = 0; // A value of how close a pixel is to any number of nodes
                
                for(int pos = 0; pos < 256; pos++){
                    float2 objectPos = _NodePositions[pos].xy;
                    float distanceToNode = distance(screenPos, objectPos);
                    
                    //float heatForThisNode = pow((1.0-(pow(distanceToNode,2)/pow(_BlobWidth, 2))), 2);
                    
                    float heatForThisNode = _BlobRadius / sqrt(pow((screenPos.x - objectPos.x), 2) + pow((screenPos.y - objectPos.y), 2));
                    
                    if(distanceToNode > _BlobRadius){
                        heatForThisNode = 0;
                    }
               
                    bool isEmptyVector = objectPos.xy == (0,0);
                    
                    if(!isEmptyVector){
                        pixelHeat += heatForThisNode;
                    }
                }
                
                pixelHeat = min(_BlobRadius, pixelHeat);
                float shade = _BlobRadius/pixelHeat;
                
                if(pixelHeat > _BlobThreshold) {
                    col.r = shade;
                }

                
                // just invert the colors
                // col.rgb = _InColor.rgb;
                return col;
            }
            ENDCG
        }
    }
}
