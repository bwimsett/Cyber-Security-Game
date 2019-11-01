using UnityEngine;

namespace DefaultNamespace {
    public class Level {

        private int budget;
        private bool editMode;


        public Level() {
            editMode = false;
        }

        public void SetEditMode(bool value) {
            editMode = value;
        }

        public bool IsEditMode() {
            return editMode;
        }

        public void SetBudget(int amount) {
            if (editMode) {
                budget = amount;
                GameManager.levelScene.guiManager.RefreshBudget();
            }
        }

        public int GetBudget() {
            return budget;
        }
        
    }
}