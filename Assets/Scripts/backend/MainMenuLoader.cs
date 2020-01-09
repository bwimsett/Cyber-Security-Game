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
            GameManager.currentSaveGame = gs.GetMostRecentSave();

            if (GameManager.currentSaveGame == null) {
                GameManager.currentSaveGame = new SaveGame(levelSelector.levelSets.Length*5);
            }
            
            levelSelector.Refresh();
        }
        
    }
}