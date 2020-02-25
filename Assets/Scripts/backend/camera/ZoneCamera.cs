using DefaultNamespace;
using UnityEngine;

namespace backend {
    public class ZoneCamera : MonoBehaviour {
        
        public bool useZoneShader;
        public Material zoneMaterial;

        void Update() {
            zoneMaterial.SetColor("_InColor", Color.blue);
            int id = Shader.PropertyToID("asgasg");
            //Debug.Log(id);
            Vector4[] nodePositions = GameManager.levelScene.nodeManager.GetNodeScreenPositionsForShader();
            zoneMaterial.SetInt("_NodeCount", nodePositions.Length);
            if (nodePositions.Length > 0) {
                zoneMaterial.SetVectorArray("_NodePositions", nodePositions);
                if (nodePositions.Length > 1) {
                    //Debug.Log("Items in shader array: "+zoneMaterial.GetVectorArray("_NodePositions").Length+"\n Items in original array: "+nodePositions.Length);
                }
            }
        }
        
        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if (!useZoneShader) {
                Graphics.Blit(src, dest);
                return;
            }
            
            Graphics.Blit(src, dest, zoneMaterial);
        }
        
    }
}