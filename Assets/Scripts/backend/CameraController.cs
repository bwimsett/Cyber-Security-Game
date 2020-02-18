using System.ComponentModel;
using DefaultNamespace;
using UnityEngine;

namespace backend {
    public class CameraController : MonoBehaviour {
        public Camera camera;
        public Material zoneMaterial;
        public bool useZoneShader;

        
        [Range(0,0.1f)]
        public float cameraSizePerPixel;

        
        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if (!useZoneShader) {
                Graphics.Blit(src, dest);
                return;
            }
            
            Graphics.Blit(src, dest, zoneMaterial);
        }

        void Start() {
            ResizeToResolution();
        }

        void Update() {
            zoneMaterial.SetColor("_InColor", Color.blue);
            int id = Shader.PropertyToID("asgasg");
            Debug.Log(id);
            Vector4[] nodePositions = GameManager.levelScene.nodeManager.GetNodeScreenPositionsForShader();
            zoneMaterial.SetInt("_NodeCount", nodePositions.Length);
            if (nodePositions.Length > 0) {
                zoneMaterial.SetVectorArray("_NodePositions", nodePositions);
                if (nodePositions.Length > 1) {
                    Debug.Log("Items in shader array: "+zoneMaterial.GetVectorArray("_NodePositions").Length+"\n Items in original array: "+nodePositions.Length);
                }
            }

            ResizeToResolution();
        }

        void ResizeToResolution() {
            int screenWidth = camera.pixelWidth;

            camera.orthographicSize = Mathf.RoundToInt(cameraSizePerPixel * screenWidth);
        }
        
    }
}