using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace {
    public class CommandManager : MonoBehaviour {

        public string editModeCommand;
        public string setBudgetCommand;
        public string saveLevelCommand;
        
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
            }
            
            // Set budget
            if (command.Contains(setBudgetCommand.ToLower())) {
                GameManager.currentLevel.SetBudget(GetIntFromValue(GetValueFromCommand(command)));
            }
            
            //Save level
            if (command.Contains(saveLevelCommand.ToLower())) {
                GameManager.currentLevel.SerializeLevel(GetValueFromCommand(command));
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