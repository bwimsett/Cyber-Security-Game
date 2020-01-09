using DefaultNamespace;

namespace backend.level_serialization {
    [System.Serializable]
    public class LevelProgress {
        private int score;
        private Medal medal;

        public LevelProgress() {
            score = 0;
            medal = Medal.None;
        }

        public Medal GetMedal() {
            return medal;
        }
    }
}