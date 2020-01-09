using System;
using backend.level_serialization;
using DefaultNamespace;

namespace backend.serialization {
    public class SaveGame {
        private LevelProgress[] levelProgress;
        private DateTime saveTime;

        public SaveGame(int levelCount) {
            levelProgress = new LevelProgress[levelCount];

            for (int i = 0; i < levelProgress.Length; i++) {
                levelProgress[i] = new LevelProgress();
            }
        }
        
        public void SetLevelProgress(int levelIndex, LevelProgress progress) {
            if (levelIndex >= levelProgress.Length || levelIndex < 0) {
                return;
            }
            
            levelProgress[levelIndex] = progress;
        }

        public Medal GetLevelMedal(int levelId) {
            if (levelId >= levelProgress.Length) {
                return Medal.None;
            }

            return levelProgress[levelId].GetMedal();
        }
        
        public void SetSaveTime(DateTime time) {
            saveTime = time;
        }

        public DateTime GetSaveTime() {
            return saveTime;
        }
    }
}