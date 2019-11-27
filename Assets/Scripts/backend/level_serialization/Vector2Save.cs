
using UnityEngine;

namespace backend.level_serialization {
    [System.Serializable]
    public class Vector2Save {
        public float xPos;
        public float yPos;

        public Vector2Save(Vector2 vector) {
            xPos = vector.x;
            yPos = vector.y;
        }

        public Vector2 Vector2() {
            return new Vector2(xPos, yPos);
        }
    }
}