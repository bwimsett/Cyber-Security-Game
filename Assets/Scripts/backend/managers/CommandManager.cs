using System;
using System.Collections.Generic;
using backend;
using UnityEngine;

namespace DefaultNamespace {
    public class CommandManager : MonoBehaviour {

        public string editModeCommand;
        public string setBudgetCommand;
        public string saveLevelCommand;
        public string loadLevelCommand;
        public string clearLevelCommand;
        public string openAttackVisualiserCommand;
        public string zoneShaderCommand;
        public string snapToGridCommand;
        public string goldBoundaryCommand;
        public string silverBoundaryCommand;
        public string bronzeBoundaryCommand;
        
        public void ParseInput(string input) {
            string[] commandList = ExtractCommandList(input);

            foreach (string command in commandList) {
                ExecuteCommand(command);
            }
        }

        private string[] ExtractCommandList(string input) {
            List<string> output = new List<string>();
            string currentString = "";
            
            
            foreach(char i in input) {
                if (i == ' ') {
                    output.Add(currentString);
                    currentString = "";
                    continue;
                }

                currentString += i;
            }

            output.Add(currentString);
            return output.ToArray();
        }

        private void ExecuteCommand(string command) {
            command = command.ToLower();
            
            // Enter level builder
            if (command.Contains(editModeCommand.ToLower())) {
                GameManager.currentLevel.SetEditMode(GetBoolFromValue(GetValueFromCommand(command)));
                Debug.Log("Edit mode: " + GameManager.currentLevel.IsEditMode());
            }
            
            // Set budget
            if (command.Contains(setBudgetCommand.ToLower())) {
                GameManager.currentLevel.SetBudget(GetIntFromValue(GetValueFromCommand(command)));
            }
            
            // Save level
            if (command.Contains(saveLevelCommand.ToLower())) {
                GameManager.currentLevel.SerializeLevel(GetValueFromCommand(command));
            }
            
            // Load level
            if (command.Contains(loadLevelCommand.ToLower())) {
                GameManager.currentLevel.LoadLevel(GetValueFromCommand(command));
            }
            
            // Clear Level
            if (command.Contains(clearLevelCommand.ToLower())) {
                GameManager.currentLevel.ClearLevel();
            }
            
            // Attack visualiser debug panel
            if (command.Contains(openAttackVisualiserCommand.ToLower())) {
                GameManager.levelScene.guiManager.OpenAttackVisualiserDebug();
            }
            
            if (command.Contains(zoneShaderCommand.ToLower())) {
                Camera.main.GetComponent<ZoneCamera>().useZoneShader =
                    GetBoolFromValue(GetValueFromCommand(command));
            }
            
            if (command.Contains(snapToGridCommand.ToLower())) {
                GameManager.levelScene.grid.snapToGrid = GetBoolFromValue(GetValueFromCommand(command));
            }
            
            if (command.Contains(bronzeBoundaryCommand.ToLower())) {
                GameManager.currentLevel.SetMedalBoundary(Medal.Bronze, GetIntFromValue(GetValueFromCommand(command)));
            }
            
            if (command.Contains(silverBoundaryCommand.ToLower())) {
                GameManager.currentLevel.SetMedalBoundary(Medal.Silver, GetIntFromValue(GetValueFromCommand(command)));
            }
            
            if (command.Contains(goldBoundaryCommand.ToLower())) {
                GameManager.currentLevel.SetMedalBoundary(Medal.Gold, GetIntFromValue(GetValueFromCommand(command)));
            }
        }
        
        private string GetValueFromCommand(string text) {
            string value = "";
            bool withinBrackets = false;
            
            for (int i = 0; i < text.Length; i++) {
                if (withinBrackets) {
                    if (text[i] == ')') {
                        return value;
                    }

                    value += text[i];
                    continue;
                }

                if (text[i] == '(') {
                    withinBrackets = true;
                }
            }

            return value;
        }

        private bool GetBoolFromValue(string value) {
            bool result = false;
            result = bool.Parse(value);
            return result;
        }
        
        private int GetIntFromValue(string value) {
            int result = 0;
            result = Int32.Parse(value);
            return result;
        }
    }
}