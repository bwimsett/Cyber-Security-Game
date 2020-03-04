using System.Collections.Generic;
using backend.level;
using backend.level_serialization;
using backend.serialization;
using UnityEngine;

namespace DefaultNamespace {
    public class GameManager : MonoBehaviour {

        public int scrambledLevelNameCount;
        
        public static LevelScene levelScene;
        public static Level currentLevel;
        public static LevelSave selectedLevelSave;
        public static string levelName;
        public static string levelNumber;
        public static SaveGame currentSaveGame;
        public static LevelScore currentLevelScore;

        void Awake() {
            levelScene = GameObject.Find("Level Scene").GetComponent<LevelScene>();

            currentLevel = new Level();

            if (selectedLevelSave != null) {
                currentLevel.LoadLevel(selectedLevelSave);
            }

            if (currentLevelScore == null) {
                currentLevelScore = new LevelScore();
            }
            
            Physics.queriesHitTriggers = true;

        }

        public static void Save() {
            GameSerializer gs = new GameSerializer();
            
            gs.SaveGame(currentSaveGame);
        }

        
    }
}