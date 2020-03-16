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
        public static LevelDescription selectedLevelDescription;
        public static string levelNumber;
        public static SaveGame currentSaveGame;
        public static LevelScore currentLevelScore;

        void Awake() {
            levelScene = GameObject.Find("Level Scene").GetComponent<LevelScene>();

            currentLevel = new Level();

            if (selectedLevelDescription != null) {
                LevelSerializer levelSerializer = new LevelSerializer();
                LevelSave levelSave = levelSerializer.GetLevelSave(selectedLevelDescription.levelFile);
                currentLevel.LoadLevel(levelSave);
            }

            if (currentLevelScore == null) {
                currentLevelScore = new LevelScore();
            }
            
            Physics.queriesHitTriggers = true;

        }

        public void SetSelectedLevelDescription(LevelDescription levelDescription) {
            selectedLevelDescription = levelDescription;
            currentLevelScore = new LevelScore();
        }

        public static void Save() {
            GameSerializer gs = new GameSerializer();
            
            gs.SaveGame(currentSaveGame);
        }

        
    }
}