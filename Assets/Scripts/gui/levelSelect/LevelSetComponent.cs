using System.Reflection.Emit;
using backend.level;
using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace gui.levelSelect {
    public class LevelSetComponent : MonoBehaviour {

        private LevelSetMedal[] medals;
        private LevelSetConnection[] connections;
        public GameObject medalPrefab;
        public GameObject levelSetConnectionPrefab;
        public Transform medalContainer;
        public Transform connectionContainer;
        private LevelSet levelSet;

        public TextMeshProUGUI levelSetTitle;
        public TextMeshProUGUI levelSetTokens;
        public TextMeshProUGUI levelSetUnlockRequirements;

        
        private int levelSetPos;
        
        public void SetLevels(LevelSet levelSet, int levelSetPos) {
            this.levelSet = levelSet;
            this.levelSetPos = levelSetPos;
            levelSetTitle.text = levelSet.setName;
            levelSetUnlockRequirements.text = levelSet.unlockCost+" tokens";
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

            bool locked = GameManager.currentSaveGame.GetTokens() < levelSet.unlockCost;
            
            // Instantiate medal
            for (int i = 0; i < levelSet.levels.Length; i++) {     
                medals[i] = Instantiate(medalPrefab, medalContainer).GetComponent<LevelSetMedal>();
                medals[i].name = ""+i;
                medals[i].SetLevel(levelSet.levels[i], levelSetPos*5+i);
                medals[i].SetLock(locked);
            }
            
            connections = new LevelSetConnection[medals.Length-1];
            
            // Instantiate medal connections
            for (int i = 0; i < connections.Length; i++) {
                connections[i] = Instantiate(levelSetConnectionPrefab, connectionContainer)
                    .GetComponent<LevelSetConnection>();
                connections[i].SetMedals(medals[i], medals[i+1]);
            }
        }

        public void UpdateConnectionOpacity(int opacity) {
            foreach (LevelSetConnection c in connections) {
                Color startColor = c.lineRenderer.startColor;
                Color endColor = c.lineRenderer.endColor;

                startColor.a = opacity;
                endColor.a = opacity;

                c.lineRenderer.startColor = startColor;
                c.lineRenderer.endColor = endColor;
            }
        }

    }
}