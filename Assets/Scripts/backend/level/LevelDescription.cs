using UnityEngine;

namespace backend.level {
    [CreateAssetMenu(fileName = "Level Description", menuName = "Level Description")]
    public class LevelDescription : ScriptableObject {
        public string levelName;
        public TextAsset levelFile;
    }
}