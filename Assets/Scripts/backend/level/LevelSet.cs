using UnityEngine;

namespace backend.level {
    [CreateAssetMenu(fileName = "Level Set", menuName = "Level Set")]
    public class LevelSet : ScriptableObject {
        public string setName;
        public int unlockCost;
        public LevelDescription[] levels;
    }
}