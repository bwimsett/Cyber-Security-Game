using System.Collections.Generic;
using backend.level_serialization;
using UnityEngine;

namespace DefaultNamespace {
    
    public class Level {

        private int budget;
        private bool editMode = true;
        public List<Node> nodes;
        private int currentNodeId;

        public Level() {
            currentNodeId = 0;
            nodes = new List<Node>();
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

        public int GetNewNodeID() {
            currentNodeId++;
            return currentNodeId;
        }

        public void SerializeLevel(string levelName) {
            LevelSerializer lvlSerializer = new LevelSerializer();
            lvlSerializer.SaveCurrentLevelAsTemplate(levelName);
        }
        
    }
}