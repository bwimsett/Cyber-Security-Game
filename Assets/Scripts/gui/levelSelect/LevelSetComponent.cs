using backend.level;
using TMPro;
using UnityEngine;

namespace gui.levelSelect {
    public class LevelSetComponent : MonoBehaviour {

        private LevelSetMedal[] medals;
        private LevelSetConnection[] connections;
        public GameObject medalPrefab;
        public Transform medalContainer;
        private LevelSet levelSet;

        private int levelSetPos;
        
        public void SetLevels(LevelSet levelSet, int levelSetPos) {
            this.levelSet = levelSet;
            this.levelSetPos = levelSetPos;
            Refresh();
        }

        private void Clear() {
            if (medals == null) {
                return;
            }
            
            foreach (LevelSetMedal lsm in medals) {
                Destroy(lsm.gameObject);
            }
        }
        
        public void Refresh() {
            Clear();
            medals = new LevelSetMedal[levelSet.levels.Length];
            
            for (int i = 0; i < levelSet.levels.Length; i++) {
                medals[i] = Instantiate(medalPrefab, medalContainer).GetComponent<LevelSetMedal>();
                medals[i].name = ""+i;
                medals[i].SetLevel(levelSet.levels[i], levelSetPos*5+i);
            }
        }

    }
}