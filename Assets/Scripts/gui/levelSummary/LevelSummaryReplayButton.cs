using backend;
using DefaultNamespace;
using GameAnalyticsSDK.Setup;
using UnityEngine;

namespace gui.levelSummary {
    public class LevelSummaryReplayButton : MonoBehaviour{
        private int threatIndex;

        public void SetThreatindex(int threatIndex) {
            this.threatIndex = threatIndex;
        }

        public void ReplayThreat() {
            GameManager.levelScene.guiManager.DisplayLevelScene(true);
            GameManager.levelScene.guiManager.OpenThreatSummary();
            GameManager.levelScene.guiManager.threatSummaryScreen.SetThreatIndex(threatIndex);
        }
    }
}