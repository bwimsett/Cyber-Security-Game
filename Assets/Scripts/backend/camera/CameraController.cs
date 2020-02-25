using System.ComponentModel;
using DefaultNamespace;
using UnityEngine;

namespace backend {
    public class CameraController : MonoBehaviour {
        public Camera camera;
        
        [Range(0,0.1f)]
        public float cameraSizePerPixel;

        void Start() {
            ResizeToResolution();
        }

        void Update() {
            ResizeToResolution();
        }

        void ResizeToResolution() {
            int screenWidth = camera.pixelWidth;
            camera.orthographicSize = Mathf.RoundToInt(cameraSizePerPixel * screenWidth);
        }
        
    }
}