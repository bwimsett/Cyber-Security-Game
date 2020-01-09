using UnityEngine;

namespace backend.level {
    [CreateAssetMenu(fileName = "Level Set", menuName = "Level Set")]
    public class LevelSet : ScriptableObject {
        public int unlockCost;
        public LevelDescription[] levels;
    }
}