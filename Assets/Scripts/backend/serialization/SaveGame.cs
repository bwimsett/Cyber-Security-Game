using System;
using backend.level;
using backend.level_serialization;
using DefaultNamespace;

namespace backend.serialization {
    [Serializable]
    public class SaveGame {
        private LevelScore[] levelScore;
        private DateTime saveTime;

        public SaveGame(int levelCount) {
            levelScore = new LevelScore[levelCount];

            for (int i = 0; i < levelScore.Length; i++) {
                levelScore[i] = new LevelScore();
            }
        }
        
        public void SetLevelProgress(int levelIndex, LevelScore progress) {
            if (levelIndex >= levelScore.Length || levelIndex < 0) {
                return;
            }
            
            levelScore[levelIndex] = progress;
        }

        public Medal GetLevelMedal(int levelIndex) {
            LevelScore prog = GetLevelScore(levelIndex);
            
            if (prog == null) {
                return Medal.None;
            }

            return prog.medal;
        }

        public LevelScore GetLevelScore(int levelIndex) {
            if (levelIndex >= levelScore.Length) {
                return null;
            }

            return levelScore[levelIndex];
        }

        public void SetSaveTime(DateTime time) {
            saveTime = time;
        }

        public DateTime GetSaveTime() {
            return saveTime;
        }
    }
}