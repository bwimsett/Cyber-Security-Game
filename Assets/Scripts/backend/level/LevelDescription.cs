using UnityEngine;

namespace backend.level {
    [CreateAssetMenu(fileName = "Level Description", menuName = "Level Description")]
    public class LevelDescription : ScriptableObject {
        public int levelIndex;
        public string levelName;
        public TextAsset levelFile;
        public int unlockTokens;
        public LevelDescription nextLevel;
    }
}