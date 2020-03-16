using System;
using System.Collections.Generic;
using System.Linq;
using backend;
using backend.level;
using backend.level_serialization;
using backend.serialization;
using DefaultNamespace.node;
using gui.controlsmenu;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Setup;
using UnityEngine;

namespace DefaultNamespace {
    
    public class Level {

        private int budget;
        private int remainingBudget;
        private bool editMode = true;
        public List<Node> nodes;
        private int currentNodeId;
        private int attempts = 0;
        private string levelName;

        private LevelScore levelScore;

        private int bronzeScore = 100;
        private int silverScore = 750;
        private int goldScore = 1000;

        public Level() {
            currentNodeId = 0;
            nodes = new List<Node>();
        }
        
        public Level(LevelScore levelScore) {
            currentNodeId = 0;
            nodes = new List<Node>();
            this.levelScore = levelScore;
        }

        public void CalculateScore(Threat[] successfulThreats, Threat[] failedThreats) {
            // Calculate score
            GameManager.currentLevelScore.CalculateScore(successfulThreats, failedThreats);

            if (!IsEditMode()) {
                // Refresh level summary window
                GameManager.levelScene.guiManager.levelSummaryWindow.Refresh();

                // Save game
                GameManager.Save();
            }
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
            RefreshNodeEditMode();
        }

        private void RefreshNodeEditMode() {
            foreach (Node n in nodes) {
                n.nodeObject.RefreshEditMode();
            }
        }

        public bool IsEditMode() {
            return editMode;
        }

        public void SetBudget(int amount) {
            budget = amount;
            RecalculateBudget();
        }

        public int GetBudget() {
            return budget;
        }

        public int GetRemainingBudget() {
            return remainingBudget;
        }

        public void SetRemainingBudget(int amount) {
            remainingBudget = amount;
            GameManager.levelScene.guiManager.SetBudgetText(amount);
        }

        public int GetAttempts() {
            return attempts;
        }

        public bool CanPurchaseForAmount(int amount) {
            if (remainingBudget < amount && !editMode) {
                return false;
            }

            return true;
        }

        public void RecalculateBudget() {

            int totalCost = 0;
            
            foreach (Node n in nodes) {
                int nodeCost = 0;
                
                // Get cost of node
                totalCost += n.nodeObject.GetNodeDefinition().nodeCost;
                
                // Get cost of node config
                NodeField[] fields = n.GetBehaviour().GetFields();

                foreach (NodeField f in fields) {
                    nodeCost += f.GetCost();
                }

                totalCost += nodeCost;
            }
            
            SetRemainingBudget(budget - totalCost);
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
            
            GameManager.levelScene.guiManager.HideThreatSummary();
            GameManager.levelScene.guiManager.RefreshBudget();
            

            SetEditMode(false);
        }

        public void LoadLevel(LevelDescription levelDescription) {
            // Set game manager level description to new level
            GameManager.selectedLevelDescription = levelDescription;
            
            // Set game manager level progress
            GameManager.currentLevelScore = GameManager.currentSaveGame.GetLevelScore(levelDescription.levelIndex);
            
            // Extract save file
            LevelSerializer levelSerializer = new LevelSerializer();
            LevelSave levelSave = levelSerializer.GetLevelSave(levelDescription.levelFile);
            
            // Load level
            LoadLevel(levelSave);
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