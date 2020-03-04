using backend;
using gui;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class GUIManager : MonoBehaviour {
        public ControlDescriptionWindow controlDescriptionWindow;
        public TextMeshProUGUI budgetText;
        public ControlSettingsWindow controlSettingsWindow;
        public AttackVisualiserDebugPanel AttackVisualiserDebugPanel;
        public ControlsMenu controlsMenu;
        public LevelSummaryWindow levelSummaryWindow;
        public LevelNameText levelNameText;
        public ThreatSummaryScreen threatSummaryScreen;
        public Animator levelSceneAnimator;
        
        void Start() {
            RefreshBudget();
        }
        
        public Vector2 GetMousePosition() {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return mousePos;
        }

        public void RefreshBudget() {
            SetBudgetText(GameManager.currentLevel.GetRemainingBudget());
        }

        public void SetBudgetText(int value) {
            budgetText.text = "Budget | " +value+" hrs";
        }

        public void Back() {
            SceneManager.LoadScene(0);
        }

        public void OpenControlSettingsWindow(Node node) {
            controlSettingsWindow.SetNode(node);
            controlSettingsWindow.gameObject.SetActive(true);
        }

        public void CloseControlSettingsWindow() {
            controlSettingsWindow.gameObject.SetActive(false);
        }

        public void OpenAttackVisualiserDebug() {
            AttackVisualiserDebugPanel.gameObject.SetActive(true);
        }

        public void SetThreatsForSummary(Threat[] threats) {
            levelNameText.gameObject.SetActive(false);
            threatSummaryScreen.gameObject.SetActive(true);
            threatSummaryScreen.SetThreats(threats);
        }

        public void DisplayLevelSummary() {
            levelSummaryWindow.SetVisible(true);
            levelSceneAnimator.SetTrigger("swipeout");
            levelSummaryWindow.Refresh();
        }
        
    }
    
}