using backend.level_serialization;
using backend.serialization;
using DefaultNamespace;
using UnityEngine;

namespace backend {
    public class MainMenuLoader : MonoBehaviour {
        public LevelSelector levelSelector;
        
        void Awake() {
            InitialiseMostRecentSave();
        }

        public void InitialiseMostRecentSave() {
            GameSerializer gs = new GameSerializer();
            
            // If the game has not already loaded a save game
            if (GameManager.currentSaveGame == null) {
                GameManager.currentSaveGame = gs.GetMostRecentSave();
            }

            // Create a new save game if none exist
            if (GameManager.currentSaveGame == null) {
                GameManager.currentSaveGame = new SaveGame(levelSelector.levelSets.Length*5);
            }
            
            levelSelector.Refresh();
        }
        
    }
}