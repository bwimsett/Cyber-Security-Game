using DefaultNamespace;
using UnityEngine;

namespace backend {
    public class LevelGrid : MonoBehaviour {
        public float gridScale;


        public Vector2 GetGridPos(Vector2 position) {
            
            // Only snap in edit mode
            if (!GameManager.currentLevel.IsEditMode()) {
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