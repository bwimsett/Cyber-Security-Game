using System;
using backend.level;
using backend.level_serialization;
using DefaultNamespace;
using UnityEngine;

namespace backend.serialization {
    [Serializable]
    public class SaveGame {
        private LevelScore[] levelScores;
        private DateTime saveTime;

        public SaveGame(int levelCount) {
            levelScores = new LevelScore[levelCount];

            for (int i = 0; i < levelScores.Length; i++) {
                levelScores[i] = new LevelScore();
            }
        }
        
        public void SetLevelProgress(int levelIndex, LevelScore progress) {
            if (levelIndex >= levelScores.Length || levelIndex < 0) {
                return;
            }
            
            levelScores[levelIndex] = progress;
        }

        public Medal GetLevelMedal(int levelIndex) {
            LevelScore prog = GetLevelScore(levelIndex);
            
            if (prog == null) {
                return Medal.None;
            }

            return prog.medal;
        }

        public LevelScore GetLevelScore(int levelIndex) {
            if (levelIndex >= levelScores.Length) {
                return null;
            }

            return levelScores[levelIndex];
        }

        public int GetTokens() {
            int total = 0;
            
            foreach (LevelScore score in levelScores) {
                total += score.GetTokens();
            }

            return total;
        }

        public int GetMaxTokens() {
            int max = 0;

            foreach (LevelScore score in levelScores) {
                max += score.GetMaxTokens();
            }

            return max;
        }

        public int GetPercentageCompletion() {
            float totalTokens = GetTokens();
            float maxTokens = GetMaxTokens();

            int percentage = Mathf.RoundToInt(totalTokens / maxTokens * 100);

            return percentage;
        }

        public void SetSaveTime(DateTime time) {
            saveTime = time;
        }

        public DateTime GetSaveTime() {
            return saveTime;
        }
    }
}