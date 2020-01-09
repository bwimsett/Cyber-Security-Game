using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public Animator mainMenuGroupAnimator;
    public Animator levelSelectGroupAnimator;
    public Animator mainMenuTitleAnimator;

    public void LevelSelect() {
        mainMenuGroupAnimator.SetTrigger("fadeOut");
        levelSelectGroupAnimator.SetTrigger("fadeIn");
        mainMenuTitleAnimator.SetTrigger("shrink");
    }

    public void BackToMainMenu() {
        mainMenuGroupAnimator.SetTrigger("fadeIn");
        levelSelectGroupAnimator.SetTrigger("fadeOut");
        mainMenuTitleAnimator.SetTrigger("grow");
    }

}
