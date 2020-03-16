using backend;
using GameAnalyticsSDK.Setup;
using UnityEngine;

namespace DefaultNamespace {
    public class LevelScene : MonoBehaviour{
        public ConnectionManager connectionManager;
        public NodeManager nodeManager;
        public GUIManager guiManager;
        public ThreatManager threatManager;
        public AttackVisualiser attackVisualiser;
        public LevelGrid grid;
        
        public void TriggerLevelTitleRefresh() {
            GameManager.levelScene.guiManager.levelNameText.SetText(GameManager.selectedLevelDescription.levelName);
            GameManager.levelScene.guiManager.levelNameText.triggerRefresh = true;
        }

        public void SetControlMenuInteractable() {
            GameManager.levelScene.guiManager.controlsMenu.SetInteractable(true);
        }
    }
}