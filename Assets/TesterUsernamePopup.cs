using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using TMPro;
using UnityEngine;

public class TesterUsernamePopup : MonoBehaviour {

    private static bool usernameReceived;

    public CanvasGroup mainMenuCanvasGroup;
    public TMP_InputField usernameInputField;

    void Awake() {
        if (usernameReceived) {
            gameObject.SetActive(false);
            return;
        }
        
        mainMenuCanvasGroup.interactable = false;
    }

    public void Begin() {
        string username = usernameInputField.text;

        if (username.Length == 0 || username.Length > 20) {
            return;
        }
        
        GameAnalytics.SetCustomId(username);
        GameAnalytics.Initialize();

        usernameReceived = true;

        mainMenuCanvasGroup.interactable = true;
        gameObject.SetActive(false);
    }

}
