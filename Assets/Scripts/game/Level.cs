using System;
using System.Collections.Generic;
using backend.level_serialization;
using gui.controlsmenu;
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
            for(int i = 0; i < nodes.Count; i++){
                GameManager.levelScene.connectionManager.CreateConnectionsFromIDArray(nodes[i], levelSave.nodes[i].connectedNodes);
            }
            
            
        }
        
    }
}