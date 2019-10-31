using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class DeveloperConsole : MonoBehaviour {

    public TMP_InputField inputField;
    public CommandManager commandManager;
 
    public void EnterCommand() {
        commandManager.ParseInput(inputField.text);
    }
}
