using backend;
using gui;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class GUIManager : MonoBehaviour {
        public ControlDescriptionWindow controlDescriptionWindow;
        public TextMeshProUGUI budgetText;
        public TextMeshProUGUI tokensText;
        public ControlSettingsWindow controlSettingsWindow;
        public AttackVisualiserDebugPanel AttackVisualiserDebugPanel;
        public ControlsMenu controlsMenu;
        public LevelSummaryWindow levelSummaryWindow;
        public LevelNameText levelNameText;
        public ThreatSummaryScreen threatSummaryScreen;
        public Animator levelSceneAnimator;
        public Animator trashAnimator;
        
        void Start() {
            RefreshBudget();
        }
        
        public Vector2 GetMousePosition() {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return mousePos;
        }

        public void RefreshBudget() {
            GameManager.currentLevel.RecalculateBudget();
            
            SetBudgetText(GameManager.currentLevel.GetRemainingBudget());
            tokensText.text = GameManager.currentSaveGame.GetTokens() + " Tokens";
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

        public void OpenThreatSummary() {
            levelNameText.textField.enabled = false;
            threatSummaryScreen.gameObject.SetActive(true);
            threatSummaryScreen.SetThreats(GameManager.levelScene.threatManager.GetThreats(ThreatStatus.Success));
        }

        public void HideThreatSummary() {
            threatSummaryScreen.gameObject.SetActive(false);
            levelNameText.textField.enabled = true;
        }

        public void DisplayLevelSummary() {
            RefreshBudget();
            Threat[] failedThreats = GameManager.levelScene.threatManager.GetThreats(ThreatStatus.Failure);
            Threat[] successfulThreats = GameManager.levelScene.threatManager.GetThreats(ThreatStatus.Success);
            GameManager.currentLevel.CalculateScore(successfulThreats, failedThreats);
            GameManager.levelScene.guiManager.RefreshBudget();
            levelSummaryWindow.SetVisible(true);
            levelSceneAnimator.SetTrigger("swipeout");
            levelSummaryWindow.Refresh();         
            AnalyticsManager.PauseTimer();
        }

        public void DisplayLevelScene(bool fromLeft) {
            if (fromLeft) {
                levelSceneAnimator.SetTrigger("swipeinleft");
                levelSummaryWindow.animator.SetTrigger("swipeoutright");
                GameManager.levelScene.attackVisualiser.ClearVisualisation();
                HideThreatSummary();
                AnalyticsManager.ResumeTimer();
                return;
            }
            levelSummaryWindow.animator.SetTrigger("swipeout");
            levelSceneAnimator.SetTrigger("swipein");
            AnalyticsManager.ResumeTimer();
        }

        public void BugReport() {
            string link =
                "https://docs.google.com/forms/d/e/1FAIpQLSdkV3hDfOIQ8UW5pCagnO_p4JB9pZhUKnQ33KUE1-2D_WrzcQ/viewform?usp=pp_url&entry.643683302=";
            string levelName = GameManager.selectedLevelDescription.levelName;

            
            string finalLink = link + levelName;
            
            Application.OpenURL(finalLink);
        }

        public void ShowTrash() {
            trashAnimator.SetBool("Visible", true);
        }

        public void ShakeTrash() {
            trashAnimator.SetTrigger("Shake");
        }

        public void HideTrash() {
            trashAnimator.SetBool("Visible", false);
        }
       
        
    }
    
}