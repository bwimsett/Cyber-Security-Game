using UnityEngine;

namespace backend {
    public class LevelGrid : MonoBehaviour {
        public float gridScale;
        public bool snapToGrid;


        public Vector2 GetGridPos(Vector2 position) {
            if (!snapToGrid) {
                return position;
            }
            
            float xDistance = position.x % gridScale;
            float yDistance = position.y % gridScale;
            
            Vector2 distance = new Vector2(xDistance, yDistance);

            Vector2 newPos = position - distance;

            //Debug.Log(newPos.x + " " + newPos.y);
            
            return newPos;
        }
    }
}