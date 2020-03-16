using System.Collections;
using System.Collections.Generic;
using backend;
using backend.serialization;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

    public Animator mainMenuGroupAnimator;
    public Animator levelSelectGroupAnimator;
    public Animator mainMenuTitleAnimator;
    public GameObject newGamePopup;
    public MainMenuLoader loader;
    public CanvasGroup mainMenuCanvas;
    public LevelSelector levelSelector;

    public Button mainMenuButton;
    public Button continueButton;

    void Start() {
        RefreshButtons();
    }

    private void RefreshButtons() {
        if (GameManager.currentSaveGame == null) {
            continueButton.interactable = false;
            return;
        }

        continueButton.interactable = true;
    }

    public void LevelSelect() {
        mainMenuButton.interactable = true;
        levelSelector.Refresh();
        mainMenuGroupAnimator.SetTrigger("fadeOut");
        levelSelectGroupAnimator.SetTrigger("fadeIn");
        mainMenuTitleAnimator.SetTrigger("shrink");
    }

    public void BackToMainMenu() {
        mainMenuButton.interactable = false;
        RefreshButtons();
        mainMenuGroupAnimator.SetTrigger("fadeIn");
        levelSelectGroupAnimator.SetTrigger("fadeOut");
        mainMenuTitleAnimator.SetTrigger("grow");
    }

    public void NewGame() {
        if (GameManager.currentSaveGame != null) {
            newGamePopup.gameObject.SetActive(true);
            return;
        }
        
        loader.NewGame();
        LevelSelect();
    }

    public void Quit() {
        GameManager.Save();
        Application.Quit();
    }

}
