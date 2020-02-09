using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class LevelGUIBackground : MonoBehaviour
{
    public void BackgroundClick() {
        Debug.Log("Background clicked");
        GameManager.levelScene.guiManager.CloseControlSettingsWindow();
    }
}
