using UnityEngine;

namespace DefaultNamespace {
    public class GameManager : MonoBehaviour{

        public static LevelScene levelScene;
        public static Level currentLevel;

        void Start() {
            levelScene = GameObject.Find("Level Scene").GetComponent<LevelScene>();
            currentLevel = new Level();
        }

    }
}