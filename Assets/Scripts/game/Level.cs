using System;
using System.Collections.Generic;
using System.Linq;
using backend;
using backend.level;
using backend.level_serialization;
using backend.serialization;
using gui.controlsmenu;
using UnityEngine;

namespace DefaultNamespace {
    
    public class Level {

        private int budget;
        private bool editMode = true;
        public List<Node> nodes;
        private int currentNodeId;
        private int attempts = 0;

        private LevelScore levelScore;

        private int bronzeScore;
        private int silverScore;
        private int goldScore;

        public Level() {
            currentNodeId = 0;
            nodes = new List<Node>();
        }
        
        public Level(LevelScore levelScore) {
            currentNodeId = 0;
            nodes = new List<Node>();
            this.levelScore = levelScore;
        }

        public void CalculateScore(Threat[] failedThreats) {
            // Calculate score
            GameManager.currentLevelScore.CalculateScore(failedThreats.ToArray());
                
            // Refresh level summary window
            GameManager.levelScene.guiManager.levelSummaryWindow.Refresh();       
                
            // Save game
            GameSerializer serializer = new GameSerializer(); 
            serializer.SaveGame(GameManager.currentSaveGame);
        }
        
        public void SetLevelScore(LevelScore levelScore) {
            this.levelScore = levelScore;
        }

        public LevelScore GetLevelScore() {
            return levelScore;
        }

        public void SetEditMode(bool value) {
            editMode = value;
            GameManager.levelScene.guiManager.controlsMenu.RefreshOptions();
        }

        public bool IsEditMode() {
            return editMode;
        }

        public void SetBudget(int amount) {
            if (editMode) {
                budget = amount;
                GameManager.levelScene.guiManager.SetBudgetText(budget);
            }
        }

        public int GetBudget() {
            return budget;
        }

        public int GetAttempts() {
            return attempts;
        }

        public bool PurchaseForAmount(int amount) {
            if (amount > budget) {
                return false;
            }

            budget -= amount;

            GameManager.levelScene.guiManager.RefreshBudget();
            
            return true;
        }

        public int GetNewNodeID() {
            currentNodeId++;
            return currentNodeId;
        }

        public void SerializeLevel(string levelName) {
            LevelSerializer lvlSerializer = new LevelSerializer();
            lvlSerializer.SaveCurrentLevelAsTemplate(levelName);
        }

        public void ClearLevel() {
            //Destroy connections
            Connection[] connections = GameManager.levelScene.connectionManager.GetConnections();
            foreach (Connection c in connections) {
                GameManager.levelScene.connectionManager.RemoveConnection(c);
            }
            
            //Destroy Nodes
            for (int i = nodes.Count - 1; i >= 0; i--) {
                nodes[i].nodeObject.Destroy();
            }
            
            nodes = new List<Node>();
        }
        
        public void LoadLevel(String name) {
            LevelSerializer ls = new LevelSerializer();
            LevelSave levelSave = ls.GetLevelSave(name);
            LoadLevel(levelSave);
        }

        public void LoadLevel(LevelSave levelSave) {
            if (levelSave == null) {
                return;
            }


            ClearLevel();


            // Set budget
            SetBudget(levelSave.budget);
            
            // Set current node id
            currentNodeId = levelSave.currentNodeID;
            
            // Load nodes
            foreach (NodeSave n in levelSave.nodes) {
                GameManager.levelScene.nodeManager.CreateNodeFromSave(n);
            }
           
            // Create node connections
            foreach (ConnectionSave c in levelSave.connections) {
                GameManager.levelScene.connectionManager.CreateConnectionFromSave(c);
            }

            attempts = levelSave.attempts;

            bronzeScore = levelSave.bronzeScore;
            silverScore = levelSave.silverScore;
            goldScore = levelSave.goldScore;
        }

        public void SetMedalBoundary(Medal medalType, int value) {
            switch (medalType) {
                case Medal.Bronze: bronzeScore = value;
                    break;
                case Medal.Silver: silverScore = value;
                    break;
                case Medal.Gold: goldScore = value;
                    break;
            }

            if (bronzeScore >= silverScore) {
                silverScore = bronzeScore + 1;
            }

            if (silverScore >= goldScore) {
                goldScore = silverScore + 1;
            }

            Debug.Log("New medal boundaries, Bronze: " + bronzeScore + " Silver: " + silverScore + " Gold: " + goldScore);
        }

        public int GetMedalBoundary(Medal medalType) {
            switch (medalType) {
                case Medal.Bronze: return bronzeScore;
                case Medal.Silver: return silverScore;
                case Medal.Gold: return goldScore;
            }

            return 0;
        }
        
        
    }
}