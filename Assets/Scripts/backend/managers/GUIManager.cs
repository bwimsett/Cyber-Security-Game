using gui;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class GUIManager : MonoBehaviour {
        public ControlDescriptionWindow controlDescriptionWindow;
        public TextMeshProUGUI budgetText;
        public ControlSettingsWindow controlSettingsWindow;

        public Vector2 GetMousePosition() {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return mousePos;
        }

        public void RefreshBudget() {
            SetBudgetText(GameManager.currentLevel.GetBudget());
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


    }
    
}