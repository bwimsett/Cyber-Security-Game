using gui;
using TMPro;
using UnityEngine;

namespace DefaultNamespace {
    public class GUIManager : MonoBehaviour {
        public ControlDescriptionWindow controlDescriptionWindow;
        public TextMeshProUGUI budgetText;

        public Vector2 GetMousePosition() {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return mousePos;
        }

        public void RefreshBudget() {
            budgetText.text = "Budget | " + GameManager.currentLevel.GetBudget()+" hrs";
        }
    }
}