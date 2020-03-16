using System.Collections;
using System.Collections.Generic;
using backend;
using backend.serialization;
using DefaultNamespace;
using UnityEngine;

public class NewGameOverwritePopup : MonoBehaviour {

    public MainMenuLoader mainMenuLoader;
    public MainMenu mainMenu;

    public void OnEnable() {
        mainMenu.mainMenuCanvas.interactable = false;
    }
    
    public void Yes() {
        mainMenuLoader.NewGame();
        mainMenu.LevelSelect();
        gameObject.SetActive(false);
        mainMenu.mainMenuCanvas.interactable = true;
    }

    public void Cancel() {
        gameObject.SetActive(false);
        mainMenu.mainMenuCanvas.interactable = true;
    }
    
}
