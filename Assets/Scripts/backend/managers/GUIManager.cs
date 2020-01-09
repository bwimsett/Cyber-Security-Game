using gui;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace {
    public class GUIManager : MonoBehaviour {
        public ControlDescriptionWindow controlDescriptionWindow;
        public TextMeshProUGUI budgetText;

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
    }
}